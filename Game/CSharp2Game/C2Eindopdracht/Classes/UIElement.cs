using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class UIElement
	{
		public string label { get; set; }
		public int value { get; set; }
		public Vector2 parentOffset { get; set; }
		public Vector2 position { get; set; }

		public UIElement(string label, int value, Vector2 parentOffset, Vector2 position)
		{
			this.label = label;
			this.value = value;
			this.parentOffset = parentOffset;
			this.position = position;
		}
	}
}
