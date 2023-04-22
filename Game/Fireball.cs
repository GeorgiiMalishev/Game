using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game;

public class Fireball:IElement
{
    private static Texture2D _texture;
    public const int Damage = 10;
    public const int ManaCost = 70;
    public static int CooldownCounter;
    public const int Cooldown = 200;
    
    private Vector2 position;
    private const float Velocity = 0.03f;
    private readonly int direction;
    private float flyTime;

    private Rectangle hitbox => new Rectangle((int)position.X - 15, (int)position.Y - 15, 30, 30);

    public Fireball(Vector2 position, int direction)
    {
        this.position = position;
        this.direction = direction;
    }

    public static void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("Images/fireball");
    }
    
    public void Update(GameTime gameTime)
    {
        Move(gameTime);
        CooldownCounter = CooldownCounter > 0 
            ? CooldownCounter - gameTime.ElapsedGameTime.Milliseconds 
            : 0;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, hitbox, Color.White);
    }

    private void Move(GameTime gameTime)
    {
        flyTime += gameTime.ElapsedGameTime.Milliseconds;
        position.X += direction * Velocity * flyTime;
    }

    private bool IsExist()
    {
        return position.X is <= 800 and >= 0;
    }
}