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
        
        public void draw(SpriteBatch spriteBatch, bool renderHitboxes, int tileSize)
		{
            if(renderHitboxes)
			{
				foreach (Rectangle wall in colliders)
				{
					spriteBatch.Draw(
						Game1.blankTexture,
						wall,
						Color.DimGray
					);
				}
		    }
            else
			{
                for (int i = 0; i < tileMap.tiles.Count; i++)
                {
                    for (int j = 0; j < tileMap.tiles[i].Count; j++)
                    {
                        if (tileMap.tiles[i][j] != 0)
                        {
                            spriteBatch.Draw(
                                tileSet,
                                new Vector2(position.X + (j * 24), position.Y + (i * 24)),
                                getTileTextureOffset(tileMap.tiles[i][j], tileSize),
                                Color.White
                            );
                        }
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
        private Rectangle? getTileTextureOffset(int tileId, int tileSize)
        {
            tileSize++;
            int offSet = tileSize + 5;
            return tileId switch
            {
                1 => new Rectangle(0, 0, tileSize, tileSize),
                2 => new Rectangle(offSet, 0, tileSize, tileSize),
                3 => new Rectangle(offSet * 2, 0, tileSize, tileSize),
                4 => new Rectangle(offSet * 3, 0, tileSize, tileSize),
                5 => new Rectangle(0, offSet, tileSize, tileSize),
                6 => new Rectangle(offSet, offSet, tileSize, tileSize),
                7 => new Rectangle(offSet * 2, offSet, tileSize, tileSize),
                8 => new Rectangle(offSet * 3, offSet, tileSize, tileSize),
                9 => new Rectangle(0, offSet * 2, tileSize, tileSize),
                10 => new Rectangle(offSet, offSet * 2, tileSize, tileSize),
                11 => new Rectangle(offSet * 2, offSet * 2, tileSize, tileSize),
                12 => new Rectangle(offSet * 3, offSet * 2, tileSize, tileSize),
                13 => new Rectangle(0, offSet * 3, tileSize, tileSize),
                14 => new Rectangle(offSet, offSet * 3, tileSize, tileSize),
                15 => new Rectangle(offSet * 2, offSet * 3, tileSize, tileSize),
                16 => new Rectangle(offSet * 3, offSet * 3, tileSize, tileSize),
                _ => new Rectangle(offSet, offSet * 3, tileSize, tileSize),
            };
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
