using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Game;

public class Plate
{
    private Vector2 position;
    private Texture2D texture;
    public readonly Rectangle Border;

    public Plate(Vector2 position, Texture2D texture)
    {
        this.position = position;
        this.texture = texture;
        Border = new Rectangle((int)position.X, (int)position.Y, 30, 30);
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

    public bool IsConflicting()
    {
        return true;
    }
}