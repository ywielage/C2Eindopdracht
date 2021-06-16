using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    static class LevelComponentPresets
    {
        public static List<Rectangle> getPresetWestEastPreset(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 500, 200));
            colliders.Add(new Rectangle(xPos, yPos + 300, 500, 200));

            return colliders;
        }

        public static List<Rectangle> getPresetNorthSouthPreset(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 200, 500));
            colliders.Add(new Rectangle(xPos + 300, yPos, 200, 500));

            return colliders;
        }

        public static List<Rectangle> getPresetWestSouthPreset(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 300, 200));
            colliders.Add(new Rectangle(xPos + 300, yPos, 200, 500));
            colliders.Add(new Rectangle(xPos, yPos + 300, 200, 200));

            return colliders;
        }

        public static List<Rectangle> getPresetNorthEastPreset(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 200, 300));
            colliders.Add(new Rectangle(xPos, yPos + 300, 500, 200));
            colliders.Add(new Rectangle(xPos + 300, yPos, 200, 200));

            return colliders;
        }

        public static List<Rectangle> getPresetWestNorthPreset(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 200, 200));
            colliders.Add(new Rectangle(xPos + 300, yPos, 200, 500));
            colliders.Add(new Rectangle(xPos, yPos + 300, 500, 200));

            return colliders;
        }

        public static List<Rectangle> getPresetEastSouthPreset(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 500, 200));
            colliders.Add(new Rectangle(xPos, yPos + 200, 200, 300));
            colliders.Add(new Rectangle(xPos + 300, yPos + 300, 200, 200));

            return colliders;
        }
        public static List<Rectangle> getPresetEmptyPreset(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 500, 500));

            return colliders;
        }

    }
}
