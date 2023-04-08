using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class Player
{
    private readonly Texture2D texture;
    public Vector2 Position;
    public int Velocity;

    public Player(Texture2D texture, Vector2 position, int velocity)
    {
        this.texture = texture;
        Position = position;
        Velocity = velocity;
    }

    public void Update(Keys key)
    {
        switch (key)
        {
            case Keys.A:
                Position.X -= Velocity;
                break;
            case Keys.D:
                Position.X += Velocity;
                break;
        }
    }
    
    public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, Position, Color.White);
}