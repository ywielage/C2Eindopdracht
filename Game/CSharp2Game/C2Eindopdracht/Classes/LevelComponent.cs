using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class LevelComponent
    {
        public bool isFilled { get; set; }
        public Side entrance { get; set; }
        public Side exit { get; set; }

        public LevelComponent(Side entrance, Side exit)
        {
            this.isFilled = false;
            this.entrance = entrance;
            this.exit = exit;
        }
    }

    enum Side
    {
        NORTH,
        EAST,
        SOUTH,
        WEST,
        NONE
    }
}
