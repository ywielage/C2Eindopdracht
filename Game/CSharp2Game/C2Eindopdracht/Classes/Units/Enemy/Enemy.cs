using C2Eindopdracht.Classes.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace C2Eindopdracht.Classes
{
    abstract class Enemy : Unit
    {
        public Aggression aggression { get; set; }
        public int attackRange { get; set; }

        /// <summary>
        /// Class made for enemies. 
        /// </summary>
        /// <param name="xPos">Horizontal Position</param> 
        /// <param name="yPos">Vertical position</param> 
        /// <param name="width">Width enemy</param>  
        /// <param name="height">Height enemy</param>
        protected Enemy(int xPos, int yPos, int width, int height, int hp) : base(xPos, yPos, width, height)
        {
            this.maxHp = hp;
            this.currHp = hp;
            this.healthBar = new HealthBar(new Rectangle(xPos, yPos, 20, 10), 20, Color.Purple, 0, -15);
        }

        /// <summary>
        /// Update enemy
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        /// <param name="levelComponents">List of levelcomponents</param>
        /// <param name="player">Player object</param> 
        /// <param name="ui">UI object</param> 
        public void update(GameTime gameTime, List<List<LevelComponent>> levelComponents, Player player, UI ui)
        {
            checkWallCollisions(levelComponents);
            checkHitboxCollisions(player, ui);
            if (knockback != null)
            {
                updateKnockBack(gameTime);
            }
            else
            {
                decideMovement(gameTime, player);
            }
            updateAttacks(gameTime);
            alignHitboxToPosition();
            alignHealthBarToPosition();
            //printValues();
        }

        /// <summary>
        /// Check if any of the player attack hitboxes collide with the enemy.
        /// </summary>
        /// <param name="enemies"></param>
        /// <param name="enemyCounter"></param>
        private void checkHitboxCollisions(Player player, UI ui)
		{
            foreach (Attack attack in attacks)
            {
                player.struck(ui, attack);
            }
        }

        /// <summary>
        /// Strike the enemy with an attack, dealing damage
        /// </summary>
        /// <param name="enemyCounter">User interface element which holds the count of enemies</param>
        /// <param name="attack">Attack to see if it collides with the enemy</param>
        public void struck(UIElementLabelValue enemyCounter, Attack attack)
        {
            if (attack.hitbox.Intersects(hitbox) && !attack.enemiesHit.Contains(this))
            {
                currHp -= 1;
                healthBar.updateHealthBar(maxHp, currHp);
                knockback = new Cooldown(.5f);
                if (position.X < position.X)
                {
                    xSpeed = 0 - xSpeed;
                }
                ySpeed = -5f;
                position = new Vector2(position.X, position.Y - 5f);
                attack.hitEnemy(this);
                if (currHp <= 0)
                {
                    isAlive = false;
                    enemyCounter.value--;
                }
            }
        }

        /// <summary>
        /// Decide where to go or to attack based on the position of the player
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        /// <param name="player">The player</param>
        public abstract void decideMovement(GameTime gameTime, Player player);

        protected override void jump(float jumpSpeed, float jumpStartHeight)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                if (grounded)
                {
                    Vector2 tempPosition = position;
                    ySpeed = 0 - jumpSpeed;
                    tempPosition.Y -= jumpStartHeight;
                    position = tempPosition;
                }
            }
        }        
    }

    /// <summary>
    /// Enum which shows all possible aggression levels of enemies
    /// </summary>
    enum Aggression
    {
        FRIENDLY,
        NEUTRAL,
        AGGRESSIVE
    }
}
