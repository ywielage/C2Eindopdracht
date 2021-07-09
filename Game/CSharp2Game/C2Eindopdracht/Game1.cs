using C2Eindopdracht.Classes;
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

        //Texture mainly for drawing hitboxes
        public static Texture2D blankTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
			// TODO: Add your initialization logic here
			camera = new Camera
			{
				Zoom = 1.1f
			};

            level = new Level(4, 4, 8);
            level.init(false);
            level.drawLevelInDebug();

            player = new Player(20 + level.levelComponentSize, 170 + level.levelComponentSize, 18, 30, 20);

            ui = new UI(new Vector2(player.position.X - _graphics.PreferredBackBufferWidth / 2, player.position.Y - _graphics.PreferredBackBufferHeight / 2));
            ui.addUIElement("Enemies alive", level.enemies.Count, new Vector2(5, 5), 0);
            ui.addUIElement("To move press A and D, to jump press Space or W, you can walljump too!", new Vector2(5, 50), 10f);
            ui.addUIElement("To attack press J, to shield press K", new Vector2(5, 70), 10f);
            ui.addUIElement("In shield you're not able to attack but can dodge enemy attacks", new Vector2(5, 90), 10f);
            ui.addUIElement("You can toggle the render mode by pressing Tab", new Vector2(5, 130), 10f);
            ui.addUIElement("You can toggle fullscreen by pressing F", new Vector2(5, 150), 10f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            LevelComponent.tileSet = Content.Load<Texture2D>("tileset-map-squared");
            Player.tileSet = Content.Load<Texture2D>("character1");
            MageEnemy.tileSet = Content.Load<Texture2D>("enemyMage");
            FighterEnemy.tileSet = Content.Load<Texture2D>("enemyFighter");
            Projectile.tileSet = Content.Load<Texture2D>("fireball");
            arial16 = Content.Load<SpriteFont>("fonts/Arial16");

            blankTexture = Content.Load<Texture2D>("blankTexture");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(SmartKeyboard.HasBeenPressed(Keys.Tab))
                renderHitboxes = !renderHitboxes;

            if(SmartKeyboard.HasBeenPressed(Keys.F))
                _graphics.ToggleFullScreen();

            // TODO: Add your update logic here
            player.update(gameTime, level.list, level.enemies, (UIElementLabelValue)ui.getUIElementByLabel("Enemies alive"));

            foreach(Enemy enemy in level.enemies)
			{
                enemy.update(gameTime, level.list, player, ui);
			}

            ui.update(new Vector2(player.position.X - _graphics.PreferredBackBufferWidth / (2 * camera.Zoom), player.position.Y - _graphics.PreferredBackBufferHeight / (2 * camera.Zoom)), gameTime);
            camera.Pos = player.position;
            level.checkEndTriggerHit(player.hitbox, ui, 5, 50);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(GraphicsDevice));

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

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
