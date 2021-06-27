using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class LevelTooSmallException : Exception
	{
		public LevelTooSmallException(int width, int height) : base(String.Format("Level is too small must be at least 2x2: width {0}, height {1}", width, height))
		{
			
		}
	}
}
