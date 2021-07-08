using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class Level
    {
        public int width { get; set; }
        public int height { get; set; }
        public int levelComponentSize { get; set; }
        public int tileSize { get; set; }
        public TileMapLoader tileMapLoader { get; set; }
        public EnemySpawner enemySpawner { get; set; }
        public List<Enemy> enemies { get; set; }
        public List<List<LevelComponent>> list { get; set; }
        public Rectangle endTrigger { get; set; }
        
        public Level(int width, int height, int enemyAmount)
        {
            if(width < 2 || height < 2)
			{
                throw new LevelTooSmallException(width, height);
            }
            this.width = width + 2;
            this.height = height + 2;
            this.levelComponentSize = 384;
            this.tileSize = 24;
            this.endTrigger = new Rectangle(levelComponentSize * (width + 1) - tileSize, (levelComponentSize * (height + 1)- levelComponentSize / 2) - tileSize, tileSize, tileSize * 2);

            tileMapLoader = new TileMapLoader();
            enemySpawner = new EnemySpawner(enemyAmount);
            enemies = new List<Enemy>();
        }

        /// <summary>
        /// Initialize the level and assign the tilemaps and colliders
        /// </summary>
        /// <param name="debug">Debug mode to see which levelcomponent at that time</param>
        public void init(bool debug)
        {
            
            list = initList(width, height);

            int resetCount = 0;
            while (true)
            {
                if (createPath(width, height, debug))
                {
                    break;
                }
                resetList();
                if (debug)
                {
                    Debug.WriteLine("Path creation reset because it had no options - " + resetCount + " times");
                    resetCount++;
                }
            }

            setPositionOfEmptyLevelComponents();
            tileMapLoader.setTileMapsFromJson();
            assignComponentTileMapAndColliders(tileMapLoader);
            enemies = enemySpawner.spawnEnemies(width, height, levelComponentSize, tileSize, list);
        }

        /// <summary>
        /// Create the level based on the width and height given.
        /// </summary>
        /// <param name="width">The amount of levelcomponents the level is wide</param>
        /// <param name="height">The amount of levelcomponents the level is high</param>
        /// <param name="debug">Debug mode to see which levelcomponent at that time</param>
        /// <returns></returns>
        private bool createPath(int width, int height, bool debug)
        {
            int xPos = 1;
            int yPos = 1;
            Directions lastExit = Directions.SOUTH;
            var random = new Random();
            int count = 0;

            while (true)  
            {
                if(count == 3)
                {
                    if (!hasFreeNeighbours(xPos, yPos))
                    {
                        return false;
                    }
                    count = 0;
                }
                if (xPos == 1 && yPos == 1)
                {
                    if(debug)
                    {
                        printLevelComponent(xPos, yPos, Directions.WEST, 3);
                    }
                    
                    list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], Directions.WEST, Directions.SOUTH, 1, 1);
                    lastExit = Directions.SOUTH;
                    yPos++;
                }
                else if (yPos == height - 2 && xPos == width - 2)
                {
                    if (debug)
                    {
                        printLevelComponent(xPos, yPos, lastExit, 2);
                        Debug.WriteLine("=======");
                    }
                    list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], getOppositeSide(lastExit), Directions.EAST, xPos, yPos);
                    return true;
                }
                else
                {
                    int randomDirection = random.Next(1, 5);

                    if (debug)
                    {
                        printLevelComponent(xPos, yPos, lastExit, randomDirection);
                    }

                    Directions exit;
                    if (randomDirection == 1 && yPos > 1)
                    {
                        if (lastExit != Directions.SOUTH && !list[yPos-1][xPos].isFilled)
                        {
                            exit = Directions.NORTH;
                            list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], getOppositeSide(lastExit), exit, xPos, yPos);
                            lastExit = exit;
                            yPos--;
                            count = 0;
                        }

                    }
                    else if (randomDirection == 2 && xPos < width - 2)
                    {
                        if (lastExit != Directions.WEST && !list[yPos][xPos+1].isFilled)
                        {
                            exit = Directions.EAST;
                            list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], getOppositeSide(lastExit), exit, xPos, yPos);
                            lastExit = exit;
                            xPos++;
                            count = 0;
                        }

                    }
                    else if (randomDirection == 3 && yPos < height - 2)
                    {
                        if (lastExit != Directions.NORTH && !list[yPos+1][xPos].isFilled)
                        {
                            exit = Directions.SOUTH;
                            list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], getOppositeSide(lastExit), exit, xPos, yPos);
                            lastExit = exit;
                            yPos++;
                            count = 0;
                        }

                    }
                    else if (randomDirection == 4 && xPos > 1)
                    {
                        if (lastExit != Directions.EAST && !list[yPos][xPos-1].isFilled)
                        {
                            exit = Directions.WEST;
                            list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], getOppositeSide(lastExit), exit, xPos, yPos);
                            lastExit = exit;
                            xPos--;
                            count = 0;
                        }
                    }
                }
                count++;
            }
        }

        /// <summary>
        /// Iniitialize every levelcomponent so there is an empty level
        /// </summary>
        /// <param name="width">The amount of levelcomponents the level is wide</param>
        /// <param name="height">The amount of levelcomponents the level is high</param>
        /// <returns></returns>
        private List<List<LevelComponent>> initList(int width, int height)
        {
            List<List<LevelComponent>> list = new List<List<LevelComponent>>();

            for(int i = 0; i < height; i++)
            {
                list.Add(new List<LevelComponent>());
                for(int j = 0; j < width; j++)
                {
                    list[i].Add(new LevelComponent(Directions.NONE, Directions.NONE));
                }
            }
            return list;
        }

        /// <summary>
        /// Reset the level to an empty level
        /// </summary>
        private void resetList()
        {
            for(int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    list[i][j] = new LevelComponent(Directions.NONE, Directions.NONE);
                }
            }
        }

        /// <summary>
        /// Assign the tiles to every different levelcomponent taken from the JSON file
        /// Assign the colliders for every tile that is not empty
        /// </summary>
        /// <param name="tileMapLoader">The tilemap to take the tiles from</param>
        private void assignComponentTileMapAndColliders(TileMapLoader tileMapLoader)
        {
            assignEnd(tileMapLoader);
            foreach (List<LevelComponent> rowList in list)
            {
                foreach (LevelComponent levelComponent in rowList)
                {
                    if(!(levelComponent.entrance == Directions.WEST && levelComponent.exit == Directions.NONE))
					{
                        levelComponent.assignTileMap(tileMapLoader);
                        levelComponent.assignColliders(tileSize);
                    }
                }
            }
        }

        /// <summary>
        /// Assign the end levelcomponent to it's position
        /// </summary>
        /// <param name="tileMapLoader">The tilemap to take the tiles from</param>
        private void assignEnd(TileMapLoader tileMapLoader)
        {
            list[height - 2][width - 1].entrance = Directions.WEST;
            list[height - 2][width - 1].exit = Directions.NONE;
            list[height - 2][width - 1].assignTileMap(tileMapLoader);
            list[height - 2][width - 1].assignColliders(tileSize);
        }

        /// <summary>
        /// Set the levelcomponent position of every levelcomponent that's still empty
        /// </summary>
        private void setPositionOfEmptyLevelComponents()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (!list[i][j].isFilled)
                    {
                        list[i][j].position = new Point(j * levelComponentSize, i * levelComponentSize);
                    }
                }
            }
        }

        /// <summary>
        /// Set the current levelcomponent to the given entrance and exit
        /// </summary>
        /// <param name="levelComponent">The levelcomponent to set</param>
        /// <param name="entrance">The entrance of the levelcomponent</param>
        /// <param name="exit">The exit of the levelcomponent</param>
        /// <param name="xPos">The horizontal position of the levelcomponent</param>
        /// <param name="yPos">The vertical position of the levelcomponent</param>
        /// <returns>The filled in levelcomponent</returns>
        private LevelComponent setSelectedListPos(LevelComponent levelComponent, Directions entrance, Directions exit, int xPos, int yPos)
        {
            levelComponent.exit = exit;
            levelComponent.entrance = entrance;
            levelComponent.isFilled = true;

            Point position = levelComponent.position;
            position.X = xPos * levelComponentSize;
            position.Y = yPos * levelComponentSize;
            levelComponent.position = position;

            return levelComponent;
        }

        /// <summary>
        /// Print the current levelcomponent to debug
        /// </summary>
        /// <param name="xPos">The horizontal position of the levelcomponent</param>
        /// <param name="yPos">The vertical position of the levelcomponent</param>
        /// <param name="entrance">The entrance of the levelcomponent</param>
        /// <param name="randomDirection">The random direction to go in 1=N, 2=E, 3=S, 4=W</param>
        private void printLevelComponent(int xPos, int yPos, Directions entrance, int randomDirection)
        {
            Debug.WriteLine("=======");
            Debug.WriteLine("xPos: " + xPos);
            Debug.WriteLine("yPos: " + yPos);
            Debug.WriteLine("Entrance: " + getOppositeSide(entrance));

            if (randomDirection == 1)
            {
                Debug.WriteLine("Exit: " + Directions.NORTH);
            }
            else if (randomDirection == 2)
            {
                Debug.WriteLine("Exit: " + Directions.EAST);
            }
            else if (randomDirection == 3)
            {
                Debug.WriteLine("Exit: " + Directions.SOUTH);
            }
            else if (randomDirection == 4)
            {
                Debug.WriteLine("Exit: " + Directions.WEST);
            }
        }

        /// <summary>
        /// Draw every levelcomponent in a grid in debug
        /// </summary>
        public void drawLevelInDebug()
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int y = 1; y <= 3; y++)
                {
                    for (int j = 0; j < list[i].Count; j++)
                    {
                        for(int x = 1; x <= 3; x++)
                        {
                            if(y % 2 == 1 && x % 2 == 1)
                            {
                                Debug.Write("+ ");
                            }

                            if(x == 2 && y == 1)
                            {
                                Debug.Write(getStringLevelComponentOpening(list[i][j], Directions.NORTH, y));
                            }
                            else if (x == 1 && y == 2)
                            {
                                Debug.Write(getStringLevelComponentOpening(list[i][j], Directions.WEST, y));
                            }
                            else if (x == 3 && y == 2)
                            {
                                Debug.Write(getStringLevelComponentOpening(list[i][j], Directions.EAST, y));
                            }
                            else if (x == 2 && y == 3)
                            {
                                Debug.Write(getStringLevelComponentOpening(list[i][j], Directions.SOUTH, y));
                            }
                            else if (x == 2 && y == 2)
                            {
                                if(list[i][j].isFilled)
                                {
                                    Debug.Write("O ");
                                }
                                else
                                {
                                    Debug.Write("  ");
                                }
                            }
                        }
                        Debug.Write(" ");
                    }
                    Debug.Write("\n");
                }
                Debug.Write("\n");
            }
        }

        /// <summary>
        /// Get the representing character for the exit/entrance
        /// </summary>
        /// <param name="levelComponent">The levelcomponent used</param>
        /// <param name="side">The side the char should point to</param>
        /// <param name="yHeight">The current vertical height</param>
        /// <returns>A character representing an opening in drawLevelDebug</returns>
        private string getStringLevelComponentOpening(LevelComponent levelComponent, Directions side, int yHeight)
        {
            Directions setSide = Directions.NONE;
            if(levelComponent.exit == side)
            {
                setSide = side;
                
            }
            else if(levelComponent.entrance == side)
            {
                setSide = getOppositeSide(side);
            }

            if(setSide != Directions.NONE)
            {
                if (setSide == Directions.NORTH)
                {
                    return "^ ";
                }
                else if (setSide == Directions.EAST)
                {
                    return "> ";
                }
                else if (setSide == Directions.SOUTH)
                {
                    return "v ";
                }
                else
                {
                    return "< ";
                }
            }
            
            else
            {
                if(yHeight % 2 == 1)
                {
                    return "- ";
                }
                else
                {
                    return "| ";
                }
            }
        }

        /// <summary>
        /// Get the opposite of the Direction given
        /// </summary>
        /// <param name="side">Direction to reverse</param>
        /// <returns>The reversed Direction</returns>
        private Directions getOppositeSide(Directions side)
        {
            if (side == Directions.NORTH)
            {
                return Directions.SOUTH;
            }
            else if (side == Directions.EAST)
            {
                return Directions.WEST;
            }
            else if (side == Directions.SOUTH)
            {
                return Directions.NORTH;
            }
            else if (side == Directions.WEST)
            {
                return Directions.EAST;
            }
            return Directions.NORTH;
        }

        /// <summary>
        /// Check if the createpath method has any free neighbours and isn't stuck in a corner
        /// </summary>
        /// <param name="x">Current horizontal position</param>
        /// <param name="y">Current vertical position</param>
        /// <returns>True if there are free neighbours to any of the four sides of the given position</returns>
        private bool hasFreeNeighbours(int x, int y)
        {
            bool north = false;
            bool east = false;
            bool south = false;
            bool west = false;

            if(y > 1)
            {
                if(list[y - 1][x].isFilled)
                {
                    north = true;
                }
            }
            else
            {
                north = true;
            }

            if(x < width - 2)
            {
                if (list[y][x + 1].isFilled)
                {
                    east = true;
                }
            } 
            else
            {
                east = true;
            }

            if(y < height - 2)
            {
                if (list[y + 1][x].isFilled)
                {
                    south = true;
                }
            }
            else
            {
                south = true;
            }

            if (x > 1)
            {
                if (list[y][x - 1].isFilled)
                {
                    west = true;
                }
            }
            else
            {
                west = true;
            }

            if (north && east && south && west)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check if the player hit the end of the level
        /// </summary>
        /// <param name="playerHitbox">The hitbox of the player</param>
        /// <param name="ui">The UI</param>
        /// <param name="x">The horizontal position</param>
        /// <param name="y">The vertical position</param>
        public void checkEndTriggerHit(Rectangle playerHitbox, UI ui, float x, float y)
		{
            if (endTrigger.Intersects(playerHitbox))
            {
                if (enemies.Count == 0)
                {
                    ui.addUIElement("You've won!", new Vector2(x, y), 5f);
                }
                else
                {
                    ui.addUIElement("There are still " + enemies.Count + " enemies remaining", new Vector2(x, y), 5f);
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, bool renderHitboxes)
		{
            foreach(List<LevelComponent> rowList in list)
			{
                foreach(LevelComponent levelComponent in rowList)
				{
                    levelComponent.draw(spriteBatch, renderHitboxes, tileSize);
				}
			}
		}
    }
}