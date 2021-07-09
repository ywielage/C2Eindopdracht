using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class EnemySpawner
	{
		public int amount { get; set; }
		public List<Enemy> enemyPresets { get; set; }

		/// <summary>
		/// Sets amount of enemies
		/// </summary>
		/// <param name="amount">Amount of enemies</param>
		public EnemySpawner(int amount)
		{
			this.amount = amount;
			this.enemyPresets = new List<Enemy>();
			setEnemyPresets();
		}

		/// <summary>
		/// Sets the list of enemies to pick from
		/// </summary>
		private void setEnemyPresets()
		{
			enemyPresets.Add(new MageEnemy(0, 0, 18, 30, 3));
			enemyPresets.Add(new FighterEnemy(0, 0, 18, 30, 5));
		}

		/// <summary>
		/// Spawn a random enemy in any of the empty spots in the level
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="levelComponent"></param>
		/// <param name="tileSize"></param>
		/// <param name="levelComponents"></param>
		/// <returns></returns>
		public List<Enemy> spawnEnemies(int width, int height, int levelComponent, int tileSize, List<List<LevelComponent>> levelComponents)
		{
			List<Enemy> enemies = new List<Enemy>();
			Random random = new Random();
			while (amount > 0)
			{
				int levelComponentX = random.Next(0, width);
				int levelComponentY = random.Next(0, height);

				int tileMapSize = levelComponents[levelComponentY][levelComponentX].tileMap.tiles[0].Count;

				int tileX = random.Next(0, tileMapSize);
				int tileY = random.Next(0, tileMapSize);

				if(levelComponents[levelComponentY][levelComponentX].tileMap.tiles[tileX][tileY] == 0)
				{
					if (hasSpawnSpace(levelComponents[levelComponentY][levelComponentX].tileMap.tiles, tileX, tileY))
					{
						int xPos = levelComponentX * levelComponent + tileX * tileSize;
						int yPos = levelComponentY * levelComponent + tileY * tileSize;

						enemies.Add(generateEnemy(random, xPos, yPos));

						amount--;
					}
				}
			}
			return enemies;
		}

		/// <summary>
		/// Generate a random enemy
		/// </summary>
		/// <param name="random">Random index to pick from</param>
		/// <param name="xPos">Horizontal spawn position</param>
		/// <param name="yPos">Vertical spawn position</param>
		/// <returns></returns>
		private Enemy generateEnemy(Random random, int xPos, int yPos)
		{
			int randomEnemyType = random.Next(1, 3);
			if (randomEnemyType == 1)
			{
				return new MageEnemy(xPos, yPos, 18, 30, 3);
			}
			else if (randomEnemyType == 2)
			{
				return new FighterEnemy(xPos, yPos, 18, 30, 5);
			}
			return null;
		}

		/// <summary>
		/// Check if the given tile has space around it to spawn
		/// </summary>
		/// <param name="tiles">A set of tiles</param>
		/// <param name="tileX">Horizontal position</param>
		/// <param name="tileY">Vertical position</param>
		/// <returns>True if all surrounding spaces are open</returns>
		private bool hasSpawnSpace(List<List<int>> tiles, int tileX, int tileY)
		{
			if ( hasTopRowSpace(tiles, tileX, tileY) &&
				hasLeftRightSpace(tiles, tileX, tileY) &&
				hasBotRowSpace(tiles, tileX, tileY))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Check if the given tile has space at the top row
		/// </summary>
		/// <param name="tiles">A set of tiles</param>
		/// <param name="tileX">Horizontal position</param>
		/// <param name="tileY">Vertical position</param>
		/// <returns>True if top row spaces are open</returns>
		private bool hasTopRowSpace(List<List<int>> tiles, int tileX, int tileY)
		{
			bool topLeft = false;
			bool topMid = false;
			bool topRight = false;

			if (tileY - 1 > 0)
			{
				if (tileX - 1 > 0)
				{
					if (tiles[tileY - 1][tileX - 1] == 0)
					{
						topLeft = true;
					}
				}
				if (tiles[tileY - 1][tileX] == 0)
				{
					topMid = true;
				}
				if (tileX + 1 < tiles[tileY].Count)
				{
					if (tiles[tileY - 1][tileX + 1] == 0)
					{
						topRight = true;
					}
				}
			}
			return topLeft && topMid && topRight;
		}

		/// <summary>
		/// Check if the given tile has space at the left and right of it
		/// </summary>
		/// <param name="tiles">A set of tiles</param>
		/// <param name="tileX">Horizontal position</param>
		/// <param name="tileY">Vertical position</param>
		/// <returns>True if left and right spaces are open</returns>
		private bool hasLeftRightSpace(List<List<int>> tiles, int tileX, int tileY)
		{
			bool midLeft = false;
			bool midRight = false;

			if (tileX - 1 > 0)
			{
				if (tiles[tileY][tileX - 1] == 0)
				{
					midLeft = true;
				}
			}
			if (tiles[tileY][tileX + 1] == 0)
			{
				if (tiles[tileY - 1][tileX + 1] == 0)
				{
					midRight = true;
				}
			}
			return midLeft && midRight;
		}

		/// <summary>
		/// Check if the given tile has space at the bot row
		/// </summary>
		/// <param name="tiles">A set of tiles</param>
		/// <param name="tileX">Horizontal position</param>
		/// <param name="tileY">Vertical position</param>
		/// <returns>True if bot row spaces are open</returns>
		private bool hasBotRowSpace(List<List<int>> tiles, int tileX, int tileY)
		{
			bool botLeft = false;
			bool botMid = false;
			bool botRight = false;

			if (tileY + 1 < tiles.Count)
			{
				if (tileX - 1 > 0)
				{
					if (tiles[tileY + 1][tileX - 1] == 0)
					{
						botLeft = true;
					}
				}
				if (tiles[tileY + 1][tileX] == 0)
				{
					botMid = true;
				}
				if (tiles[tileY + 1][tileX + 1] == 0)
				{
					if (tiles[tileY - 1][tileX + 1] == 0)
					{
						botRight = true;
					}
				}
			}
			return botLeft && botMid && botRight;
		}
	}
}
