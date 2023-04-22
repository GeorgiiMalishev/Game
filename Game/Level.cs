using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Game;

public class Level
{
    private string levelInString;
    private List<IElement> elements = new ();
    public List<Plate> Plates = new ();
    public Player Player;

    public Level(string levelInString)
    {
        this.levelInString = levelInString;
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
                case '\n' or '\r':
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
    }

    public void Update(GameTime gameTime)
    {
        elements.ForEach(element => element.Update(gameTime));
        elements = elements.Where(element => element.IsExist()).ToList();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        elements.ForEach(element => element.Draw(spriteBatch));
        Player.Draw(spriteBatch);
    }

    public void CreateFireball()
    {
        if (Fireball.CooldownCounter != 0 || Player.ManaScore - Fireball.ManaCost < double.Epsilon) 
            return;
        elements.Add(new Fireball(Player.Position, Player.LastDirection));
        Fireball.CooldownCounter  = Fireball.Cooldown;
        Player.ManaScore -= Fireball.ManaCost;
    }
}