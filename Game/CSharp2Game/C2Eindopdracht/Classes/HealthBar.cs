using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class HealthBar
    {
        private Rectangle bar;
        public int fullWidth { get; set; }
        public Color color { get; set; }
        public int xOffset { get; set; }
        public int yOffset { get; set; }

        public HealthBar(Rectangle bar, int fullWidth, Color color, int xOffset, int yOffset)
        {
            this.bar = bar;
            this.fullWidth = fullWidth;
            this.color = color;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }

        public Rectangle getBar()
        {
            return this.bar;
        }

        public void setBar(Rectangle bar)
        {
            this.bar = bar;
        }

        public void updateHealthBar(int maxHp, int currHp)
        {
            float percentHealthBar = (float)currHp / (float)maxHp;
            bar.Width = (int)(fullWidth * percentHealthBar);
        }
    }
}
