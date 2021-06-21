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
            player = new Player(20, 170, 20, .3f, 200f);
            renderHitboxes = false;
            
            level = new Level(5, 5, 20);
            level.init(false);
            level.drawLevelInDebug();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            LevelComponent.tileSet = Content.Load<Texture2D>("tileset-map-squared");
            Player.tileSet = Content.Load<Texture2D>("character1");
            MageEnemy.tileSet = Content.Load<Texture2D>("enemy1wit");
            FighterEnemy.tileSet = Content.Load<Texture2D>("enemy1wit");
            Projectile.tileSet = Content.Load<Texture2D>("fireball");

            blankTexture = Content.Load<Texture2D>("blankTexture");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(SmartKeyboard.HasBeenPressed(Keys.Tab))
                renderHitboxes = !renderHitboxes;

            // TODO: Add your update logic here
            player.update(gameTime, level.list, level.enemies);

            foreach(Enemy enemy in level.enemies)
			{
                enemy.update(gameTime, level.list, player);
			}

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            camera.Pos = player.getPosition();
            camera.Zoom = 1.5f;

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.get_transformation(GraphicsDevice));

            if (renderHitboxes)
			{
                //Draw player hitbox
                if(player.shieldActive)
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
                            FighterEnemy.tileSet,
                            enemy.getPosition(),
                            Color.White
                        );
                    }
                    else if(enemy is MageEnemy)
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
                        if(attack is Projectile)
                        {
                            Projectile projectileAttack = (Projectile)attack;
                            if(projectileAttack.face == Face.LEFT)
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
    }
}
