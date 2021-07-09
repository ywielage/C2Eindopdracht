using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class HealthBar
    {
        public Rectangle bar { get; set; }
        public int fullWidth { get; set; }
        public Color color { get; set; }
        public int xOffset { get; set; }
        public int yOffset { get; set; }

        /// <summary>
        /// Visual representation of the health bar
        /// </summary>
        /// <param name="bar">Bar itself</param>  
        /// <param name="fullWidth">Max length if full hp</param> 
        /// <param name="color">Color of the bar</param> 
        /// <param name="xOffset">Horizontal offset from unit</param>
        /// <param name="yOffset">Vertical offset from unit</param> 
        public HealthBar(Rectangle bar, int fullWidth, Color color, int xOffset, int yOffset)
        {
            this.bar = bar;
            this.fullWidth = fullWidth;
            this.color = color;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }

        /// <summary>
        /// Change values healthbar
        /// </summary>
        /// <param name="maxHp">Maximum hp</param> 
        /// <param name="currHp"></param> Current hp
        public void updateHealthBar(int maxHp, int currHp)
        {
            Rectangle tempBar = bar;
            float percentHealthBar = (float)currHp / (float)maxHp;
            tempBar.Width = (int)(fullWidth * percentHealthBar);
            bar = tempBar;
        }

        /// <summary>
        /// Draw the healthbar of the unit
        /// </summary>
        /// <param name="spriteBatch">Helper class for drawing text strings and sprites in one or more optimized batches</param>
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Game1.blankTexture,
                bar,
                color
            );
        }
    }
}
