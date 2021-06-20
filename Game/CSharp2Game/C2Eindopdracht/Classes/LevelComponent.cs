using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class LevelComponent
    {
        public bool isFilled { get; set; }
        public Directions entrance { get; set; }
        public Directions exit { get; set; }
        public List<Rectangle> colliders { get; set; }
        public TileMap tileMap { get; set; }
        public Point position { get; set; }
        public static Texture2D tileSet { get; set; }

        public LevelComponent(Directions entrance, Directions exit)
        {
            this.isFilled = false;
            this.entrance = entrance;
            this.exit = exit;
            colliders = new List<Rectangle>();
        }

        public void assignTileMap(TileMapLoader tileMapLoader)
        {
            this.tileMap = tileMapLoader.getTileMap(entrance, exit);
        }

        public void assignColliders(int tileSize)
        {
            for (int i = 0; i < tileMap.tiles.Count; i++)
            {
                for (int j = 0; j < tileMap.tiles[i].Count; j++)
                {
                    if (tileMap.tiles[i][j] != 0)
                    {
                        colliders.Add(new Rectangle(position.X + (j * tileSize), position.Y + (i * tileSize), tileSize+1, tileSize+1));
                    }
                }
            }
        }

        public Rectangle? getTileTextureOffset(int tileId, int tileSize)
        {
            switch (tileId)
            {
                case 1:
                    return new Rectangle(0, 0, tileSize+1, tileSize + 1);
                case 2:
                    return new Rectangle(30, 0, tileSize + 1, tileSize + 1);
                case 3:
                    return new Rectangle(60, 0, tileSize + 1, tileSize + 1);
                case 4:
                    return new Rectangle(90, 0, tileSize + 1, tileSize + 1);
                case 5:
                    return new Rectangle(0, 30, tileSize + 1, tileSize + 1);
                case 6:
                    return new Rectangle(30, 30, tileSize + 1, tileSize + 1);
                case 7:
                    return new Rectangle(60, 30, tileSize + 1, tileSize + 1);
                case 8:
                    return new Rectangle(90, 30, tileSize + 1, tileSize + 1);
                case 9:
                    return new Rectangle(0, 60, tileSize + 1, tileSize + 1);
                case 10:
                    return new Rectangle(30, 60, tileSize + 1, tileSize + 1);
                case 11:
                    return new Rectangle(60, 60, tileSize + 1, tileSize + 1);
                case 12:
                    return new Rectangle(90, 60, tileSize + 1, tileSize + 1);
                case 13:
                    return new Rectangle(0, 90, tileSize + 1, tileSize + 1);
                case 14:
                    return new Rectangle(30, 90, tileSize + 1, tileSize + 1);
                case 15:
                    return new Rectangle(60, 90, tileSize + 1, tileSize + 1);
                case 16:
                    return new Rectangle(90, 90, tileSize + 1, tileSize + 1);

                default:
                    return null;
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
