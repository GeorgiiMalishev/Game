using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class Player
{
    private readonly Texture2D texture;
    private Vector2 position;
    private int velocity;
    private float fallTime;
    private bool isOnGround = true;
    private float jumpScale = 10;
    private double manaScore = 100;
    private double hpScore = 100;
    private Direction direction;

    public Player(Texture2D texture, Vector2 position, int velocity)
    {
        this.texture = texture;
        this.position = position;
        this.velocity = velocity;
    }

    public void Update(Keys key, GameTime gameTime)
    {
        switch (key)
        {
            case Keys.A:
                position.X -= velocity;
                direction = Direction.Left;
                break;
            case Keys.D:
                position.X += velocity;
                direction = Direction.Right;
                break;
            case Keys.Space:
                if (isOnGround)
                {
                    position.Y += jumpScale;
                    isOnGround = false;
                }
                break;
            case Keys.E:
                MakeShot();
                break;
        }
        position.Y += 9.8f * gameTime.ElapsedGameTime.Milliseconds * velocity * fallTime * 0.0003f;
        fallTime += gameTime.ElapsedGameTime.Milliseconds;

        isOnGround = position.Y >= 1000;
        
        if (isOnGround)
            fallTime = 0;
        else
            fallTime += gameTime.ElapsedGameTime.Milliseconds;
    }
    
    public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, position, Color.White);

    public void MakeShot()
    {
        const int shotCost = 40;
        if (manaScore - shotCost < 0) 
            return;
        var shot = new Shot(position, direction);

    }
}