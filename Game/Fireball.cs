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
    private static int velocity = 5;
    private Direction direction;
    
    
    public Fireball(Vector2 position, Direction direction)
    {
        this.position = position;
        this.direction = direction;
    }
    
    
    public void Update(GameTime gameTime)
    {
        position.X += (int)direction * velocity;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        //spriteBatch.Draw(Texture, position, Color.LightGreen);
    }
}