using Microsoft.Xna.Framework;
using Newtonsoft;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class TileMapLoader
    {
        public List<TileMap> tileMaps { get; set; }
        public TileMapLoader()
        {
            tileMaps = new List<TileMap>();
        }

        /// <summary>
        /// Set the tileMaps from the JSON file tileMaps.json
        /// </summary>
        public void setTileMapsFromJson()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"../../../Content", "tileMaps.json");
			using StreamReader streamReader = new StreamReader(path);
			string json = streamReader.ReadToEnd();
			this.tileMaps = JsonConvert.DeserializeObject<List<TileMap>>(json);
		}

        /// <summary>
        /// Get a tilemap for a levelcomponent
        /// </summary>
        /// <param name="entrance">The entrance of the levelcomponent</param>
        /// <param name="exit">The exit of the levelcomponent</param>
        /// <returns>A tilemap fitting the levelcomponent</returns>
        public TileMap getTileMap(Directions entrance, Directions exit)
        {
            Random random = new Random();
            List<TileMap> fittingTileMaps = getFittingTileMaps(entrance, exit);
            if (fittingTileMaps.Count != 0)
            {
                return fittingTileMaps[random.Next(0, fittingTileMaps.Count)];
            }
            return null;
        }

        /// <summary>
        /// Get a list of fitting tilemaps
        /// </summary>
        /// <param name="entrance">The entrance of the levelcomponent</param>
        /// <param name="exit">The exit of the levelcomponent</param>
        /// <returns>A list of tilemaps fitting the levelcomponent openings</returns>
        private List<TileMap> getFittingTileMaps(Directions entrance, Directions exit)
		{
            List<TileMap> fittingTileMaps = new List<TileMap>();
            foreach (TileMap tileMap in this.tileMaps)
            {
                if ((entrance == tileMap.openingOne || entrance == tileMap.openingTwo) && (exit == tileMap.openingOne || exit == tileMap.openingTwo) && (entrance != exit))
                {
                    fittingTileMaps.Add(tileMap);
                }
            }
            if(fittingTileMaps.Count == 0)
			{
                fittingTileMaps.Add(this.tileMaps[0]);
			}
            return fittingTileMaps;
        }
    }
}
