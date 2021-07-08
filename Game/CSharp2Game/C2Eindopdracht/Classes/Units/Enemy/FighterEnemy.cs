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
            jumpSpeed = 6f;
            jumpStartHeight = 3f;
            aggression = Aggression.AGGRESSIVE;
            attackRange = 20;
        }

        protected override Attack getAttack()
        {
            return attack(1, new Cooldown(.5f), .4f, new Rectangle((int)position.X, (int)position.Y, 24, 24), 5);
        }

        protected override void drawHitbox(SpriteBatch spriteBatch, bool renderHitboxes)
        {
            spriteBatch.Draw(
                renderHitboxes ? Game1.blankTexture : tileSet,
                hitbox,
                renderHitboxes ? Color.Orange : Color.White
            );
        }
    }
}
