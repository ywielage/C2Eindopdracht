﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace C2Eindopdracht.Classes.Units
{
    abstract class Unit : ITileSet
    {
        public Vector2 position { get; set; }
        public Rectangle hitbox { get; set; }
        public int maxHp { get; set; }
        public int currHp { get; set; }
        public float xSpeed { get; set; }
        public float ySpeed { get; set; }
        public float gravity { get; set; }
        public bool grounded { get; set; }
        public float jumpSpeed { get; set; }
        public float jumpStartHeight { get; set; }
        public Face face { get; set; }
        public Cooldown knockback { get; set; }
        public List<Attack> attacks { get; set; }
        public bool canAttack { get; set; }
        public Cooldown attackCooldown { get; set; }
        public HealthBar healthBar { get; set; }
        public bool isAlive { get; set; }

        /// <summary>
        /// Every moving unit with hp and a healthbar
        /// </summary>
        /// <param name="xPos">Horizontal position</param>
        /// <param name="yPos">Vertical position</param>
        /// <param name="width">Width of the unit</param>
        /// <param name="height">Height of the unit</param>
		protected Unit(int xPos, int yPos, int width, int height)
		{
			position = new Vector2(xPos, yPos);
			hitbox = new Rectangle(xPos, yPos, width, height);
			attacks = new List<Attack>();
			gravity = gravity;
			ySpeed = 0;
			face = Face.RIGHT;
			grounded = false;
			canAttack = true;
			knockback = null;
			attackCooldown = new Cooldown(0);
			isAlive = true;
		}

		/// <summary>
		/// Adjusts hitbox to current position
		/// </summary>
		protected void alignHitboxToPosition()
		{
			Rectangle hitbox = this.hitbox;
			hitbox.Location = position.ToPoint();
			this.hitbox = hitbox;
		}

		/// <summary>
		/// Adjust healthbar to current position
		/// </summary>
		protected void alignHealthBarToPosition()
		{
			int healthBarHeight = healthBar.bar.Height;
			int healthBarWidth = healthBar.bar.Width;
            HealthBar tempBar = healthBar;
            tempBar.bar = new Rectangle(new Point((int)position.X + healthBar.xOffset, (int)position.Y + healthBar.yOffset), new Point(healthBarWidth, healthBarHeight));
            healthBar = tempBar;
		}

        /// <summary>
        /// Check if unit collides with any walls
        /// </summary>
        /// <param name="walls">All walls it can collide with</param>
        protected bool checkWallCollisions(List<List<LevelComponent>> walls, bool doubleJumpAvailable )
        {
            int touchingGrounds = 0;

            foreach (List<LevelComponent> rowList in walls)
            {
                foreach (LevelComponent levelComponent in rowList)
                {
                    foreach (Rectangle wall in levelComponent.colliders)
                    {
                        touchingGrounds = countTouchingGrounds(touchingGrounds, wall);
                    }
                }
            }
            setYSpeed(touchingGrounds);

            //TODO: Added to let player class know if doublejump is available 
            return doubleJumpAvailable;
        }

        /// <summary>
        /// Increase Y speed if not grounded, set to 0 if grounded
        /// </summary>
        /// <param name="touchingGrounds">Amount of surfaces the unit is standing on</param>
        protected abstract void setYSpeed(int touchingGrounds);

        /// <summary>
        /// Check with how many walls the unit collides
        /// </summary>
        /// <param name="touchingGrounds">Amount of colliding walls</param>
        /// <param name="wall">The current wall</param>
        /// <returns>How many walls it collides with</returns>
        protected int countTouchingGrounds(int touchingGrounds, Rectangle wall)
        {
            Vector2 tempPosition = position;
            if (wall.Left < hitbox.Right && wall.Right > hitbox.Left)
            {
                if (wall.Top - hitbox.Bottom == 0)
                {
                    touchingGrounds++;
                }
                else if (hitbox.Top - wall.Bottom < 1 && hitbox.Top - wall.Bottom > -10)
                {
                    ySpeed = 0;
                }
                else if (wall.Top - hitbox.Bottom < 1 && wall.Top - hitbox.Bottom > -11)
                {
                    touchingGrounds++;
                    tempPosition.Y = wall.Top - hitbox.Height;
                }
            }
            if (wall.Top < hitbox.Bottom && wall.Bottom > hitbox.Top)
            {
                if (hitbox.Left - wall.Right < 1 && hitbox.Left - wall.Right > -5)
                {
                    tempPosition.X = wall.Right + 2;
                }
                if (wall.Left - hitbox.Right < 1 && wall.Left - hitbox.Right > -5)
                {
                    tempPosition.X = wall.Left - hitbox.Width - 2;
                }
            }
            position = tempPosition;
            return touchingGrounds;
        }

        /// <summary>
        /// Updates the attacks.
        /// Removes attacks that have expired.
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param> 
        protected void updateAttacks(GameTime gameTime)
        {
            foreach (Attack attack in attacks)
            {
                if (attack.cooldown.elapsedTime <= attack.cooldown.duration && canAttack)
                {
                    canAttack = false;
                    attackCooldown = new Cooldown(attack.cooldown.duration);
                }
                attack.update(gameTime);
            }

            List<Attack> expiredAttacks = new List<Attack>();
            foreach (Attack attack in attacks)
            {
                if (attack.expired)
                {
                    expiredAttacks.Add(attack);
                }
            }
            attacks.RemoveAll(attack => expiredAttacks.Contains(attack));

            if (!canAttack && attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                canAttack = true;
            }
            else
            {
                attackCooldown.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Update the knockback
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        protected void updateKnockBack(GameTime gameTime)
        {
            Vector2 tempPosition = position;
            knockback.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            tempPosition.X -= xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position = tempPosition;

            if (knockback.duration <= knockback.elapsedTime)
            {
                knockback = null;
                if (xSpeed < 0)
                {
                    xSpeed -= xSpeed * 2;
                }
            }
        }

        /// <summary>
        /// Move the unit left
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        protected void moveLeft(GameTime gameTime)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                Vector2 tempPosition = position;
                tempPosition.X -= xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                face = Face.LEFT;
                position = tempPosition;
            }
        }

        /// <summary>
        /// Move the unit right
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        protected void moveRight(GameTime gameTime)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                Vector2 tempPosition = position;
                tempPosition.X += xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                face = Face.RIGHT;
                position = tempPosition;
            }
        }

        /// <summary>
        /// Let the unit jump with a given speed
        /// </summary>
        protected abstract void jump();

        /// <summary>
        /// Attack based on the offset of which direction the unit is facing
        /// </summary>
        /// <param name="damage">Amount of damage the attack does</param>
        /// <param name="cooldown">Cooldown for the next attack</param>
        /// <param name="duration">Duration of the attack</param>
        /// <param name="hitbox">Hitbox of the attack which units can colide with</param>
        /// <param name="hitboxXOffSet">Offset of the hitbox, if the unit is facing left the attack comes out slightly more to that side</param>
        /// <returns>A new offset based on the face</returns>
        protected virtual Attack attack(int damage, Cooldown cooldown, float duration, Rectangle hitbox, int hitboxXOffSet)
        {
            
            if (face == Face.LEFT)
            {
                hitbox.X -= hitboxXOffSet;
            }
            else
            {
                hitbox.X += hitboxXOffSet;
            }
            return new Attack(damage, cooldown, duration, hitbox);
        }

        /// <summary>
        /// Prints values of unit
        /// </summary>
        public void printValues()
        {
            Debug.WriteLine("=============================");
            Debug.WriteLine("Unit Pos:\tX " + position.X + ",\tY " + position.Y);
            Debug.WriteLine("Hitbox Pos:\t\tX " + hitbox.X + ",\tY " + hitbox.Y);
            Debug.WriteLine("Speed:\t\t\tX " + xSpeed + ",\tY" + ySpeed);
            Debug.WriteLine("Grounded: " + grounded);
        }

        /// <summary>
        /// Draw the unit
        /// </summary>
        /// <param name="spriteBatch">Helper class for drawing text strings and sprites in one or more optimized batches</param>
        /// <param name="renderHitboxes">Renders just the hitbox Rectangles if true</param>
        public void draw(SpriteBatch spriteBatch, bool renderHitboxes)
		{
            drawHitbox(spriteBatch, renderHitboxes);
            healthBar.draw(spriteBatch);
            foreach (Attack attack in attacks)
            {
                attack.draw(spriteBatch, renderHitboxes);
            }
        }

        /// <summary>
        /// Draw the hitbox of the unit
        /// </summary>
        /// <param name="spriteBatch">Helper class for drawing text strings and sprites in one or more optimized batches</param>
        /// <param name="renderHitboxes">Renders just the hitbox Rectangles if true</param>
        protected abstract void drawHitbox(SpriteBatch spriteBatch, bool renderHitboxes);
    }
}