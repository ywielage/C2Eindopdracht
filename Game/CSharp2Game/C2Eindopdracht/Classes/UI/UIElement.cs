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
		}

		/// <summary>
		/// Draw the UI element
		/// </summary>
		/// <param name="spriteBatch">Helper class for drawing text strings and sprites in one or more optimized batches</param>
		/// <param name="arial16">Arial font size 16</param>
		public void draw(SpriteBatch spriteBatch, SpriteFont arial16)
		{
			spriteBatch.DrawString(
				arial16,
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
