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
    private bool isOnGround;
    private bool isJumping;
    private float jumpScale = 100;
    private double manaScore = 100;
    private double hpScore = 100;
    private Direction direction;
    private float jumpTime;

    public Player(Texture2D texture, Vector2 position, int velocity)
    {
        this.texture = texture;
        this.position = position;
        this.velocity = velocity;
    }

    public void Update(Keys key, GameTime gameTime)
    {
        isOnGround = position.Y >= 350;
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
                isJumping = isOnGround || isJumping;
                break; 
            case Keys.E:
                //make fireaball
                break;
        }

        if (isJumping)
        {
            position.Y -= gameTime.ElapsedGameTime.Milliseconds * 0.0040f;
            jumpTime += gameTime.ElapsedGameTime.Milliseconds;
            isJumping = jumpTime <= 300;
        }
        else
            jumpTime = 0;
        
        if (isOnGround && !isJumping)
            fallTime = 0;
        else
        {
            position.Y += gameTime.ElapsedGameTime.Milliseconds * fallTime * 0.0007f;
            fallTime += gameTime.ElapsedGameTime.Milliseconds;
        }
    }
    
    public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, position, Color.White);
    

}