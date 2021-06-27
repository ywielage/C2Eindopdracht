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

		public void update(Vector2 position, GameTime gameTime)
		{
			this.position = position;
			List<UIElement> elapsedUI = new List<UIElement>();

			foreach (UIElement element in elements)
			{
				element.position = position + element.parentOffset;
				if (element.activeTime != null)
				{
					element.activeTime.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (element.activeTime.elapsedTime >= element.activeTime.duration)
					{
						elapsedUI.Add(element);
					}
				}
			}
			elements.RemoveAll(element => elapsedUI.Contains(element));
		}

		public void addUIElement(string label, Vector2 parentOffset, float activeTime)
		{
			if(getUIElementByLabel(label) == null)
			{
				elements.Add(new UIElementLabel(label, parentOffset, this.position, activeTime));
			}
		}

		public void addUIElement(string label, int value, Vector2 parentOffset, float activeTime)
		{
			if (getUIElementByLabel(label) == null)
			{
				elements.Add(new UIElementLabelValue(label, value, parentOffset, this.position, activeTime));
			}
		}

		public UIElement getUIElementByLabel(string label)
		{
			return elements.Find(element => element.label.Equals(label));
		}
	}
}
