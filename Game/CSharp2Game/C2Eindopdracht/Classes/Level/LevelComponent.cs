using C2Eindopdracht.Classes.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class LevelComponent : ITileSet
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

        /// <summary>
        /// Assign a tilemap to this levelcomponent
        /// </summary>
        /// <param name="tileMapLoader">The tilemaploader to use</param>
        public void assignTileMap(TileMapLoader tileMapLoader)
        {
            this.tileMap = tileMapLoader.getTileMap(entrance, exit);
        }

        /// <summary>
        /// Assign colliders to this levelcomponent
        /// </summary>
        /// <param name="tileSize">The vertical and horizontal size of the tiles</param>
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

        /// <summary>
        /// Get the tiletextureOffset from the tileset
        /// </summary>
        /// <param name="tileId">The position to get from the tileset 1-16 going from left to right, then from top to bottom</param>
        /// <param name="tileSize">The size of the tile</param>
        /// <returns>Returns an area of the tileset to use</returns>
        public Rectangle? getTileTextureOffset(int tileId, int tileSize)
        {
            tileSize++;
            int offSet = tileSize + 5;
            switch (tileId)
            {
                case 1:
                    return new Rectangle(0, 0, tileSize, tileSize);
                case 2:
                    return new Rectangle(offSet, 0, tileSize, tileSize);
                case 3:
                    return new Rectangle(offSet * 2, 0, tileSize , tileSize);
                case 4:
                    return new Rectangle(offSet * 3, 0, tileSize, tileSize);
                case 5:
                    return new Rectangle(0, offSet, tileSize, tileSize);
                case 6:
                    return new Rectangle(offSet, offSet, tileSize, tileSize);
                case 7:
                    return new Rectangle(offSet * 2, offSet, tileSize, tileSize);
                case 8:
                    return new Rectangle(offSet * 3, offSet, tileSize, tileSize);
                case 9:
                    return new Rectangle(0, offSet * 2, tileSize, tileSize);
                case 10:
                    return new Rectangle(offSet, offSet * 2, tileSize, tileSize);
                case 11:
                    return new Rectangle(offSet * 2, offSet * 2, tileSize, tileSize);
                case 12:
                    return new Rectangle(offSet * 3, offSet * 2, tileSize, tileSize);
                case 13:
                    return new Rectangle(0, offSet * 3, tileSize, tileSize);
                case 14:
                    return new Rectangle(offSet, offSet * 3, tileSize, tileSize);
                case 15:
                    return new Rectangle(offSet * 2, offSet * 3, tileSize, tileSize);
                case 16:
                    return new Rectangle(offSet * 3, offSet * 3, tileSize, tileSize);

                default:
                    return new Rectangle(offSet, offSet * 3, tileSize + 1, tileSize + 1);
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
