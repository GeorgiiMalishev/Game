using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class Fireball
{
    public static Texture2D Texture;
    public const int Damage = 10;
    private Vector2 position;
    private static float velocity = 0.03f;
    private int direction;
    private float flyTime;
    
    
    public Fireball(Vector2 position, int direction)
    {
        this.position = position;
        this.direction = direction;
    }
    
    
    public void Update(GameTime gameTime)
    {
        flyTime += gameTime.ElapsedGameTime.Milliseconds;
        position.X += direction * velocity * flyTime;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, position, Color.White);
    }

    public bool IsExist()
    {
        return position.X is <= 750 and >= 0
               && direction != 0;
    }
}