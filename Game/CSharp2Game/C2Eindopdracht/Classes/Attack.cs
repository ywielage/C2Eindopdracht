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
        public int damage { get; set; }
        public Cooldown cooldown { get; set; }
        public float activeTime { get; set; }
        public Rectangle hitbox { get; set; }

        public Attack(int damage, Cooldown cooldown, float duration, Rectangle hitbox)
        {
            this.damage = damage;
            this.cooldown = cooldown;
            this.activeTime = duration;
            this.hitbox = hitbox;
        }
    }
}
