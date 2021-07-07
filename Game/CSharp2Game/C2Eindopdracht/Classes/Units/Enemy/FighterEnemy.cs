using C2Eindopdracht.Classes.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class FighterEnemy : Enemy, ITileSet
    {
        public static Texture2D tileSet { get; set; }

        /// <summary>
        /// Constructor of Fighter enemy
        /// </summary>
        /// <param name="xPos">Horizontal position</param>
        /// <param name="yPos">Vertical position</param> 
        /// <param name="width">Width of fighter enemy</param> 
        /// <param name="height">Height of fighter enemy</param> 
        public FighterEnemy(int xPos, int yPos, int width, int height, int hp) : base(xPos, yPos, width, height, hp)
        {
            xSpeed = 100f;
            gravity = .3f;
            aggression = Aggression.AGGRESSIVE;
            attackRange = 20;
        }

        public override void decideMovement(GameTime gameTime, Player player)
        {
            Rectangle playerHitbox = player.hitbox;
            if (playerHitbox.Y < hitbox.Y)
            {
                jump(6f, 3f);
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
                attacks.Add(attack(1, new Cooldown(.5f), .4f, new Rectangle((int)position.X, (int)position.Y, 24, 24), 5));
            }
        }
    }
}
