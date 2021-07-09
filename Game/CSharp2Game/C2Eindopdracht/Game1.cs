using C2Eindopdracht.Classes;
using C2Eindopdracht.Classes.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace C2Eindopdracht
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Camera camera;
        private Player player;
        private Level level;
        private UI ui;
        private bool renderHitboxes;
        private SpriteFont arial16;
        private MenuScreen menuScreen;
        private PauseScreen pauseScreen;
        private GameState gameState;
        private bool keyDown;

        //Texture mainly for drawing hitboxes
        public static Texture2D blankTexture;

        /// <summary>
        /// Constructor of game. Initializes GraphicsDeviceManager and gives Content a directory
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        /// <summary>
        /// Initializes camera that follows the player, the player itself, the level, creates UI and screens
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gameState = GameState.MENU;
            keyDown = false;

            camera = new Camera
			{
				Zoom = 1.1f
			};

            level = new Level(4, 4, 8);
            level.init(false);
            level.drawLevelInDebug();

            player = new Player(20 + level.levelComponentSize, 170 + level.levelComponentSize, 18, 30, 20);

            intialUI();
            initialScreens();

            base.Initialize();
        }
        /// <summary>
        /// Loads the content
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            LevelComponent.tileSet = Content.Load<Texture2D>("tileset-map-squared");
            Player.tileSet = Content.Load<Texture2D>("character1");
            MageEnemy.tileSet = Content.Load<Texture2D>("enemyMage");
            FighterEnemy.tileSet = Content.Load<Texture2D>("enemyFighter");
            Projectile.tileSet = Content.Load<Texture2D>("fireball");
            MenuScreen.tileSet = Content.Load<Texture2D>("startImage");
            arial16 = Content.Load<SpriteFont>("fonts/Arial16");

            blankTexture = Content.Load<Texture2D>("blankTexture");
        }
        /// <summary>
        /// Updates up to 60 times a second. Changes depending on gameState. 
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                 Exit();

            checkGameStateKeyPress();

            if (SmartKeyboard.HasBeenPressed(Keys.F))
                _graphics.ToggleFullScreen();

            if (SmartKeyboard.HasBeenPressed(Keys.Tab))
                renderHitboxes = !renderHitboxes;

            if (gameState == GameState.GAME)
            {
                // TODO: Add your update logic here
                player.update(gameTime, level.list, level.enemies, (UIElementLabelValue)ui.getUIElementByLabel("Enemies alive"));

                foreach (Enemy enemy in level.enemies)
                {
                    enemy.update(gameTime, level.list, player, ui);
                }

                ui.update(new Vector2(player.position.X - _graphics.PreferredBackBufferWidth / (2 * camera.Zoom), player.position.Y - _graphics.PreferredBackBufferHeight / (2 * camera.Zoom)), gameTime);
                camera.Pos = player.position;
                level.checkEndTriggerHit(player.hitbox, ui, 5, 50);
            } else if(gameState == GameState.PAUSE)
            {
                pauseScreen.update(new Vector2(player.position.X - _graphics.PreferredBackBufferWidth / (2 * camera.Zoom), player.position.Y - _graphics.PreferredBackBufferHeight / (2 * camera.Zoom)), gameTime);
            }
        }

        /// <summary>
        /// Draws all elements. The elements that are drawn depend on gameState.
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(GraphicsDevice));

            if (gameState == GameState.GAME)
            {
                drawGame();
            }
            else if(gameState == GameState.PAUSE)
            {
                pauseScreen.draw(_spriteBatch, arial16);
            }
            else if(gameState == GameState.MENU)
            {
                menuScreen.draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw the game
        /// </summary>
        private void drawGame()
		{
            foreach (UIElement element in ui.elements)
            {
                element.draw(_spriteBatch, arial16);
            }
            foreach (Enemy enemy in level.enemies)
            {
                enemy.draw(_spriteBatch, renderHitboxes);
            }
            player.draw(_spriteBatch, renderHitboxes);
            level.draw(_spriteBatch, renderHitboxes);
        } 
        

        /// <summary>
        /// Initial UI the game starts with
        /// </summary>
        private void intialUI()
        {
            ui = new UI(new Vector2(player.position.X - _graphics.PreferredBackBufferWidth / 2, player.position.Y - _graphics.PreferredBackBufferHeight / 2));
            ui.addUIElement("Enemies alive", level.enemies.Count, new Vector2(5, 5), 0);
            ui.addUIElement("To move press A and D, to jump press Space or W", new Vector2(5, 50), 10f);
            ui.addUIElement("To attack press J, to shield press K", new Vector2(5, 70), 10f);
            ui.addUIElement("In shield you're not able to attack but can dodge enemy attacks", new Vector2(5, 90), 10f);
            ui.addUIElement("You can toggle the render mode by pressing Tab", new Vector2(5, 130), 10f);
            ui.addUIElement("You can toggle fullscreen by pressing F", new Vector2(5, 150), 10f);
            ui.addUIElement("You can toggle pause by pressing Y", new Vector2(5, 170), 10f);
        }

        /// <summary>
        /// Initial screens the game starts with
        /// </summary>
        private void initialScreens()
		{
            menuScreen = new MenuScreen(new Rectangle(-130, -75, 254, 104));
            pauseScreen = new PauseScreen(new Vector2(player.position.X - _graphics.PreferredBackBufferWidth / 2, player.position.Y - _graphics.PreferredBackBufferHeight / 2));
            pauseScreen.ui.addUIElement("Press Y to continue playing", new Vector2(230, 200), 0);
        }

        /// <summary>
        /// Check key presses for screen and gamestate changes
        /// </summary>
        private void checkGameStateKeyPress()
		{
            if (gameState == GameState.MENU)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    gameState = GameState.GAME;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Y) && !keyDown && gameState != GameState.MENU)
            {
                toggleGameStateBetweenMenuAndGame();
                keyDown = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Y) && keyDown)
                keyDown = false;
        }

        /// <summary>
        /// Toggle from gamestate game to pause and pause to game
        /// </summary>
        private void toggleGameStateBetweenMenuAndGame()
		{
            if (gameState == GameState.GAME)
            {
                gameState = GameState.PAUSE;
            }
            else
            {
                gameState = GameState.GAME;
            }
        }
    }

    public enum GameState
    {
        PAUSE,
        GAME,
        MENU
    }
}
