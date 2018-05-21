using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.TileHandling
{
    public class Tilemapper
    {
    

        public static bool IsGridPassableAtCoordinate(Grid gameWorldGrid, Vector2Int position)
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

        public static BoundsInt GetMaxBoundsOfGrid(Grid grid)
        {
            var currentBounds = new BoundsInt();

            for (var i = 0; i < grid.transform.childCount; i++)
            {
                Tilemap child = grid.transform.GetChild(i).GetComponent<Tilemap>();
                child.CompressBounds();
                currentBounds.xMax = (child.cellBounds.xMax > currentBounds.xMax)
                    ? child.cellBounds.xMax
                    : currentBounds.xMax;
                currentBounds.yMax = (child.cellBounds.yMax > currentBounds.yMax)
                    ? child.cellBounds.yMax
                    : currentBounds.yMax;
                currentBounds.xMin = (child.cellBounds.xMin < currentBounds.xMin)
                    ? child.cellBounds.xMin
                    : currentBounds.xMin;
                currentBounds.yMin = (child.cellBounds.yMin < currentBounds.yMin)
                    ? child.cellBounds.yMin
                    : currentBounds.yMin;
            }

            return currentBounds;
        }
    }
}