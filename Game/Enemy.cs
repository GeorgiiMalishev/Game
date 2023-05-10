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
        foreach (var fireball in level.PlayerAttack.Where(a => a.Hitbox.Intersects(Hitbox)))
        {
            hp -= fireball.Damage;
            fireball.IsExist = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(font, $"hp: {hp}", position - new Vector2(15, 15), Color.Black);
        spriteBatch.Draw(texture, Hitbox, Color.Beige);
    }
    
    public bool IsExist()
    {
        return hp > 0;
    }
}