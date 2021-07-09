using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class UI
	{
		public Vector2 position { get; set; }
		public List<UIElement> elements { get; set; }

		public UI(Vector2 position)
		{
			this.position = position;
			this.elements = new List<UIElement>();
		}

		/// <summary>
		/// Update the UI position and all of it's elements
		/// </summary>
		/// <param name="position">Position to update to</param>
		/// <param name="gameTime">Holds the timestate of a Game</param>
		public void update(Vector2 position, GameTime gameTime)
		{
			this.position = position;
			List<UIElement> elapsedUI = new List<UIElement>();

			foreach (UIElement element in elements)
			{
				element.update(position, gameTime);
			}
			elements.RemoveAll(element => element.expired);
		}

		/// <summary>
		/// Add a label UI element
		/// </summary>
		/// <param name="label">The text of the UI element</param>
		/// <param name="parentOffset">The offset of the topleft position of the screen</param>
		/// <param name="activeTime">The time the UI element is active, 0 if forever</param>
		public void addUIElement(string label, Vector2 parentOffset, float activeTime)
		{
			if(getUIElementByLabel(label) == null)
			{
				elements.Add(new UIElementLabel(label, parentOffset, this.position, activeTime));
			}
		}

		/// <summary>
		/// Add a label: value UI element
		/// </summary>
		/// <param name="label">The text of the UI element</param>
		/// <param name="value">The value the UI element is holding</param>
		/// <param name="parentOffset">The offset of the topleft position of the screen</param>
		/// <param name="activeTime">The time the UI element is active, 0 if forever</param>
		public void addUIElement(string label, int value, Vector2 parentOffset, float activeTime)
		{
			if (getUIElementByLabel(label) == null)
			{
				elements.Add(new UIElementLabelValue(label, value, parentOffset, this.position, activeTime));
			}
		}

		/// <summary>
		/// Get a UI element from the list from the label given
		/// </summary>
		/// <param name="label">A UI label text</param>
		/// <returns>The UI element matching the given string</returns>
		public UIElement getUIElementByLabel(string label)
		{
			return elements.Find(element => element.label.Equals(label));
		}
	}
}
