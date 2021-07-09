using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using C2Eindopdracht.Classes.Units;

namespace C2Eindopdracht.Classes
{
    class Attack
    {
        public Rectangle hitbox { get; set; }
        public int damage { get; set; }
        public Cooldown cooldown { get; set; }
        public float activeTime { get; set; }
        public bool playerHit { get; set; }
        public List<Enemy> enemiesHit { get; set; }
        public bool expired { get; set; }
        public float knockbackTime { get; set; }

        /// <summary>
        /// Attack class, sets default values for attack
        /// </summary>
        /// <param name="damage">Value is the amount of damage an attack deals</param> 
        /// <param name="cooldown">Timespan until next attack can be done</param> 
        /// <param name="activeTime">Time attack is active</param>
        /// <param name="hitbox">Hitbox of attack. If hitbox of attack hits unit, damage will be dealt</param>
        /// <param name="knockbackTime">Time the knockback should last</param>
        public Attack(int damage, Cooldown cooldown, float activeTime, Rectangle hitbox, float knockbackTime)
        {
            this.damage = damage;
            this.cooldown = cooldown;
            this.activeTime = activeTime;
            this.hitbox = hitbox;
            this.enemiesHit = new List<Enemy>();
            this.playerHit = false;
            this.expired = false;
            this.knockbackTime = knockbackTime;
        }

        /// <summary>
        /// Hit the enemy adding the enemy to list of enemies hit
        /// </summary>
        /// <param name="enemy">Enemy that got hit</param>
        public void hitEnemy(Enemy enemy)
		{
            enemiesHit.Add(enemy);
		}

        /// <summary>
        /// Hit the player marking the attack as hit
        /// </summary>
        public void hitPlayer()
		{
            playerHit = true;
		}

        /// <summary>
        /// Update the attack, elapsing the duration
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        /// <returns>The updated attack</returns>
        public virtual void update(GameTime gameTime)
		{
            if (cooldown.elapsedTime >= activeTime)
            {
                expired = true;
            }
            else
            {
                cooldown.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
		}

        /// <summary>
        /// Draw the attack
        /// </summary>
        /// <param name="spriteBatch">Helper class for drawing text strings and sprites in one or more optimized batches</param>
        /// <param name="renderHitboxes">Renders just the hitbox Rectangles if true</param>
        public virtual void draw(SpriteBatch spriteBatch, bool renderHitboxes)
		{
            if(renderHitboxes)
			{
                spriteBatch.Draw(
                    renderHitboxes ? Game1.blankTexture : ITileSet.tileSet,
                    hitbox,
                    Color.Red
                );
            }
        }
    }
}