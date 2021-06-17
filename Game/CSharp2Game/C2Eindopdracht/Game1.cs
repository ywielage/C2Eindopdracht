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
        private List<Enemy> enemies;
        private Level level;

        private Texture2D blankTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            enemies = new List<Enemy>();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera();
            player = new Player(50, 220, .3f, 200f);
            enemies.Add(new LightEnemy(50, 220, .3f, 200f));
            
            level = new Level(5, 5);
            level.init(false);
            level.drawLevelInDebug();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Level.tileSet = Content.Load<Texture2D>("");
            Player.tileSet = Content.Load<Texture2D>("character1");
            LightEnemy.tileSet = Content.Load<Texture2D>("enemy1wit");
            HeavyEnemy.tileSet = Content.Load<Texture2D>("enemy1wit");

            blankTexture = Content.Load<Texture2D>("blankTexture");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var keyboardState = SmartKeyboard.GetState();

            player.checkCollisions(level.list);

            if (keyboardState.IsKeyDown(Keys.A))
            {
                player.moveLeft(gameTime);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                player.moveRight(gameTime);
            }

            if (SmartKeyboard.HasBeenPressed(Keys.Space))
            {
                player.jump(-6f, 3f);
            }

            if (SmartKeyboard.HasBeenPressed(Keys.J) && player.canAttack)
            {
                player.attack(1, new Cooldown(.5f), .2f, new Rectangle((int)player.getPosition().X, (int)player.getPosition().Y, 25, 25), 5);                
            }

            player.updateAttacks(gameTime);
            player.alignHitboxToPosition();

            //character.printCharacterValues();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);
            camera.Pos = player.getPosition();
            camera.Zoom = .8f;

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(GraphicsDevice));
            _spriteBatch.Draw(
                Player.tileSet,
                player.getPosition(),
                Color.White
            );

            foreach (Attack attack in player.attacks)
            {
                _spriteBatch.Draw(
                    blankTexture,
                    attack.hitbox,
                    Color.Red
                );
            }

            foreach (List<LevelComponent> rowList in level.list)
            {
                foreach (LevelComponent levelComponent in rowList)
                {
                    foreach(Rectangle wall in levelComponent.colliders)
                    {
                        _spriteBatch.Draw(
                           blankTexture,
                           wall,
                           Color.DarkSlateGray
                       );
                    }
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
