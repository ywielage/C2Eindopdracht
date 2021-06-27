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
        /// Logica voor aanvallen
        /// </summary>
        /// <param name="damage"></param> Schade aanval
        /// <param name="cooldown"></param> Tijdsduur tot mogelijkheid volgende aanval
        /// <param name="activeTime"></param> // Tijd actief
        /// <param name="hitbox"></param> // Hitbox van aanval
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
