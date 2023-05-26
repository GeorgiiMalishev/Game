using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Plane = System.Numerics.Plane;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Game;

public class Level
{
    public int Id;
    private string levelInString;
    private List<IElement> elements = new ();
    public List<Plate> Plates = new ();
    public List<Enemy> Enemies = new();
    public List<Fireball> PlayerAttack = new();
    public List<Fireball> EnemyAttack = new();
    public Player Player;

    public Level(string levelInString, int id)
    {
        this.levelInString = levelInString;
        Id = id;
    }

    public void Initialize()
    {
        var x = 0f;
        var y = 0f;
        foreach (var element in levelInString)
        {
            switch (element)
            {
                case 'P':
                    Player = new Player(new Vector2(x, y));
                    break;
                case '-':
                    elements.Add(new Plate(new Vector2(x, y)));
                    Plates.Add(new Plate(new Vector2(x, y)));
                    break;
                case 'E':
                    Enemies.Add(new Enemy(new Vector2(x, y)));
                    break;
                case '\n':
                    y+=10;
                    x = 0;
                    break;
            }
            x+=10;
        }
    }

    public void LoadContent(ContentManager content)
    {
       Fireball.LoadContent(content);
       Plate.LoadContent(content);
       Player.LoadContent(content);
       Enemy.LoadContent(content);
    }

    public void Update(GameTime gameTime, Keys[] keys)
    {
        Player.Update(keys, gameTime, this);
        
        PlayerAttack.ForEach(f => f.Update(gameTime));
        PlayerAttack = PlayerAttack.Where(f => f.IsExist).ToList();

        Enemies.ForEach(e =>
        {
            e.Update(gameTime, this);
            CreateEnemyFireball(e);
        }); 
        EnemyAttack.ForEach(f =>
        {
            f.Update(gameTime);
            if (!f.Hitbox.Intersects(Player.Hitbox)) return;
            Player.Hp -= f.Damage;
            f.IsExist = false;
        });
        Enemies = Enemies.Where(e => e.IsExist()).ToList();
        EnemyAttack = EnemyAttack.Where(f => f.IsExist).ToList();
        
        elements.ForEach(element => element.Update(gameTime));
        elements = elements.Where(element => element.IsExist()).ToList();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        EnemyAttack.ForEach(f => f.Draw(spriteBatch));
        Enemies.ForEach(e => e.Draw(spriteBatch));
        elements.ForEach(element => element.Draw(spriteBatch));
        PlayerAttack.ForEach(f => f.Draw(spriteBatch));
        Player.Draw(spriteBatch);
    }

    public void CreateFireball()
    {
        if (Player.ManaScore - Fireball.ManaCost < double.Epsilon) 
            return;
        PlayerAttack.Add(new Fireball(Player.Position + new Vector2(15, 15), Player.LastDirection));
        Fireball.CooldownCounter  = Fireball.Cooldown;
        Player.ManaScore -= Fireball.ManaCost;
    }
    
    public void CreateEnemyFireball(Enemy enemy)
    {
        if (enemy.CooldownCounter > 0) 
            return;
        EnemyAttack.Add(MakeTargetedFireball(enemy, Player));
        enemy.CooldownCounter  = enemy.Cooldown;
    }

    private Fireball MakeTargetedFireball(Enemy start, Player target)
    {
        var y1 = start.Position.Y;  
        var y2 = target.Position.Y;    
        var x1 = start.Position.X;  
        var x2 = target.Position.X;
        var direction = x1 > x2 ? -1 : 1;
        
        float xRatio;
        float yRatio; 
        if (Math.Abs(x1 - x2) < Double.Epsilon)
        {
            yRatio = 1;
            xRatio = 0;
        }
        else
        {
            yRatio = (y2 - y1) / (x1 - x2) * direction * -1;
            xRatio = 1;
        }

        return new Fireball(start.Position + new Vector2(15, 15), direction)
        {
            Color = Color.Orange,
            YRatio = yRatio,
            XRatio = xRatio
        };
    }

    public bool IsComplete()
    {
        return Enemies.Count == 0;
    }
}