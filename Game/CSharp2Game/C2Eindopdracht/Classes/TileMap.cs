using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class TileMap
    {
        public Directions openingOne { get; set; }
        public Directions openingTwo { get; set; }

        public List<List<int>> tiles;

        public TileMap()
        {
            this.tiles = new List<List<int>>();
        }
    }
}
