using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private Level _level;
    
    //private List<Plate> = new List<Plate>{new Plate()}

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1920;
        graphics.PreferredBackBufferHeight = 1080;
        graphics.ToggleFullScreen();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        var levelInString = new StreamReader("Levels/testlevel.txt").ReadToEndAsync().Result;
        _level = new Level(levelInString);
        _level.Initialize();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        _level.LoadContent(Content);
    }
    
    protected override void Update(GameTime gameTime)
    {
        var keyPressed = Keys.None;
        var pressedKeys = Keyboard.GetState().GetPressedKeys();
        _level.Update(gameTime);
        _level.Player.Update(pressedKeys, gameTime, _level);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        _level.Draw(spriteBatch);
        spriteBatch.End();
        base.Draw(gameTime);
    }
}