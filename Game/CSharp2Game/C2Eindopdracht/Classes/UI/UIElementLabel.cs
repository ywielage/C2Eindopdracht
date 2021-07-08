using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class UIElementLabel : UIElement
	{
		public UIElementLabel(string label, Vector2 parentOffset, Vector2 position, float activeTime) : base(label, parentOffset, position, activeTime)
		{
		}

		public override string getDrawText()
		{
			return label;
		}
	}
}
