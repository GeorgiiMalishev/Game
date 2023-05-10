using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class Menu
{
    public List<Button> Buttons;

    public Menu(List<Button> buttons)
    {
        Buttons = buttons;
    }
    
    public void Draw(SpriteBatch spriteBatch) => Buttons.ForEach(button => button.Draw(spriteBatch));
}

