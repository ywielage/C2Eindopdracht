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
        /// <param name="gameTime">Timespan since start of game</param>
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

        private void checkHitboxCollisions(Player player, UI ui)
		{
            foreach (Attack attack in attacks)
            {
                player.struck(ui, attack);
            }
        }

        public void struck(UIElementLabelValue enemyCounter, Attack attack)
        {
            if (attack.getHitbox().Intersects(hitBox) && !attack.enemiesHit.Contains(this))
            {
                currHp -= 1;
                healthBar.updateHealthBar(maxHp, currHp);
                knockback = new Cooldown(.5f);
                if (position.X < position.X)
                {
                    xSpeed = 0 - xSpeed;
                }
                ySpeed = -5f;
                setPosition(new Vector2(position.X, position.Y - 5f));
                attack.hitEnemy(this);
                if (currHp <= 0)
                {
                    isAlive = false;
                    enemyCounter.value--;
                }
            }
        }        

        public abstract void decideMovement(GameTime gameTime, Player player);

        protected override void jump(float jumpSpeed, float jumpStartHeight)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                if (grounded)
                {
                    ySpeed = 0 - jumpSpeed;
                    position.Y -= jumpStartHeight;
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
