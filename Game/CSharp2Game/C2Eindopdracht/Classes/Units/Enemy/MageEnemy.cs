using C2Eindopdracht.Classes.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class MageEnemy : Enemy, ITileSet
    {
        public static Texture2D tileSet { get; set; }

        /// <summary>
        /// Constructor of mage enemies
        /// </summary>
        /// <param name="xPos">Horizontal position</param> 
        /// <param name="yPos">Vertical position</param> 
        /// <param name="width">Width of mage enemy</param> 
        /// <param name="height">Height of mage enemy</param>
        public MageEnemy(int xPos, int yPos, int  width, int height, int hp) : base(xPos, yPos, width, height, hp)
        {
            xSpeed = 80f;
            gravity = .3f;
            aggression = Aggression.AGGRESSIVE;
            attackRange = 100;
        }

        public override void decideMovement(GameTime gameTime, Player player)
        {
            Rectangle playerHitbox = player.hitbox;
            if (playerHitbox.Y < hitbox.Y)
            {
                jump(4.5f, 3f);
            }
            if (playerHitbox.X < hitbox.X)
            {
                moveLeft(gameTime);
            }
            if (playerHitbox.X > hitbox.X)
            {
                moveRight(gameTime);
            }
            if (playerHitbox.X - hitbox.X < attackRange && playerHitbox.X - hitbox.X > -attackRange && 
                playerHitbox.Y - hitbox.Y < attackRange && playerHitbox.Y - hitbox.Y > -attackRange && 
                canAttack)
            {
                attacks.Add(attack(1, new Cooldown(.8f), .8f, new Rectangle((int)position.X, (int)position.Y, 20, 15), 5));
            }
        }

        protected override Attack attack(int damage, Cooldown cooldown, float duration, Rectangle hitbox, int hitboxXOffSet)
		{
            if (face == Face.LEFT)
            {
                hitbox.X -= hitboxXOffSet;
            }
            else
            {
                hitbox.X += hitboxXOffSet;
            }
            return new Projectile(damage, cooldown, duration, hitbox, 200f, face);
        } 
    }
}
