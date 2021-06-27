using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class MageEnemy : Enemy
    {
        public override int maxHp { get; set; }
        public override int currHp { get; set; }
        public override float xSpeed { get; set; }
        public override Aggression aggression { get; set; }
        public override int attackRange { get; set; }
        public static Texture2D tileSet { get; set; }

        /// <summary>
        /// Constructor of mage enemies
        /// </summary>
        /// <param name="xPos">Horizontal position</param> 
        /// <param name="yPos">Vertical position</param> 
        /// <param name="width">Width of mage enemy</param> 
        /// <param name="height">Height of mage enemy</param> 
        /// <param name="aggression">Aggression level of enemy</param> 
        public MageEnemy(int xPos, int yPos, int  width, int height) : base(xPos, yPos, width, height)
        {
            maxHp = 3;
            currHp = 3;
            xSpeed = 80f;
            gravity = .3f;
            aggression = Aggression.AGGRESSIVE;
            attackRange = 100;
        }

        /// <summary>
        /// Algorithm which controls movement of enemy
        /// </summary>
        /// <param name="gameTime">Duration of game</param> 
        /// <param name="player">Player object</param> 
        public override void decideMovement(GameTime gameTime, Player player)
        {
            Rectangle playerHitbox = player.getHitbox();
            if (playerHitbox.Y < hitBox.Y)
            {
                jump(4.5f, 3f);
            }
            if (playerHitbox.X < hitBox.X)
            {
                moveLeft(gameTime);
            }
            if (playerHitbox.X > hitBox.X)
            {
                moveRight(gameTime);
            }
            if (playerHitbox.X - hitBox.X < attackRange && playerHitbox.X - hitBox.X > -attackRange && 
                playerHitbox.Y - hitBox.Y < attackRange && playerHitbox.Y - hitBox.Y > -attackRange && 
                canAttack)
            {
                attacks.Add(attack(1, new Cooldown(.8f), .8f, new Rectangle((int)position.X, (int)position.Y, 20, 15), 5));
            }
        }

        /// <summary>
        /// Decides when mage enemy attacks
        /// </summary>
        /// <param name="damage">Damage of attack</param> 
        /// <param name="cooldown">Timespan until next attack</param> 
        /// <param name="duration">Time attack takes</param> 
        /// <param name="hitbox">Hitbox of attack</param> 
        /// <param name="hitboxXOffSet">Hitbox offset of attack</param> 
        /// <returns></returns>
        public override Attack attack(int damage, Cooldown cooldown, float duration, Rectangle hitbox, int hitboxXOffSet)
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
