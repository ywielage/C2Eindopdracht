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
        public override int attackRange { get; set; }
        public static Texture2D tileSet { get; set; }
        public MageEnemy(int xPos, int yPos, int width, int height) : base(xPos, yPos, width, height)
        {
            maxHp = 3;
            currHp = 3;
            xSpeed = 80f;
            gravity = .3f;
            attackRange = 100;
        }

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
