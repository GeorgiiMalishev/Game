using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using static Game.GameStates;

namespace Game;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private Level _level;
    
    private GameStates gameState = NotStarted;
    private GameStates previosGameState = Running;

    private Menu gameMenu;
    private Menu mainMenu;
    private Menu deathMenu;
    private double menuCd;
    
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
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);


        gameMenu = new Menu(new List<Button>()
        {
            new(new Rectangle(400, 150, 150, 50), Content.Load<Texture2D>("Images/enemy"),
                Content.Load<SpriteFont>("Fonts/simplefont"), "Continue", Color.Brown),
            new(new Rectangle(400, 250, 150, 50), Content.Load<Texture2D>("Images/enemy"),
                Content.Load<SpriteFont>("Fonts/simplefont"), "Settings", Color.Brown),
            new(new Rectangle(400, 350, 150, 50), Content.Load<Texture2D>("Images/enemy"),
                Content.Load<SpriteFont>("Fonts/simplefont"), "Main Menu", Color.Brown)

        });
        
        mainMenu = new Menu(new List<Button>()
        {
            new(new Rectangle(50, 150, 250, 100), Content.Load<Texture2D>("Images/enemy"),
                Content.Load<SpriteFont>("Fonts/simplefont"), "Start Game", Color.Gold),
            new(new Rectangle(50, 320, 250, 100), Content.Load<Texture2D>("Images/enemy"),
                Content.Load<SpriteFont>("Fonts/simplefont"), "Settings", Color.Gold),
            new(new Rectangle(50, 490, 250, 100), Content.Load<Texture2D>("Images/enemy"),
                Content.Load<SpriteFont>("Fonts/simplefont"), "Exit", Color.Gold)

        });
        
        deathMenu = new Menu(new List<Button>()
        {
            new(new Rectangle(400, 500, 200, 75), Content.Load<Texture2D>("Images/enemy"),
                Content.Load<SpriteFont>("Fonts/simplefont"), "Continue", Color.Lime),
            new(new Rectangle(650, 500, 200, 75), Content.Load<Texture2D>("Images/enemy"),
                Content.Load<SpriteFont>("Fonts/simplefont"), "Main Menu", Color.Indigo),

        });
    }
    
    protected override void Update(GameTime gameTime)
    {
        if (mainMenu.Buttons[0].IsPressed())
        {
            LoadLevel("Levels/level1.txt", 1);
        }

        else if(mainMenu.Buttons[2].IsPressed())
        {
            Exit();
        }
        
        if (deathMenu.Buttons[0].IsPressed())
            if (_level.IsComplete())
                LoadLevel($"Levels/level{_level.Id + 1}.txt", _level.Id + 1);
            else
                LoadLevel($"Levels/level{_level.Id}.txt", _level.Id);
        var pressedKeys = Keyboard.GetState().GetPressedKeys();

        if (menuCd >= 200 && pressedKeys.Contains(Keys.Escape) && gameState != NotStarted)
        {
            gameState = gameState == Paused ? Running : Paused;
            menuCd = 0;
        }

        menuCd = menuCd >= 200 ? menuCd : menuCd + gameTime.ElapsedGameTime.Milliseconds;
        
        if (gameMenu.Buttons[0].IsPressed())
            gameState = Running;
        else if (gameMenu.Buttons[2].IsPressed() || deathMenu.Buttons[1].IsPressed())
            gameState = NotStarted;
        if (gameState == Running)
            _level.Update(gameTime, pressedKeys);
        if (gameState != NotStarted && (!_level.Player.IsAlive || _level.IsComplete()))
            gameState = Over; 
        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin ();

        if (gameState == NotStarted)
            mainMenu.Draw(spriteBatch);
        else
            _level.Draw(spriteBatch);

        switch (gameState)
        {
            case Paused:
                gameMenu.Draw(spriteBatch);
                break;
            case Over:
                deathMenu.Draw(spriteBatch);
                break;
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }

    private void LoadLevel(string path, int id)
    {
        var levelInString = new StreamReader(path).ReadToEndAsync().Result;
        _level = new Level(levelInString, id);
        _level.LoadContent(Content);
        _level.Initialize();

        gameState = Running;
    }
}