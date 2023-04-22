using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game;

public class Fireball
{
    public static Texture2D Texture;
    public const int Damage = 10;
    private const int ManaCost = 70;
    private static int _cooldown;
    private const int Cooldown = 200;
    
    private Vector2 position;
    private const float Velocity = 0.03f;
    private readonly int direction;
    private float flyTime;
    private static List<Fireball> _fireballs = new ();

    private Rectangle hitbox => new Rectangle((int)position.X - 15, (int)position.Y - 15, 30, 30);

    private Fireball(Vector2 position, int direction)
    {
        this.position = position;
        this.direction = direction;
    }
    
    public static void Update(GameTime gameTime)
    {
        _fireballs.ForEach(fireball => fireball.Move(gameTime));
        _fireballs = _fireballs.Where(fireball => fireball.IsExist()).ToList();
        _cooldown = _cooldown > 0 
            ? _cooldown - gameTime.ElapsedGameTime.Milliseconds 
            : 0;
    }
    
    public static void Draw(SpriteBatch spriteBatch)
    {
        _fireballs.ForEach(fireball => spriteBatch.Draw(Texture, fireball.hitbox, Color.White));
    }

    private void Move(GameTime gameTime)
    {
        flyTime += gameTime.ElapsedGameTime.Milliseconds;
        position.X += direction * Velocity * flyTime;
    }

    private bool IsExist()
    {
        return position.X is <= 800 and >= 0
               && direction != 0;
    }

    public static void Create(Vector2 playerPosition, int direction, ref double manaScore)
    {
        if (_cooldown != 0 || manaScore - ManaCost < double.Epsilon) 
            return;
        _fireballs.Add(new Fireball(playerPosition, direction));
        _cooldown = Cooldown;
        manaScore -= ManaCost;
    }
}