using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class LevelComponent
    {
        public bool isFilled { get; set; }
        public Directions entrance { get; set; }
        public Directions exit { get; set; }
        public List<Rectangle> colliders { get; set; }
        public Vector2 position { get; set; }

        public LevelComponent(Directions entrance, Directions exit)
        {
            this.isFilled = false;
            this.entrance = entrance;
            this.exit = exit;
            colliders = new List<Rectangle>();
        }

        public void assignColliders()
        {
            if((entrance == Directions.WEST || exit == Directions.WEST) && (entrance == Directions.EAST || exit == Directions.EAST))
            {
                colliders = LevelComponentPresets.getPresetWestEastPreset((int) position.X, (int) position.Y);
            }
            else if ((entrance == Directions.NORTH || exit == Directions.NORTH) && (entrance == Directions.SOUTH || exit == Directions.SOUTH))
            {
                colliders = LevelComponentPresets.getPresetNorthSouthPreset((int)position.X, (int)position.Y);
            }
            else if ((entrance == Directions.WEST || exit == Directions.WEST) && (entrance == Directions.SOUTH || exit == Directions.SOUTH))
            {
                colliders = LevelComponentPresets.getPresetWestSouthPreset((int)position.X, (int)position.Y);
            }
            else if ((entrance == Directions.NORTH || exit == Directions.NORTH) && (entrance == Directions.EAST || exit == Directions.EAST))
            {
                colliders = LevelComponentPresets.getPresetNorthEastPreset((int)position.X, (int)position.Y);
            }
            else if ((entrance == Directions.WEST || exit == Directions.WEST) && (entrance == Directions.NORTH || exit == Directions.NORTH))
            {
                colliders = LevelComponentPresets.getPresetWestNorthPreset((int)position.X, (int)position.Y);
            }
            else if ((entrance == Directions.EAST || exit == Directions.EAST) && (entrance == Directions.SOUTH || exit == Directions.SOUTH))
            {
                colliders = LevelComponentPresets.getPresetEastSouthPreset((int)position.X, (int)position.Y);
            }
            else
            {
                colliders = LevelComponentPresets.getPresetEmptyPreset((int)position.X, (int)position.Y);
            }
        }
    }

    enum Directions
    {
        NORTH,
        EAST,
        SOUTH,
        WEST,
        NONE
    }
}
