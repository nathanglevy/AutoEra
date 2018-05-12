using System.Collections;
using System.Runtime.Serialization;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;

public class Tilemapper
{
    public static bool isGridPassableAtCoordinate(Grid gameWorldGrid, Vector2Int position)
    {
        bool foundPassible = false;
        for (var i = 0; i < gameWorldGrid.transform.childCount; i++) {
            Tilemap child = gameWorldGrid.transform.GetChild(i).GetComponent<Tilemap>();
            child.CompressBounds();
            TileBase tile = child.GetTile(new Vector3Int(position.x,position.y,0));
            if (tile == null)
                continue;
            BlockingTile convertedTile = tile as BlockingTile;
            
            if ((convertedTile != null) && (convertedTile.isImpassible)) {
                Debug.Log("Tile is impassible: " + convertedTile.isImpassible);
                return false;
            } else {
                foundPassible = true;
            }
        }
        return foundPassible;
    }
}