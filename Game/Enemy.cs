using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game;

public class Enemy : IElement
{
    private static Texture2D texture;
    private Vector2 position;
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
    }

    public static void LoadContent(ContentManager content)
    {
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
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(font, $"hp: {hp}", position - new Vector2(15, 15), Color.Black);
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