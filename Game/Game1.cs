using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private Player player;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        player = new Player(Content.Load<Texture2D>("Images/player"), new Vector2(300, 300), 10);
        Fireball.Texture = Content.Load<Texture2D>("Images/fireball");
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }
    
    protected override void Update(GameTime gameTime)
    {
        var keyPressed = Keys.None;
        var pressedKeys = Keyboard.GetState().GetPressedKeys();
        if (pressedKeys.Length != 0)
             keyPressed = pressedKeys[0];
        player.Update(keyPressed, gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        player.Draw(spriteBatch);
        spriteBatch.End();
        base.Draw(gameTime);
    }
}