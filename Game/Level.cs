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
        
        Enemies.ForEach(e => e.Update(gameTime, this));
        Enemies = Enemies.Where(e => e.IsExist()).ToList();
        
        elements.ForEach(element => element.Update(gameTime));
        elements = elements.Where(element => element.IsExist()).ToList();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Enemies.ForEach(e => e.Draw(spriteBatch));
        elements.ForEach(element => element.Draw(spriteBatch));
        PlayerAttack.ForEach(f => f.Draw(spriteBatch));
        Player.Draw(spriteBatch);
    }

    public void CreateFireball()
    {
        if (Fireball.CooldownCounter != 0 || Player.ManaScore - Fireball.ManaCost < double.Epsilon) 
            return;
        PlayerAttack.Add(new Fireball(Player.Position + new Vector2(15, 15), Player.LastDirection));
        Fireball.CooldownCounter  = Fireball.Cooldown;
        Player.ManaScore -= Fireball.ManaCost;
    }

    public bool IsComplete()
    {
        return Enemies.Count == 0;
    }
}