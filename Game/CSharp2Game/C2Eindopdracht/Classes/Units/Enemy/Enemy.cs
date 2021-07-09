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
        /// <param name="hp">Amount of hp the enemy has</param>
        protected Enemy(int xPos, int yPos, int width, int height, int hp) : base(xPos, yPos, width, height, hp)
        {
            this.healthBar = new HealthBar(new Rectangle(xPos, yPos, 20, 10), 20, Color.Purple, 0, -15);
        }

        /// <summary>
        /// Update enemy
        /// </summary>
        /// <param name="gameTime">Holds the timestate of a Game</param>
        /// <param name="levelComponents">The levelcomponents it can collide with</param>
        /// <param name="player">Player object</param> 
        /// <param name="ui">UI object</param> 
        public void update(GameTime gameTime, List<List<LevelComponent>> levelComponents, Player player, UI ui)
        {
            setYSpeed(levelComponents);
            checkHitboxCollisions(player, ui, levelComponents);
            if (knockback != null)
            {
                updateKnockBack(gameTime, levelComponents);
            }
            else
            {
                decideMovement(gameTime, player, levelComponents);
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
        private void checkHitboxCollisions(Player player, UI ui, List<List<LevelComponent>> levelComponents)
		{
            foreach (Attack attack in attacks)
            {
                player.setUIonHit(ui, attack, levelComponents);
            }
        }

        /// <summary>
        /// Strike the enemy with an attack, dealing damage
        /// </summary>
        /// <param name="enemyCounter">User interface element which holds the count of enemies</param>
        /// <param name="attack">Attack to see if it collides with the enemy</param>
        public void setUIonHit(UIElementLabelValue enemyCounter, Attack attack, List<List<LevelComponent>> levelComponents)
        {
            if (attack.hitbox.Intersects(hitbox) && !attack.enemiesHit.Contains(this))
            {
                struck(attack, levelComponents);
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
        /// <param name="levelComponents">The levelcomponents it can collide with</param>
        private void decideMovement(GameTime gameTime, Player player, List<List<LevelComponent>> levelComponents)
        {
            Rectangle playerHitbox = player.hitbox;
            if (playerHitbox.Y < hitbox.Y)
            {
                jump(levelComponents);
            }

            if (playerHitbox.X < hitbox.X)
            {
                moveLeft(gameTime, levelComponents);
            }

            if (playerHitbox.X > hitbox.X)
            {
                moveRight(gameTime, levelComponents);
            }

            if (playerHitbox.X - hitbox.X < attackRange && playerHitbox.X - hitbox.X > -attackRange &&
                playerHitbox.Y - hitbox.Y < attackRange && playerHitbox.Y - hitbox.Y > -attackRange &&
                canAttack)
            {
                attacks.Add(getAttack());
            }
        }

        /// <summary>
        /// Get the attack values for the specific enemy
        /// </summary>
        /// <returns></returns>
        protected abstract Attack getAttack();

        protected override void jump(List<List<LevelComponent>> levelComponents)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                Vector2 tempPosition = position;
                if (grounded)
                {
                    ySpeed = 0 - jumpSpeed;
                    tempPosition.Y -= jumpStartHeight;
                }
                if (canMove(tempPosition, levelComponents))
                {
                    position = tempPosition;
                }
            }
        }

        protected abstract override void drawHitbox(SpriteBatch spriteBatch, bool renderHitboxes);
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
