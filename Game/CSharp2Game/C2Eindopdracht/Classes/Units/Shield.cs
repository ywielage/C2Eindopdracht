namespace C2Eindopdracht.Classes.Units
{
	class Shield
	{
        public HealthBar healthBar { get; set; }
		public bool isActive { get; set; }
		public bool isEmpty { get; set; }
		public int currFill { get; set; }
		public int maxFill { get; set; }
		public int fillSpeed { get; set; }

        /// <summary>
        /// Shield class for keeping track of the player shield
        /// </summary>
        /// <param name="healthBar">Visual representation of the shield</param>
        /// <param name="isActive">Shield is active</param>
        /// <param name="isEmpty">Shield is depleted</param>
        /// <param name="maxFill">Maximum shield value</param>
        /// <param name="fillSpeed">Speed with which the shield fills up when not used</param>
		public Shield(HealthBar healthBar, bool isActive, bool isEmpty, int maxFill, int fillSpeed)
		{
            this.healthBar = healthBar;
            this.isActive = isActive;
			this.isEmpty = isEmpty;
			this.currFill = maxFill;
			this.maxFill = maxFill;
			this.fillSpeed = fillSpeed;
		}

        /// <summary>
        /// Activate the shield giving the player invulnerability
        /// </summary>
		public void activate()
		{
            if (currFill >= 0 && !isEmpty)
            {
                isActive = true;
                currFill--;
                if (currFill == 0)
                {
                    isEmpty = true;
                }
                healthBar.updateHealthBar(maxFill, currFill);
            }
            else if(isEmpty)
			{
                isActive = false;
                currFill += fillSpeed;
                if (currFill == maxFill)
                {
                    isEmpty = false;
                }
                healthBar.updateHealthBar(maxFill, currFill);
            }
        }

        /// <summary>
        /// Deactivate the shield making the player vulnerable again
        /// </summary>
        public void deactivate()
		{
            isActive = false;
            if (currFill < maxFill)
			{
                currFill += fillSpeed;
            }
            if (isEmpty && currFill == maxFill)
            {
                isEmpty = false;
            }
            healthBar.updateHealthBar(maxFill, currFill);
        }
	}
}
