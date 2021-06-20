using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class FighterEnemy : Enemy
    {

        public static Texture2D tileSet { get; set; }
        private System.Drawing.Rectangle hitBox;
        public FighterEnemy(int xPos, int yPos, float gravity, float xSpeed) : base(xPos, yPos, gravity, xSpeed)
        {
            hitBox = new System.Drawing.Rectangle(xPos, yPos, 18, 30);
        }

        public void updateAttacks(GameTime gametime)
        {
            base.updateAttacks(gametime);
        }
    }
}
