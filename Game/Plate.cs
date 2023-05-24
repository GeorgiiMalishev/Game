using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Game;

public class Plate : IElement
{
    private Vector2 position;
    private static Texture2D texture;
    public Rectangle Border;
    
    private static List<Plate> _plates = new ();

    public Plate(Vector2 position)
    {
        this.position = position;
        Border = new Rectangle((int)position.X, (int)position.Y, 100, 30);
    }

    public static void LoadContent(ContentManager content)
    {
        texture = content.Load<Texture2D>("Images/enemy");
    }

    public bool IsStayOnPlate(Rectangle entityBorder)
    {
       var entityBottomCorners = entityBorder.GetCorners().Skip(2).ToArray();
       var plateTopCorners = Border.GetCorners().Take(2).ToArray();
       return entityBottomCorners.Any(corner =>
                  corner.X >= plateTopCorners[0].X
                  && corner.X <= plateTopCorners[1].X)
              && entityBottomCorners[0].Y - plateTopCorners[0].Y >= 0 
              && entityBottomCorners[0].Y - plateTopCorners[0].Y <= 6 ;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Border, Color.Gray);
    }

    public bool IsConflicting()
    {
        return true;
    }
}