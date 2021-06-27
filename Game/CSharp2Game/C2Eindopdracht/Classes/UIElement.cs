using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class UIElement
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
	}
}
