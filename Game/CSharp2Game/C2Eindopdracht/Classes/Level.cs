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
        private int width;
        private int height;
        private TileMapLoader tileMapLoader;
        public List<List<LevelComponent>> list { get; set; }
        
        public Level(int width, int height)
        {
            if(width < 2 || height < 2)
			{
                return;
            }
            this.width = width;
            this.height = height;

            tileMapLoader = new TileMapLoader();
        }

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
        }

        private bool createPath(int width, int height, bool debug)
        {
            int xPos = 0;
            int yPos = 0;
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
                if (xPos == 0 && yPos == 0)
                {
                    if(debug)
                    {
                        printLevelComponent(xPos, yPos, Directions.WEST, 3);
                    }
                    
                    list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], Directions.WEST, Directions.SOUTH, 0, 0);
                    lastExit = Directions.SOUTH;
                    yPos++;
                }
                else if (yPos + 1 == height && xPos + 1 == width)
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
                    if (randomDirection == 1 && yPos > 0)
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
                    else if (randomDirection == 2 && xPos < width-1)
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
                    else if (randomDirection == 3 && yPos < height-1)
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
                    else if (randomDirection == 4 && xPos > 0)
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

        private void assignComponentTileMapAndColliders(TileMapLoader tileMapLoader)
        {
            foreach (List<LevelComponent> rowList in list)
            {
                foreach (LevelComponent levelComponent in rowList)
                {
                    levelComponent.assignTileMap(tileMapLoader);
                    levelComponent.assignColliders();
                }
            }
        }

        private void setPositionOfEmptyLevelComponents()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (!list[i][j].isFilled)
                    {
                        list[i][j].position = new Point(j * 384, i * 384);
                    }
                }
            }
        }

        private LevelComponent setSelectedListPos(LevelComponent levelComponent, Directions entrance, Directions exit, int xPos, int yPos)
        {
            levelComponent.exit = exit;
            levelComponent.entrance = entrance;
            levelComponent.isFilled = true;

            Point position = levelComponent.position;
            position.X = xPos * 384;
            position.Y = yPos * 384;
            levelComponent.position = position;

            return levelComponent;
        }

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

        private bool hasFreeNeighbours(int x, int y)
        {
            bool north = false;
            bool east = false;
            bool south = false;
            bool west = false;

            if(y > 0)
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

            if(x < width - 1)
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

            if(y < height - 1)
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

            if (x > 0)
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
    }
}