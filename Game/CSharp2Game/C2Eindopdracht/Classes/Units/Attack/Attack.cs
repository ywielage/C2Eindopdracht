using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class Attack
    {
        protected Rectangle hitbox;
        public int damage { get; set; }
        public Cooldown cooldown { get; set; }
        public float activeTime { get; set; }
        public bool playerHit { get; set; }
        public List<Enemy> enemiesHit { get; set; }
        /// <summary>
        /// Attack class, sets default values for attack
        /// </summary>
        /// <param name="damage">Value is the amount of damage an attack deals</param> 
        /// <param name="cooldown">Timespan until next attack can be done</param> 
        /// <param name="activeTime">Time attack is active</param>
        /// <param name="hitbox">Hitbox of attack. If hitbox of attack hits unit, damage will be dealt</param> 
        public Attack(int damage, Cooldown cooldown, float activeTime, Rectangle hitbox)
        {
            this.damage = damage;
            this.cooldown = cooldown;
            this.activeTime = activeTime;
            this.hitbox = hitbox;
            this.enemiesHit = new List<Enemy>();
            this.playerHit = false;
        }

        public Rectangle getHitbox()
        {
            return this.hitbox;
        }

        public void setHitbox(Rectangle hitBox)
        {
            this.hitbox = hitBox;
        }

        public void hitEnemy(Enemy enemy)
		{
            enemiesHit.Add(enemy);
		}

        public void hitPlayer()
		{
            playerHit = true;
		}
    }
}
