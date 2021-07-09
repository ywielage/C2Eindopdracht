using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes.Screens
{
	class PauseScreen
	{
		public UI ui { get; set; }
		public PauseScreen(Vector2 uiPosition)
		{
			ui = new UI(uiPosition);
		}

		/// <summary>
		/// Update the pause screen
		/// </summary>
		/// <param name="position">The anchor of the pause screen elements</param>
		/// <param name="gameTime">Holds the timestate of a Game</param>
		public void update(Vector2 position, GameTime gameTime)
		{
			ui.update(position, gameTime);
		}

		/// <summary>
		/// Draw the pause screen
		/// </summary>
		/// <param name="spriteBatch">Helper class for drawing text strings and sprites in one or more optimized batches</param>
		/// <param name="font">Font to use for the UI elements</param>
		public void draw(SpriteBatch spriteBatch, SpriteFont font)
		{
			foreach (UIElement element in ui.elements)
			{
				element.draw(spriteBatch, font);
			}
		}
	}
}
