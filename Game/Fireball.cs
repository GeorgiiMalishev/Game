using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class Fireball
{
    public static Texture2D Texture;
    public const int Damage = 10;
    private const int ManaCost = 40;
    private static int cooldown;
    
    private Vector2 position;
    private static float velocity = 0.03f;
    private int direction;
    private float flyTime;
    private static List<Fireball> fireballs = new ();

    private Fireball(Vector2 position, int direction)
    {
        this.position = position;
        this.direction = direction;
    }
    
    public static void Update(GameTime gameTime)
    {
        fireballs.ForEach(fireball => fireball.Move(gameTime));
        fireballs = fireballs.Where(fireball => fireball.IsExist()).ToList();
        cooldown = cooldown > 0 
            ? cooldown - gameTime.ElapsedGameTime.Milliseconds 
            : 0;
    }
    
    public static void Draw(SpriteBatch spriteBatch)
    {
        fireballs.ForEach(fireball => spriteBatch.Draw(Texture, fireball.position, Color.White));
    }

    private void Move(GameTime gameTime)
    {
        flyTime += gameTime.ElapsedGameTime.Milliseconds;
        position.X += direction * velocity * flyTime;
    }

    private bool IsExist()
    {
        return position.X is <= 750 and >= 0
               && direction != 0;
    }

    public static void Create(Vector2 playerPosition, int direction, ref int manaScore)
    {
        if (cooldown != 0 || manaScore - ManaCost < 0) 
            return;
        fireballs.Add(new Fireball(playerPosition, direction));
        cooldown = 200;
        manaScore -= ManaCost;
    }
}