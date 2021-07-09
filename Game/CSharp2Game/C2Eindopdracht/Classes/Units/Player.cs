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
        public Shield shield { get; set; }
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
            maxHp = hp;
            currHp = hp;
            xSpeed = 200f;
            gravity = .3f;
            jumpSpeed = 6f;
            jumpStartHeight = 3f;
            doubleJumpAvailable = false;
            canDoubleJump = true;
            healthBar = new HealthBar(new Rectangle(xPos, yPos, 50, 10), 50, Color.Gold, -17, -15);
            shield = new Shield(false, false, 120, 1);
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
            setYSpeed(levelComponents);
            if(grounded)
			{
                doubleJumpAvailable = true;
			}
            checkHitboxCollisions(enemies, enemyCounter);
            if (knockback != null)
            {
                updateKnockBack(gameTime, levelComponents);
            }
            else if (knockback == null && isAlive)
            {
                checkKeyPresses(gameTime, levelComponents);
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
            enemies.RemoveAll(enemy => !enemy.isAlive);
        }

        /// <summary>
        /// Strike the player with an attack, dealing damage
        /// </summary>
        /// <param name="ui">User interface to change</param>
        /// <param name="attack">Attack to see if it collides with the player</param>
        public void struck(UI ui, Attack attack)
        {
            if (attack.hitbox.Intersects(hitbox) && attack.playerHit == false && !shield.isActive)
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
        private void checkKeyPresses(GameTime gameTime, List<List<LevelComponent>> levelComponents)
        {
            var keyboardState = SmartKeyboard.GetState();
            if (SmartKeyboard.HasBeenPressed(Keys.Space) || SmartKeyboard.HasBeenPressed(Keys.W))
            {
                jump(levelComponents);
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                moveLeft(gameTime, levelComponents);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                moveRight(gameTime, levelComponents);
            }

            if (SmartKeyboard.HasBeenPressed(Keys.J) && canAttack && !shield.isActive)
            {
                attacks.Add(attack(1, new Cooldown(.5f), .2f, new Rectangle((int)position.X, (int)position.Y, 24, 24), 5));
            }

            if (keyboardState.IsKeyDown(Keys.K) && canAttack)
            {
                shield.activate();
            }
            else if (keyboardState.IsKeyUp(Keys.K))
            {
                shield.deactivate();
            }
            Debug.WriteLine(shield.currFill);
        }

        protected override void jump(List<List<LevelComponent>> levelComponents)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                Vector2 tempPosition = position;
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
                if(canMove(tempPosition, levelComponents))
				{
                    position = tempPosition;
                }
            }
        }

        /// <summary>
        /// Make the player dead, making them unable to do anything
        /// </summary>
        /// <param name="ui">UI to add element to</param>
        public void gameOver(UI ui)
		{
            isAlive = false;
            ui.addUIElement("You've died!", new Vector2(5, 50), 0);
        }

		protected override void drawHitbox(SpriteBatch spriteBatch, bool renderHitboxes)
		{
            spriteBatch.Draw(
                renderHitboxes ? Game1.blankTexture : tileSet,
                hitbox,
                !renderHitboxes ? Color.White : (shield.isActive ? Color.GreenYellow : Color.Green)
            );
        }
	}

    enum Face
    {
        LEFT,
        RIGHT
    }
}
