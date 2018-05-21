using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.SerializableGame
{
    [Serializable]
    public class GameMap
    {
        public GameMap() { }
        public GameMap(Dictionary<string, Tilemap> tileMaps)
        {
            foreach (var tileMapEntry in tileMaps)
            {
                var tileMap = tileMapEntry.Value;
                var tileMapName = tileMapEntry.Key;
                var baseTileList = new List<TileObject>();
                tileMap.CompressBounds();
                BoundsInt tileMapBound = tileMap.cellBounds;
                TileBase[] tilesInBound = tileMap.GetTilesBlock(tileMapBound);
                for (int x = 0; x < tileMapBound.size.x; x++)  {
                    for (int y = 0; y < tileMapBound.size.y; y++) {
                        TileBase tile = tilesInBound[x + y * tileMapBound.size.x];
                        if (tile != null) {
                            //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                            baseTileList.Add(new TileObject(x, y, tile.name));
                        }
                    }
                }
                tileLayerDictionary.Add(tileMapName,baseTileList);
            }
        
//        this.mapBounds = tileMapBound;
        }
        //    [SerializeField]
//    public BoundsInt mapBounds;
        public Dictionary<string, List<TileObject>> tileLayerDictionary = new Dictionary<string, List<TileObject>>();
    }
}