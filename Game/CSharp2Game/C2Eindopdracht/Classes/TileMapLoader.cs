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
        public void setTileMapsFromJson()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"../../../Content", "tileMaps.json");
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                this.tileMaps = JsonConvert.DeserializeObject<List<TileMap>>(json);
            }
        }

        public TileMap getTileMap(Directions entrance, Directions exit)
        {
            Random random = new Random();
            List<TileMap> fittingTileMaps = getFittingTileMaps(entrance, exit);
            if (fittingTileMaps.Count != 0)
            {
                return fittingTileMaps[random.Next(1, fittingTileMaps.Count + 1) - 1];
            }
            return null;
        }

        private List<TileMap> getFittingTileMaps(Directions entrance, Directions exit)
		{
            List<TileMap> fittingTileMaps = new List<TileMap>();
            foreach (TileMap tileMap in this.tileMaps)
            {
                if ((entrance == tileMap.openingOne || entrance == tileMap.openingTwo) && (exit == tileMap.openingOne || exit == tileMap.openingTwo))
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
