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
    private Vector2 position;
    private  AnimatedSprite _sprite;
    private static SpriteSheet _spriteSheet;
    private string _animation;
    public Rectangle Hitbox => new((int)position.X - 15, (int)position.Y - 15, 30, 30);
    private int hp = 100;
    private static SpriteFont font;
    private bool isRightDirection = true;
    private bool isDamaged;
    private Color defaultColor = Color.Coral;
    private Color damagedColor = Color.Brown;
    private Color currentColor;
    private int colorCd = 30;
    private Vector2 velocity = new Vector2(5, 0);
    private int fallTime;

    public Enemy(Vector2 position)
    {
        this.position = position;
        _sprite = new AnimatedSprite(_spriteSheet);
        _sprite.Play("enemy2idle");
    }

    public static void LoadContent(ContentManager content)
    {
        _spriteSheet = content.Load<SpriteSheet>("sprites/enemy1.sf", new JsonContentLoader());
        texture = content.Load<Texture2D>("Images/player");
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

        var id = hp < 50 ? 2 : 1;
        var animation = isRightDirection ? $"enemy{id}Right" : $"enemy{id}Left";
        _sprite.Play(animation);
        _sprite.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(font, $"hp: {hp}", position - new Vector2(15, 15), Color.Black);
        spriteBatch.Draw(_sprite, position);
    }
    
    public bool IsExist()
    {
        return hp > 0;
    }

    private void DoMove(List<Plate> plates, GameTime gameTime)
    {
        float newXPosition;
        if (isRightDirection)
            newXPosition = position.X + velocity.X;
        else
            newXPosition = position.X - velocity.X;
        if (plates.Any(p => p.IsStayOnPlate(new Rectangle((int)newXPosition - 15, (int)position.Y - 15, 30, 30))))
            position.X = newXPosition;
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

        position.Y += velocity.Y;
    }
    
   
}