using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;

namespace Game;

public class Enemy : IElement
{
    private static Texture2D texture;
    public Vector2 Position;
    private static SpriteSheet _spriteSheet;
    private string _animation;
    public Rectangle Hitbox => new((int)Position.X - 15, (int)Position.Y - 15, 60, 60);
    private int hp = 100;
    private static SpriteFont font;
    private bool isRightDirection = false;
    private bool isDamaged;
    private Color defaultColor = Color.Coral;
    private Color damagedColor = Color.Brown;
    private Color currentColor;
    private int colorCd = 30;
    private Vector2 velocity = new Vector2(3, 0);
    private int fallTime;

    public float CooldownCounter = new Random().Next(2000);
    public readonly float Cooldown = 2000;
    public Enemy(Vector2 position)
    {
        Position = position;
    }

    public static void LoadContent(ContentManager content)
    {
        texture = content.Load<Texture2D>("Images/enemy");
        font = content.Load<SpriteFont>("Fonts/simplefont");
    }

    public void Update(GameTime gameTime, Level level)
    {
        DoMove(level.Plates, gameTime);
        
        foreach (var fireball in level.PlayerAttack.Where(a => a.Hitbox.Intersects(Hitbox)))
        {
            hp -= fireball.Damage;
            fireball.IsExist = false;
            isDamaged = true;
        }

        currentColor = isDamaged ? damagedColor : defaultColor;

        if (colorCd == 0)
        {
            isDamaged = false;
            colorCd = 30;
        }

        colorCd = colorCd - 1 >= 0 ? colorCd - 1 : colorCd;

        if (CooldownCounter >= 0)
            CooldownCounter -= gameTime.ElapsedGameTime.Milliseconds;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(font, $"hp: {hp}", new Vector2(Hitbox.X, Hitbox.Y) - new Vector2(30, 30), Color.Black);
        spriteBatch.Draw(texture, Hitbox, currentColor);
    }
    
    public bool IsExist()
    {
        return hp > 0;
    }

    private void DoMove(List<Plate> plates, GameTime gameTime)
    {
        float newXPosition;
        if (isRightDirection)
            newXPosition = Position.X + velocity.X;
        else
            newXPosition = Position.X - velocity.X;
        if (plates.Any(p => p.IsStayOnPlate(new Rectangle((int)newXPosition - 15, (int)Position.Y - 15, 60, 60))))
            Position.X = newXPosition;
        else
            isRightDirection = !isRightDirection;
        
        if (plates.Any(plate => plate.IsStayOnPlate(Hitbox)))
        {
            fallTime = 0;
            velocity.Y = 0;
        }
        
        else
        {
            velocity.Y += gameTime.ElapsedGameTime.Milliseconds * fallTime * 0.0007f;
            fallTime += gameTime.ElapsedGameTime.Milliseconds;
        }

        Position.Y += velocity.Y;
    }
    
   
}