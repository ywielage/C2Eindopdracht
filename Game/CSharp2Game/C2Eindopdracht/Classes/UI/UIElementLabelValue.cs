using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class UIElementLabelValue : UIElement
	{
		public int value { get; set; }
		public UIElementLabelValue(string label, int value, Vector2 parentOffset, Vector2 position, float activeTime) : base(label, parentOffset, position, activeTime)
		{
			this.value = value;
		}
	}
}
