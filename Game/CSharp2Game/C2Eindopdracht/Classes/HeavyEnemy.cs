using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class HeavyEnemy : Enemy
    {
        public static Texture2D tileSet { get; set; }
        public HeavyEnemy(int xPos, int yPos, float gravity, float xSpeed) : base(xPos, yPos, gravity, xSpeed)
        {

        }
    }
}
