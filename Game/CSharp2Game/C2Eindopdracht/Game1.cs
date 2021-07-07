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
        private SpriteFont Arial16;
        private bool renderHitboxes;

        //Texture mainly for drawing hitboxes
        private Texture2D blankTexture;

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
            camera.Zoom = 1.1f;
            
            renderHitboxes = false;
            
            //Level with (levelcomponents wide, levelcomponents high, amount of enemies)
            level = new Level(4, 4, 8);
            level.init(false);
            level.drawLevelInDebug();

            player = new Player(20 + level.levelComponentSize, 170 + level.levelComponentSize, 18, 30, 20);

            ui = new UI(new Vector2(player.getPosition().X - _graphics.PreferredBackBufferWidth / 2, player.getPosition().Y - _graphics.PreferredBackBufferHeight / 2));
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
            FighterEnemy.TileSet = Content.Load<Texture2D>("enemyFighter");
            Projectile.tileSet = Content.Load<Texture2D>("fireball");
            Arial16 = Content.Load<SpriteFont>("fonts/Arial16");

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

            ui.update(new Vector2(player.getPosition().X - _graphics.PreferredBackBufferWidth / (2 * camera.Zoom), player.getPosition().Y - _graphics.PreferredBackBufferHeight / (2 * camera.Zoom)), gameTime);
            camera.Pos = player.getPosition();
            level.checkEndTriggerHit(player.getHitbox(), ui, 5, 50);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(GraphicsDevice));

            //Draw every sprite element

            drawSpriteElements();

            if (renderHitboxes)
            {
                //Draw player hitbox
                drawPlayer();

                //Draw tile hitboxes
                foreach (List<LevelComponent> rowList in level.list)
                {
                    foreach (LevelComponent levelComponent in rowList)
                    {
                        foreach (Rectangle wall in levelComponent.colliders)
                        {
                            _spriteBatch.Draw(
                                blankTexture,
                                wall,
                                Color.DimGray
                            );
                        }
                    }
                }
            }
            else
            {
                //Draw player
                _spriteBatch.Draw(
                    Player.tileSet,
                    player.getPosition(),
                    Color.White
                );

                //Draw player healthbar
                _spriteBatch.Draw(
                    blankTexture,
                    player.healthBar.getBar(),
                    player.healthBar.color
                );

                foreach (Enemy enemy in level.enemies)
                {
                    //Draw enemy
                    if (enemy is FighterEnemy)
                    {
                        _spriteBatch.Draw(
                            FighterEnemy.TileSet,
                            enemy.getPosition(),
                            Color.White
                        );
                    }
                    else if (enemy is MageEnemy)
                    {
                        _spriteBatch.Draw(
                           MageEnemy.tileSet,
                           enemy.getPosition(),
                           Color.White
                       );
                    }

                    //Draw enemy healthbar
                    _spriteBatch.Draw(
                        blankTexture,
                        enemy.healthBar.getBar(),
                        enemy.healthBar.color
                    );

                    //Draw projectiles
                    foreach (Attack attack in enemy.attacks)
                    {
                        if (attack is Projectile)
                        {
                            Projectile projectileAttack = (Projectile)attack;
                            if (projectileAttack.face == Face.LEFT)
                            {
                                _spriteBatch.Draw(
                                    Projectile.tileSet,
                                    new Vector2(attack.getHitbox().X, attack.getHitbox().Y),
                                    new Rectangle(0, 0, 20, 15),
                                    Color.White
                                );
                            }
                            else if (projectileAttack.face == Face.RIGHT)
                            {
                                _spriteBatch.Draw(
                                    Projectile.tileSet,
                                    new Vector2(attack.getHitbox().X, attack.getHitbox().Y),
                                    new Rectangle(20, 0, 20, 15),
                                    Color.White
                                );
                            }
                        }
                    }
                }

                //Draw every tile of every levelcomponent
                foreach (List<LevelComponent> rowList in level.list)
                {
                    foreach (LevelComponent levelComponent in rowList)
                    {
                        for (int i = 0; i < levelComponent.tileMap.tiles.Count; i++)
                        {
                            for (int j = 0; j < levelComponent.tileMap.tiles[i].Count; j++)
                            {
                                if (levelComponent.tileMap.tiles[i][j] != 0)
                                {
                                    _spriteBatch.Draw(
                                        LevelComponent.tileSet,
                                        new Vector2(levelComponent.position.X + (j * 24), levelComponent.position.Y + (i * 24)),
                                        levelComponent.getTileTextureOffset(levelComponent.tileMap.tiles[i][j], level.tileSize),
                                        Color.DarkSlateGray
                                    );
                                }
                            }
                        }
                    }
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawPlayer()
        {
            if (player.shieldActive)
            {
                _spriteBatch.Draw(
                    blankTexture,
                    player.getHitbox(),
                    Color.GreenYellow
                );
            }
            else
            {
                _spriteBatch.Draw(
                    blankTexture,
                    player.getHitbox(),
                    Color.Green
                );
            }

            //Draw player healthbar
            _spriteBatch.Draw(
                blankTexture,
                player.healthBar.getBar(),
                player.healthBar.color
            );

            //Draw player attack hitboxes
            foreach (Attack attack in player.attacks)
            {
                _spriteBatch.Draw(
                    blankTexture,
                    attack.getHitbox(),
                    Color.Red
                );
            }

            foreach (Enemy enemy in level.enemies)
            {
                //Draw enemy hitboxes
                _spriteBatch.Draw(
                    blankTexture,
                    enemy.getHitbox(),
                    Color.Yellow
                );

                //Draw enemy healthbar
                _spriteBatch.Draw(
                    blankTexture,
                    enemy.healthBar.getBar(),
                    enemy.healthBar.color
                );

                //Draw enemy attack hitboxes
                foreach (Attack attack in enemy.attacks)
                {
                    _spriteBatch.Draw(
                        blankTexture,
                        attack.getHitbox(),
                        Color.Orange
                    );
                }
            }
        }

        private void drawSpriteElements()
        {
            foreach (UIElement element in ui.elements)
            {
                if(element is UIElementLabelValue)
				{
                    UIElementLabelValue labelValue = (UIElementLabelValue)element;
                    _spriteBatch.DrawString(
                        Arial16,
                        labelValue.label + ": " + labelValue.value,
                        element.position,
                        Color.White,
                        0f,
                        new Vector2(0, 0),
                        1f,
                        SpriteEffects.None,
                        .1f
                    );
                }
                else if(element is UIElementLabel)
				{
                    _spriteBatch.DrawString(
                        Arial16,
                        element.label,
                        element.position,
                        Color.White,
                        0f,
                        new Vector2(0, 0),
                        1f,
                        SpriteEffects.None,
                        .1f
                    );
                }
            }
        }
    }
}
