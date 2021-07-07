using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using C2Eindopdracht.Classes.Units;

namespace C2Eindopdracht.Classes
{
    class Player : Unit, ITileSet
    {
        public bool doubleJumpAvailable { get; set; }
        public bool canDoubleJump { get; set; }
        public bool shieldActive { get; set; }
        private int shieldTime { get; set; }
        private int maxShieldTime { get; set; }
        private int shieldRefill { get; set; }
        private bool shieldUsed { get; set; }
        public static Texture2D tileSet { get; set; }

        /// <summary>
        /// Sets default values
        /// </summary>
        /// <param name="xPos">Horizontal position</param> 
        /// <param name="yPos">Vertical position</param> 
        /// <param name="hp">Amount of lives</param>
        /// <param name="gravity">Takes the player back to ground</param> 
        /// <param name="xSpeed">Horizontal speed</param> 
        public Player(int xPos, int yPos, int width, int height, int hp) : base(xPos, yPos, width, height)
        {
            this.maxHp = hp;
            this.currHp = hp;
            this.xSpeed = 200f;
            this.gravity = .3f;
            this.doubleJumpAvailable = false;
            this.canDoubleJump = true;
            this.shieldActive = false;
            this.healthBar = new HealthBar(new Rectangle(xPos, yPos, 50, 10), 50, Color.Gold, -17, -15);
            this.maxShieldTime = 120;
            this.shieldRefill = 1;
            this.shieldUsed = false;
        }

        /// <summary>
        /// Update player
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        /// <param name="levelComponents">List of levelcomponents</param>
        /// <param name="enemies">List of enemies</param>
        /// <param name="enemyCounter">Count of alive enemies</param>
        public void update(GameTime gameTime, List<List<LevelComponent>> levelComponents, List<Enemy> enemies, UIElementLabelValue enemyCounter)
        {
            doubleJumpAvailable = checkWallCollisions(levelComponents);
            checkHitboxCollisions(enemies, enemyCounter);
            if (knockback != null)
            {
                updateKnockBack(gameTime);
            }
            else if (knockback == null && isAlive)
            {
                checkKeyPresses(gameTime);
            }
            updateAttacks(gameTime);
            alignHitboxToPosition();
            alignHealthBarToPosition();
            //printValues();
        }        

        /// <summary>
        /// Check if any of the enemy attack hitboxes collide with the player.
        /// Remove enemies that have died.
        /// </summary>
        /// <param name="enemies"></param>
        /// <param name="enemyCounter"></param>
        private void checkHitboxCollisions(List<Enemy> enemies, UIElementLabelValue enemyCounter)
		{
            foreach (Enemy enemy in enemies)
            {
                foreach (Attack attack in attacks)
                {
                    enemy.struck(enemyCounter, attack);
                }
            }

            List<Enemy> deadEnemies = new List<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                if (!enemy.isAlive)
                {
                    deadEnemies.Add(enemy);
                }
            }
            enemies.RemoveAll(enemy => deadEnemies.Contains(enemy));
        }

        /// <summary>
        /// Strike the player with an attack, dealing damage
        /// </summary>
        /// <param name="ui">User interface to change</param>
        /// <param name="attack">Attack to see if it collides with the player</param>
        public void struck(UI ui, Attack attack)
        {
            if (attack.hitbox.Intersects(hitbox) && attack.playerHit == false && !shieldActive)
            {
                currHp -= 1;
                healthBar.updateHealthBar(maxHp, currHp);
                knockback = new Cooldown(.2f);
                if (position.X < position.X)
                {
                    xSpeed = 0 - xSpeed;
                }
                ySpeed = -5f;
                position = new Vector2(position.X, position.Y - 5f);
                attack.hitPlayer();
                if (currHp <= 0)
                {
                    gameOver(ui);
                }
            }
        }

        /// <summary>
        /// Check if a key is pressed
        /// A move left, D move right
        /// W or Space to jump
        /// J to attack or K to shield
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        private void checkKeyPresses(GameTime gameTime)
        {
            var keyboardState = SmartKeyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.A))
            {
                moveLeft(gameTime);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                moveRight(gameTime);
            }

            if (SmartKeyboard.HasBeenPressed(Keys.Space) || SmartKeyboard.HasBeenPressed(Keys.W))
            {
                jump(6f, 3f);
            }

            if (SmartKeyboard.HasBeenPressed(Keys.J) && canAttack && !shieldActive)
            {
                attacks.Add(attack(1, new Cooldown(.5f), .2f, new Rectangle((int)position.X, (int)position.Y, 24, 24), 5));
            }

            if (keyboardState.IsKeyDown(Keys.K) && canAttack && shieldTime <= maxShieldTime && shieldUsed == false)
            {
                shieldActive = true;
                shieldTime++;
                if(shieldTime == maxShieldTime) {
                    shieldUsed = true;
                }
            }
            else if(shieldUsed == true)
            {
                shieldActive = false;
                shieldTime = shieldTime - shieldRefill;
                if (shieldTime == 0)
                {
                    shieldUsed = false;
                }
            }
            else if(keyboardState.IsKeyUp(Keys.K))
            {
                shieldActive = false;
            }
        }

        protected override void jump(float jumpSpeed, float jumpStartHeight)
        {
            Vector2 tempPosition = position;
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                if (grounded)
                {
                    ySpeed = 0 - jumpSpeed;
                    tempPosition.Y -= jumpStartHeight;
                    doubleJumpAvailable = true;
                }
                else
                {
                    if (doubleJumpAvailable)
                    {
                        ySpeed = 0 - jumpSpeed;
                        tempPosition.Y -= jumpStartHeight;
                        doubleJumpAvailable = false;
                    }
                }
            }
            position = tempPosition;
        }

        /// <summary>
        /// Game over
        /// </summary>
        /// <param name="ui">UI to add element to</param>
        public void gameOver(UI ui)
		{
            isAlive = false;
            ui.addUIElement("You've died!", new Vector2(5, 50), 0);
        }
    }

    enum Face
    {
        LEFT,
        RIGHT
    }
}
