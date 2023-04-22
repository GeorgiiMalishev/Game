using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game;

public interface IElement
{
    public static void LoadContent(ContentManager content){}
    
    public void Update(GameTime gameTime){}
    
    public void Draw(SpriteBatch spriteBatch){}
    
    public bool IsExist()
    {
        return true;
    }
    
}