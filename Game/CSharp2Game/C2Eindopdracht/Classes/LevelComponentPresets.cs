using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    static class LevelComponentPresets
    {
        public static List<Rectangle> getPresetWestEast(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 500, 200));
            colliders.Add(new Rectangle(xPos, yPos + 300, 500, 200));

            return colliders;
        }

        public static List<Rectangle> getPresetNorthSouth(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 200, 500));
            colliders.Add(new Rectangle(xPos + 300, yPos, 200, 500));
            colliders.Add(new Rectangle(xPos + 225, yPos + 25, 50, 25));
            colliders.Add(new Rectangle(xPos + 225, yPos + 125, 50, 25));
            colliders.Add(new Rectangle(xPos + 225, yPos + 225, 50, 25));
            colliders.Add(new Rectangle(xPos + 225, yPos + 325, 50, 25));
            colliders.Add(new Rectangle(xPos + 225, yPos + 425, 50, 25));

            return colliders;
        }

        public static List<Rectangle> getPresetWestSouth(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 300, 200));
            colliders.Add(new Rectangle(xPos + 300, yPos, 200, 500));
            colliders.Add(new Rectangle(xPos, yPos + 300, 200, 200));

            return colliders;
        }

        public static List<Rectangle> getPresetNorthEast(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 200, 300));
            colliders.Add(new Rectangle(xPos, yPos + 300, 500, 200));
            colliders.Add(new Rectangle(xPos + 300, yPos, 200, 200));

            return colliders;
        }

        public static List<Rectangle> getPresetWestNorth(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 200, 200));
            colliders.Add(new Rectangle(xPos + 300, yPos, 200, 500));
            colliders.Add(new Rectangle(xPos, yPos + 300, 500, 200));

            return colliders;
        }

        public static List<Rectangle> getPresetEastSouth(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 500, 200));
            colliders.Add(new Rectangle(xPos, yPos + 200, 200, 300));
            colliders.Add(new Rectangle(xPos + 300, yPos + 300, 200, 200));

            return colliders;
        }
        public static List<Rectangle> getPresetEmpty(int xPos, int yPos)
        {
            List<Rectangle> colliders = new List<Rectangle>();

            colliders.Add(new Rectangle(xPos, yPos, 500, 500));

            return colliders;
        }

    }
}
