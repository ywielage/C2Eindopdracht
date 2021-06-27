using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class FighterEnemy : Enemy
    {
        public override int maxHp { get; set; }
        public override int currHp { get; set; }
		public override float xSpeed { get; set; }
        public override Aggression aggression { get; set; }
		public override int attackRange { get; set; }
        public static Texture2D tileSet { get; set; }
        /// <summary>
        /// Constructor voor FighterEnemy
        /// </summary>
        /// <param name="xPos"></param> Horizontale positie
        /// <param name="yPos"></param> Verticale positie
        /// <param name="width"></param> Wijdte 
        /// <param name="height"></param> Hoogte
        public FighterEnemy(int xPos, int yPos, int width, int height) : base(xPos, yPos, width, height)
        {
            maxHp = 5;
            currHp = 5;
            xSpeed = 100f;
            gravity = .3f;
            aggression = Aggression.AGGRESSIVE;
            attackRange = 20;
        }

        /// <summary>
        /// Algoritme voor gedrag enemy
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        public override void decideMovement(GameTime gameTime, Player player)
        {
            Rectangle playerHitbox = player.getHitbox();
            if (playerHitbox.Y < hitBox.Y)
            {
                jump(6f, 3f);
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
                attacks.Add(attack(1, new Cooldown(.5f), .4f, new Rectangle((int)position.X, (int)position.Y, 24, 24), 5));
            }
        }

        /// <summary>
        /// Logica voor aanvallen enemy
        /// </summary>
        /// <param name="damage"></param> Schade van aanval
        /// <param name="cooldown"></param> Tijdsduur tot volgende aanval
        /// <param name="duration"></param> Looptijd van aanval
        /// <param name="hitbox"></param> Hitbox van aanval
        /// <param name="hitboxXOffSet"></param> Hitbox offset van aanval
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
            return new Attack(damage, cooldown, duration, hitbox);
        }
    }
}
