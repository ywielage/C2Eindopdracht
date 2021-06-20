using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class MageEnemy : Enemy
    {
        public static Texture2D tileSet { get; set; }
        public MageEnemy(int xPos, int yPos, float gravity, float xSpeed) : base(xPos, yPos, gravity, xSpeed)
        {

        }
    }
}
