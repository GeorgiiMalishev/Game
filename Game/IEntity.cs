using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public interface IEntity
{
    public void Update(Keys key, GameTime gameTime);

    public void Draw(SpriteBatch spriteBatch);
}