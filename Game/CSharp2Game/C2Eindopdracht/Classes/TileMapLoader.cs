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
            
            using (StreamReader r = new StreamReader(@"c:/tileMaps.json"))
            {
                string json = r.ReadToEnd();
                this.tileMaps = JsonConvert.DeserializeObject<List<TileMap>>(json);
            }
        }

        public TileMap getTileMap(Directions entrance, Directions exit)
        {
            foreach (TileMap tileMap in this.tileMaps)
            {
                if ((entrance == tileMap.openingOne || entrance == tileMap.openingTwo) && (exit == tileMap.openingOne || exit == tileMap.openingTwo))
                {
                    return tileMap;
                }
            }
            return null;
        }
    }
}
