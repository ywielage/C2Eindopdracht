using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class Level
    {
        private int width;
        private int height;
        private List<List<LevelComponent>> list;
        
        public Level(int width, int height, bool debug)
        {
            this.width = width;
            this.height = height;
            list = initList(this.width, this.height);

            int resetCount = 0;
            while(true)
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
        }

        private bool createPath(int width, int height, bool debug)
        {
            int xPos = 0;
            int yPos = 0;
            Side lastExit = Side.SOUTH;
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
                        printLevelComponent(xPos, yPos, Side.WEST, 3);
                    }
                    
                    list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], Side.WEST, Side.SOUTH);
                    lastExit = Side.SOUTH;
                    yPos++;
                }
                else if (yPos + 1 == height && xPos + 1 == width)
                {
                    if (debug)
                    {
                        printLevelComponent(xPos, yPos, lastExit, 2);
                        Debug.WriteLine("=======");
                    }
                    list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], getOppositeSide(lastExit), Side.EAST);
                    return true;
                }
                else
                {
                    int randomDirection = random.Next(1, 5);

                    if (debug)
                    {
                        printLevelComponent(xPos, yPos, lastExit, randomDirection);
                    }

                    Side exit;
                    if (randomDirection == 1 && yPos > 0)
                    {
                        if (lastExit != Side.SOUTH && !list[yPos-1][xPos].isFilled)
                        {
                            exit = Side.NORTH;
                            list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], getOppositeSide(lastExit), exit);
                            lastExit = exit;
                            yPos--;
                            count = 0;
                        }

                    }
                    else if (randomDirection == 2 && xPos < width-1)
                    {
                        if (lastExit != Side.WEST && !list[yPos][xPos+1].isFilled)
                        {
                            exit = Side.EAST;
                            list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], getOppositeSide(lastExit), exit);
                            lastExit = exit;
                            xPos++;
                            count = 0;
                        }

                    }
                    else if (randomDirection == 3 && yPos < height-1)
                    {
                        if (lastExit != Side.NORTH && !list[yPos+1][xPos].isFilled)
                        {
                            exit = Side.SOUTH;
                            list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], getOppositeSide(lastExit), exit);
                            lastExit = exit;
                            yPos++;
                            count = 0;
                        }

                    }
                    else if (randomDirection == 4 && xPos > 0)
                    {
                        if (lastExit != Side.EAST && !list[yPos][xPos-1].isFilled)
                        {
                            exit = Side.WEST;
                            list[yPos][xPos] = setSelectedListPos(list[yPos][xPos], getOppositeSide(lastExit), exit);
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
                    list[i].Add(new LevelComponent(Side.NONE, Side.NONE));
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
                    list[i][j] = new LevelComponent(Side.NONE, Side.NONE);
                }
            }
        }

        private LevelComponent setSelectedListPos(LevelComponent levelComponent, Side entrance, Side exit)
        {
            levelComponent.exit = exit;
            levelComponent.entrance = entrance;
            levelComponent.isFilled = true;

            return levelComponent;
        }

        private void printLevelComponent(int xPos, int yPos, Side entrance, int randomDirection)
        {
            Debug.WriteLine("=======");
            Debug.WriteLine("xPos: " + xPos);
            Debug.WriteLine("yPos: " + yPos);
            Debug.WriteLine("Entrance: " + getOppositeSide(entrance));

            if (randomDirection == 1)
            {
                Debug.WriteLine("Exit: " + Side.NORTH);
            }
            else if (randomDirection == 2)
            {
                Debug.WriteLine("Exit: " + Side.EAST);
            }
            else if (randomDirection == 3)
            {
                Debug.WriteLine("Exit: " + Side.SOUTH);
            }
            else if (randomDirection == 4)
            {
                Debug.WriteLine("Exit: " + Side.WEST);
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
                                Debug.Write(getStringLevelComponentOpening(list[i][j], Side.NORTH, y));
                            }
                            else if (x == 1 && y == 2)
                            {
                                Debug.Write(getStringLevelComponentOpening(list[i][j], Side.WEST, y));
                            }
                            else if (x == 3 && y == 2)
                            {
                                Debug.Write(getStringLevelComponentOpening(list[i][j], Side.EAST, y));
                            }
                            else if (x == 2 && y == 3)
                            {
                                Debug.Write(getStringLevelComponentOpening(list[i][j], Side.SOUTH, y));
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

        private string getStringLevelComponentOpening(LevelComponent levelComponent, Side side, int yHeight)
        {
            Side setSide = Side.NONE;
            if(levelComponent.exit == side)
            {
                setSide = side;
                
            }
            else if(levelComponent.entrance == side)
            {
                setSide = getOppositeSide(side);
            }

            if(setSide != Side.NONE)
            {
                if (setSide == Side.NORTH)
                {
                    return "^ ";
                }
                else if (setSide == Side.EAST)
                {
                    return "> ";
                }
                else if (setSide == Side.SOUTH)
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

        private Side getOppositeSide(Side side)
        {
            if (side == Side.NORTH)
            {
                return Side.SOUTH;
            }
            else if (side == Side.EAST)
            {
                return Side.WEST;
            }
            else if (side == Side.SOUTH)
            {
                return Side.NORTH;
            }
            else if (side == Side.WEST)
            {
                return Side.EAST;
            }
            return Side.NORTH;
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