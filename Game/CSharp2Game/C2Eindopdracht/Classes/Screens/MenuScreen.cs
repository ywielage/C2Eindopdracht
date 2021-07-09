using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes.Screens
{
	class MenuScreen
	{
		public Rectangle screen { get; set; }
		public static Texture2D tileSet { get; set; }

		public MenuScreen(Rectangle screen)
		{
			this.screen = screen;
		}

		/// <summary>
		/// Draw the menu screen
		/// </summary>
		/// <param name="spriteBatch">Helper class for drawing text strings and sprites in one or more optimized batches</param>
		public void draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(
				tileSet, 
				screen, 
				Color.White
			);
		}
	}
}
