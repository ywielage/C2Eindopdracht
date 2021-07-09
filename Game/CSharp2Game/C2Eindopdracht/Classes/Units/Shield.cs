using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes.Units
{
	class Shield
	{
        public HealthBar bar { get; set; }
		public bool isActive { get; set; }
		public bool isEmpty { get; set; }
		public int currFill { get; set; }
		public int maxFill { get; set; }
		public int fillSpeed { get; set; }

		public Shield(bool isActive, bool isEmpty, int maxFill, int fillSpeed)
		{
			this.isActive = isActive;
			this.isEmpty = isEmpty;
			this.currFill = 0;
			this.maxFill = maxFill;
			this.fillSpeed = fillSpeed;
		}

        /// <summary>
        /// Activate the shield giving the player invulnerability
        /// </summary>
		public void activate()
		{
            if (currFill <= maxFill && !isEmpty)
            {
                isActive = true;
                currFill++;
                if (currFill == maxFill)
                {
                    isEmpty = true;
                }
            }
            else if(isEmpty)
			{
                isActive = false;
                currFill -= fillSpeed;
                if (currFill == 0)
                {
                    isEmpty = false;
                }
            }
        }

        /// <summary>
        /// Deactivate the shield making the player vulnerable again
        /// </summary>
        public void deactivate()
		{
            if (isEmpty)
            {
                isActive = false;
                currFill -= fillSpeed;
                if (currFill == 0)
                {
                    isEmpty = false;
                }
            }
            isActive = false;
        }
	}
}
