using System;
using System.Collections.Generic;
using System.Linq;
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
    private List<Fireball> fireballs = new List<Fireball>();
    private int lastDirection;
    private float fireballCooldown;

    private static readonly Vector2 border = new Vector2(720, 400);


    public void Initialize(ContentManager content)
    {
        texture = content.Load<Texture2D>("Images/player");
    }
    public void Update(Keys[] keys, GameTime gameTime)
    {
        isOnGround = position.Y >= 390;
        if (keys.Contains(Keys.A))
                direction = -1;
        if (keys.Contains(Keys.D))
                direction = 1;
        if (keys.Contains(Keys.Space) || keys.Contains(Keys.W))
            isJumping = isOnGround || isJumping;
        if (keys.Contains(Keys.E) && fireballCooldown == 0)
        {
            fireballs.Add(new Fireball(position, lastDirection));
            fireballCooldown = 1000;
        }

        DoJump(gameTime);
        DoFall(gameTime);
        DoMove();

        if (direction != 0)
            lastDirection = direction;
        direction = 0;
        
        fireballs.ForEach(fireball => fireball.Update(gameTime));
        fireballs = fireballs.Where(fireball => fireball.IsExist()).ToList();
        fireballCooldown = fireballCooldown > 0 
            ? fireballCooldown - gameTime.ElapsedGameTime.Milliseconds 
            : 0;
    }

    private void DoMove()
    {
        var newXPosition = velocity.X * direction + position.X;
        position.X = newXPosition > border.X || newXPosition < 0
            ? position.X
            : newXPosition;

        var newYPosition = position.Y + velocity.Y;
        position.Y = newYPosition > border.Y || newYPosition < 0
            ? position.Y
            : newYPosition;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, Color.White);
        
        fireballs.ForEach(fireball => fireball.Draw(spriteBatch));
    }

    private void DoFall(GameTime gameTime)
    {
        if (isOnGround || isJumping)
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