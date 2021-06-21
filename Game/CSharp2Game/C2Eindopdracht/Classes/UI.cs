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

		public void updatePosition(Vector2 position)
		{
			this.position = position;
			foreach(UIElement element in elements)
			{
				element.position = position + element.parentOffset;
			}
		}

		public void addUIElement(string label, int value, Vector2 parentOffset)
		{
			elements.Add(new UIElement(label, value, parentOffset, this.position));
		}

		public UIElement getUIElementByLabel(string label)
		{
			return elements.Find(element => element.label.Equals(label));
		}
	}
}
