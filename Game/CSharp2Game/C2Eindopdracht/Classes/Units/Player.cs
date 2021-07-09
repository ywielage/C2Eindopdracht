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
        /// Player character, the one you control
        /// </summary>
        /// <param name="xPos">Horizontal Position</param> 
        /// <param name="yPos">Vertical position</param> 
        /// <param name="width">Width player</param>  
        /// <param name="height">Height player</param>
        /// <param name="hp">Amount of hp the player has</param>
        public Player(int xPos, int yPos, int width, int height, int hp) : base(xPos, yPos, width, height, hp)
        {
            xSpeed = 200f;
            gravity = .3f;
            jumpSpeed = 6f;
            jumpStartHeight = 3f;
            doubleJumpAvailable = false;
            canDoubleJump = true;
            healthBar = new HealthBar(new Rectangle(xPos, yPos, 50, 10), 50, Color.Gold, -17, -21);
            shield = new Shield(new HealthBar(new Rectangle(xPos, yPos, 50, 6), 50, Color.LightBlue, -17, -12), false, false, 120, 1);
        }

        /// <summary>
        /// Update player
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        /// <param name="levelComponents">The levelcomponents it can collide with</param>
        /// <param name="enemies">List of enemies</param>
        /// <param name="enemyCounter">Count of alive enemies</param>
        public void update(GameTime gameTime, List<List<LevelComponent>> levelComponents, List<Enemy> enemies, UIElementLabelValue enemyCounter)
        {
            setYSpeed(levelComponents);
            if(grounded)
			{
                doubleJumpAvailable = true;
			}
            checkHitboxCollisions(enemies, enemyCounter, levelComponents);
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
            alignShieldBarToPosition();
            //printValues();
        }

        /// <summary>
        /// Align the shieldbar to the player position
        /// </summary>
        protected void alignShieldBarToPosition()
        {
            int shieldBarWidth = shield.healthBar.bar.Width;
            int shieldBarHeight = shield.healthBar.bar.Height;
            HealthBar tempBar = shield.healthBar;
            tempBar.bar = new Rectangle(new Point((int)position.X + shield.healthBar.xOffset, (int)position.Y + shield.healthBar.yOffset), new Point(shieldBarWidth, shieldBarHeight));
            shield.healthBar = tempBar;
        }

        /// <summary>
        /// Check if any of the enemy attack hitboxes collide with the player.
        /// Remove enemies that have died.
        /// </summary>
        /// <param name="enemies"></param>
        /// <param name="enemyCounter"></param>
        /// /// <param name="levelComponents">The levelcomponents it can collide with</param>
        private void checkHitboxCollisions(List<Enemy> enemies, UIElementLabelValue enemyCounter, List<List<LevelComponent>> levelComponents)
		{
            foreach (Enemy enemy in enemies)
            {
                foreach (Attack attack in attacks)
                {
                    enemy.setUIonHit(enemyCounter, attack, levelComponents);
                }
            }
            enemies.RemoveAll(enemy => !enemy.isAlive);
        }

        /// <summary>
        /// Strike the player with an attack, dealing damage
        /// </summary>
        /// <param name="ui">User interface to change</param>
        /// <param name="attack">Attack to see if it collides with the player</param>
        public void setUIonHit(UI ui, Attack attack, List<List<LevelComponent>> levelComponents)
        {
            if (attack.hitbox.Intersects(hitbox) && attack.playerHit == false && !shield.isActive)
            {
                struck(attack, levelComponents);
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
                attacks.Add(attack(1, new Cooldown(.5f), .2f, new Rectangle((int)position.X, (int)position.Y, 24, 24), .5f, 5));
            }

            if (keyboardState.IsKeyDown(Keys.K) && canAttack && isAlive)
            {
                shield.activate();
            }
            else if (keyboardState.IsKeyUp(Keys.K))
            {
                shield.deactivate();
            }
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
            shield.healthBar.updateHealthBar(0, 0);
            ui.addUIElement("You've died!", new Vector2(5, 50), 0);
        }

        /// <summary>
        /// Draw the unit
        /// </summary>
        /// <param name="spriteBatch">Helper class for drawing text strings and sprites in one or more optimized batches</param>
        /// <param name="renderHitboxes">Renders just the hitbox Rectangles if true</param>
        public override void draw(SpriteBatch spriteBatch, bool renderHitboxes)
        {
            drawHitbox(spriteBatch, renderHitboxes);
            healthBar.draw(spriteBatch);
            shield.healthBar.draw(spriteBatch);
            foreach (Attack attack in attacks)
            {
                attack.draw(spriteBatch, renderHitboxes);
            }
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
