using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Game;

public class Button
{
    private Rectangle hitbox;
    private Texture2D texture;
    private SpriteFont font;
    private string text;
    
    private Color buttonColor;
    private Color previosColor;
    
    public Color textColor = Color.White;
    public Color PressedColor = Color.Gray;
    
    private Vector2 textPosition => new(hitbox.Center.X - font.MeasureString(text).X / 2, hitbox.Center.Y - font.MeasureString(text).Y / 2);

    public Button(Rectangle hitbox, Texture2D texture, SpriteFont font, string text, Color buttonColor)
    {
        this.hitbox = hitbox;
        this.texture = texture;
        this.font = font;
        this.text = text;
        this.buttonColor = buttonColor;
        previosColor = buttonColor;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        buttonColor = IsPressed() ? PressedColor : previosColor;
        
        spriteBatch.Draw(texture, hitbox, buttonColor);
        spriteBatch.DrawString(font, text, textPosition, textColor);
    }
    
    public bool IsPressed()
    {
        var mouseState = Mouse.GetState();
        var hitboxCorners = hitbox.GetCorners();
        return mouseState.LeftButton == ButtonState.Pressed
               && mouseState.X > hitboxCorners[0].X 
               && mouseState.X < hitboxCorners[2].X
               && mouseState.Y > hitboxCorners[0].Y 
               && mouseState.Y < hitboxCorners[2].Y;
    }
}