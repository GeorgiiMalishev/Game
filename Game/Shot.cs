using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class Shot
{
    public static Texture2D Texture;
    public const int Damage = 10;
    private Vector2 position;
    private static int velocity = 5;
    private Direction direction;
    
    
    public Shot(Vector2 position, Direction direction)
    {
        this.position = position;
        this.direction = direction;
    }
    
    
    public void Update(GameTime gameTime)
    {
        position.X += (int)direction * velocity;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, position, Color.LightGreen);
    }
}