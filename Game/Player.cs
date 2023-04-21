using System;
using System.Net.Mime;
using System.Net.Security;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class Player
{
    private Texture2D texture;
    private Vector2 position = new Vector2(300, 300);
    private Vector2 velocity = new Vector2(10, 0);
    private float fallTime;
    private bool isOnGround;
    private bool isJumping;
    private float jumpScale = 100;
    private double manaScore = 100;
    private double hpScore = 100;
    private int direction;
    private float jumpTime;
    private const float MaxJumpTime = 0.35f;
    private const float JumpLaunchVelocity = -35.0f;
    private const float JumpControlPower = 0.14f;
    private bool wasJumping;
    private float maxYVelocity = 500f;


    public void Initialize(ContentManager content)
    {
        texture = content.Load<Texture2D>("Images/player");
    }
    public void Update(Keys key, GameTime gameTime)
    {
        isOnGround = position.Y >= 350;
        switch (key)
        {
            case Keys.A:
                direction = -1;
                break;
            case Keys.D:
                direction = 1;
                break;
            case Keys.Space:
                isJumping = isOnGround || isJumping;
                break; 
            case Keys.E:
                //make fireaball
                break;
        }

        if (isJumping)
           DoJump(gameTime);
        else
        {
            jumpTime = 0;
            velocity.Y = 0;
        }

        if (isOnGround || isJumping)
        {
            fallTime = 0;
            if (!isJumping)
                velocity.Y = 0;
        }
        else
            DoFall(gameTime);

        position.X += velocity.X * direction;
        position.Y += velocity.Y;
        direction = 0;
    }

    private void DoFall(GameTime gameTime)
    {
        velocity.Y += gameTime.ElapsedGameTime.Milliseconds * fallTime * 0.0007f;
        fallTime += gameTime.ElapsedGameTime.Milliseconds;
    }

    private void DoJump(GameTime gameTime)
    {
        if ((!wasJumping && isOnGround) || jumpTime > 0.0f)
            jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        wasJumping = isJumping;
        
        if (jumpTime is > 0.0f and <= MaxJumpTime)
            velocity.Y = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
        else
        {
            jumpTime = 0.0f;
            velocity.Y = 0;
            isJumping = false;
        }

        
    }

    public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, position, Color.White);
    

}