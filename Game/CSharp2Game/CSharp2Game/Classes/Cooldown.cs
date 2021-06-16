using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class Cooldown
    {
        public float duration { get; set; }
        public float elapsedTime { get; set; }

        public Cooldown(float duration)
        {
            this.duration = duration;
            this.elapsedTime = 0f;
        }
    }
}
