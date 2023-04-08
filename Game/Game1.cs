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
        player = new Player(Content.Load<Texture2D>("Player"), new Vector2(0, 1000), 10);
        Shot.Texture = Content.Load<Texture2D>("Shot");
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }
    
    protected override void Update(GameTime gameTime)
    {
        var keyPressed = Keyboard.GetState().GetPressedKeys()[0];
        
        player.Update(keyPressed, gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        player.Draw(spriteBatch);
        base.Draw(gameTime);
    }
}