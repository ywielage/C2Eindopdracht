using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	abstract class UIElement
	{
		public string label { get; set; }
		public Vector2 parentOffset { get; set; }
		public Vector2 position { get; set; }
		public Cooldown activeTime { get; set; }
		public bool expired { get; set; }

		/// <summary>
		/// UI element to hold text
		/// </summary>
		/// <param name="label">Text to hold</param>
		/// <param name="parentOffset">Offset from UI parent topleft position</param>
		/// <param name="position">Position in the game</param>
		/// <param name="activeTime">Time the UI element is active</param>
		public UIElement(string label, Vector2 parentOffset, Vector2 position, float activeTime)
		{
			this.label = label;
			this.parentOffset = parentOffset;
			this.position = position;
			if(activeTime == 0)
			{
				this.activeTime = null;
			}
			else
			{
				this.activeTime = new Cooldown(activeTime);
			}
			this.expired = false;
		}

		/// <summary>
		/// Update the position and active time
		/// </summary>
		/// <param name="gameTime">Holds the timestate of a Game</param>
		public void update(Vector2 parentPosition, GameTime gameTime)
		{
			position = parentPosition + parentOffset;
			if (activeTime != null)
			{
				activeTime.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (activeTime.elapsedTime >= activeTime.duration)
				{
					expired = true;
				}
			}
		}

		/// <summary>
		/// Draw the UI element
		/// </summary>
		/// <param name="spriteBatch">Helper class for drawing text strings and sprites in one or more optimized batches</param>
		/// <param name="font">Font to use for the UI elements</param>
		public void draw(SpriteBatch spriteBatch, SpriteFont font)
		{
			spriteBatch.DrawString(
				font,
				getDrawText(),
				position,
				Color.White,
				0f,
				new Vector2(0, 0),
				1f,
				SpriteEffects.None,
				.1f
			);
		}

		/// <summary>
		/// Get the UI text that should be drawn
		/// </summary>
		/// <returns>A string with the text to be drawn</returns>
		public abstract string getDrawText();
	}
}
