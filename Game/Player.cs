using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Content;

namespace Game;

public class Player : IElement
{
    private static Texture2D texture;
    public Vector2 Position;
    private Vector2 velocity = new (4, 0);
    private Rectangle Hitbox => new ((int)Position.X - 15, (int)Position.Y - 15, 30, 30);

    private float fallTime;
    private bool isOnGround;
    
    private bool isJumping;
    private bool wasJumping;
    private float jumpTime;
    private const float MaxJumpTime = 0.35f;
    private const float JumpLaunchVelocity = -35.0f;
    private const float JumpControlPower = 0.14f;

    public double ManaScore = 100;
    private const int MaxMana = 100;
    private const double ManaRatio = 0.7;
    
    //private int hpScore = 100;
    
    private int direction;
    public int LastDirection = 1;

    private static readonly Vector2 Border = new Vector2(1920, 1080);
    
    private static SpriteFont font;
    
    public Player(Vector2 position)
    {
        this.Position = position;
    }
    

    public static void  LoadContent(ContentManager content)
    {
        texture = content.Load<Texture2D>("Images/player");
        font = content.Load<SpriteFont>("Fonts/simplefont");
    }
    public void Update(Keys[] keys, GameTime gameTime, Level level)
    {
        isOnGround = Hitbox.GetCorners()[3].Y >= Border.Y || level.Plates.Any(plate => plate.IsStayOnPlate(Hitbox));
        
        if (keys.Contains(Keys.A))
                direction = -1;
        if (keys.Contains(Keys.D))
                direction = 1;
        if (keys.Contains(Keys.Space) || keys.Contains(Keys.W))
            isJumping = isOnGround || isJumping;
        if (keys.Contains(Keys.E))
            level.CreateFireball();

        DoJump(gameTime);
        DoFall(gameTime, level);
        DoMove();
        
        
        if (direction != 0)
            LastDirection = direction;
        direction = 0;
        
        ManaScore = ManaScore + ManaRatio >= MaxMana 
            ? ManaScore 
            : ManaScore + ManaRatio;
    }

    private void DoMove()
    {
        var newXPosition = velocity.X * direction + Position.X;
        Position.X = newXPosition >= Border.X - velocity.X || newXPosition <= velocity.X
            ? Position.X
            : newXPosition;

        var newYPosition = Position.Y + velocity.Y;
        Position.Y = newYPosition >= Border.Y - 1 || newYPosition <= 1
            ? Position.Y
            : newYPosition;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Hitbox, Color.White);
        spriteBatch.DrawString(font,$"mana: {(int)ManaScore}", Position + new Vector2(-15, -30), Color.Black); 
    }

    private void DoFall(GameTime gameTime, Level level)
    {
        if (isOnGround || isJumping || level.Plates.Any(plate => plate.IsStayOnPlate(Hitbox)))
        {
            fallTime = 0;
            if (!isJumping)
                velocity.Y = 0;
        }
        
        else
        {
            velocity.Y += gameTime.ElapsedGameTime.Milliseconds * fallTime * 0.0007f;
            fallTime += gameTime.ElapsedGameTime.Milliseconds; 
        }
    }

    private void DoJump(GameTime gameTime)
    {
        if (isJumping)
        {
            if ((!wasJumping && isOnGround) || jumpTime > 0.0f)
                jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (jumpTime is > 0.0f and <= MaxJumpTime)
                velocity.Y = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
            
            else
            {
                jumpTime = 0.0f;
                velocity.Y = 0;
                isJumping = false;
            }
        
            wasJumping = isJumping;
        }
        
        else
        {
            jumpTime = 0;
            velocity.Y = 0;
        }
    }
}