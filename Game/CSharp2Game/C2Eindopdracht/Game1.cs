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
        private Character character;
        private Texture2D characterTexture;
        private Texture2D blankTexture;
        private Level level;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera();
            character = new Character(0, 220, .3f);
            
            level = new Level(5, 5);
            level.init(false);
            level.drawLevelInDebug();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            characterTexture = Content.Load<Texture2D>("character1");
            blankTexture = Content.Load<Texture2D>("blankTexture");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var keyboardState = SmartKeyboard.GetState();

            character.checkCollisions(level.list);

            if (keyboardState.IsKeyDown(Keys.A))
            {
                character.moveLeft(gameTime);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                character.moveRight(gameTime);
            }

            if (SmartKeyboard.HasBeenPressed(Keys.Space))
            {
                character.jump(-4.5f, 3f);
            }

            if (SmartKeyboard.HasBeenPressed(Keys.J) && character.canAttack)
            {
                character.attack(1, new Cooldown(.5f), .2f, new Rectangle((int)character.getPosition().X, (int)character.getPosition().Y, 25, 25), 5);                
            }

            character.updateAttacks(gameTime);
            character.alignHitboxToPosition();

            //character.printCharacterValues();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);
            camera.Pos = character.getPosition();
            camera.Zoom = .2f;

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(GraphicsDevice));
            _spriteBatch.Draw(
                characterTexture,
                character.getPosition(),
                Color.White
            );

            foreach (Attack attack in character.attacks)
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
