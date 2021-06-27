using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class EnemySpawner
	{
		public int amount { get; set; }

		/// <summary>
		/// Sets amount of enemies
		/// </summary>
		/// <param name="amount">Amount of enemies</param>
		public EnemySpawner(int amount)
		{
			this.amount = amount;
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
			while (true)
			{
				int levelComponentX = random.Next(0, width);
				int levelComponentY = random.Next(0, height);

				int tileMapSize = levelComponents[levelComponentY][levelComponentX].tileMap.tiles[0].Count;

				int tileX = random.Next(0, tileMapSize);
				int tileY = random.Next(0, tileMapSize);

				if (levelComponents[levelComponentY][levelComponentX].tileMap.tiles[tileY][tileX] == 0)
				{
					int xPos = levelComponentX * levelComponent + tileX * tileSize;
					int yPos = levelComponentY * levelComponent + tileY * tileSize;

					int randomEnemyType = random.Next(1, 3);
					if(randomEnemyType == 1)
					{
						enemies.Add(new MageEnemy(xPos, yPos, 18, 30));
					}
					else if(randomEnemyType == 2)
					{
						enemies.Add(new FighterEnemy(xPos, yPos + tileY, 18, 30));
					}
					amount--;
				}
				if (amount == 0)
				{
					break;
				}
			}
			return enemies;
		}
	}
}
