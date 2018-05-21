using System.Collections.Generic;
using Assets.Scripts.Movement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.TileHandling
{
    public static class OverlayGridGenerator
    {
        private static Tile GetTileImage(int tileIndex)
        {
            return Resources.Load<Tile>("TileMaps/ArrowTiles/ArrowSpriteSheet_"+tileIndex);
        }

        public static void GeneratePathGraphic(Grid overlayGrid, MovementPath movementPath)
        {
            Tile[] arrowTile = Resources.LoadAll<Tile>("TileMaps/ArrowTiles/");
            
            var tileMap = SetupTileMapObject(overlayGrid, "Arrow Overlay", 4);

            var renderer = tileMap.GetComponent<TilemapRenderer>();
            Material tileRenderMaterial = (Material)Resources.Load("Materials/TileRenderMaterial", typeof(Material));
            renderer.material = tileRenderMaterial;

            tileMap.ClearAllTiles();


            var pathEnumerator = movementPath.getPositionsInPath();
            pathEnumerator.MoveNext();
            var previousCell = pathEnumerator.Current;
            while (pathEnumerator.MoveNext())
            {
                Debug.Log(pathEnumerator.Current);
                var currentCell = pathEnumerator.Current;
                var diff = currentCell - previousCell;
                Debug.Log(diff);
                var arrowIndex = arrowMap[diff]*4;
                //var arrowTileSelected = arrowTile[arrowIndex];
                var arrowTileSelected = GetTileImage(arrowIndex);
                tileMap.SetTile(new Vector3Int(previousCell.x, previousCell.y, 0), arrowTileSelected);
                previousCell = currentCell;
            }

            tileMap.RefreshAllTiles();
        }

        private static readonly Dictionary<Vector2Int, int> arrowMap = new Dictionary<Vector2Int, int>()
        {
            { Vector2Int.right,0 },
            { Vector2Int.left, 1 },
            { Vector2Int.down, 2 },
            { Vector2Int.up,   3 },
            { Vector2Int.up+Vector2Int.left,    4 },
            { Vector2Int.down+Vector2Int.left,  5 },
            { Vector2Int.up+Vector2Int.right,   6 },
            { Vector2Int.down+Vector2Int.right, 7 },
        };

        static Tilemap SetupTileMapObject(Grid parent, string tileMapName, int level)
        {
            GameObject tileMapObject = GameObject.Find(tileMapName);
            if (tileMapObject == null)
            {
                tileMapObject = new GameObject(tileMapName);
                tileMapObject.transform.SetParent(parent.transform);
            }

            tileMapObject.transform.SetPositionAndRotation(Vector3.back * level, Quaternion.identity);

            var tileMap = tileMapObject.GetComponent<Tilemap>();
            if (tileMap == null)
            {
                tileMap = tileMapObject.AddComponent<Tilemap>();
                tileMapObject.AddComponent<TilemapRenderer>();
            }

            return tileMap;
        }

        public static void GenerateBlockedCellGraphic(Grid overlayGrid, Grid gameWorldGrid)
        {
            var blockTile = Resources.Load<Tile>("Materials/TileMaps/Tiles/Grassland@128x128_17");
            var tileMap = SetupTileMapObject(overlayGrid, "Blocked Cell Overlay", 3);
            tileMap.ClearAllTiles();
            //get max bounds of the game world
            var currentBounds = Tilemapper.GetMaxBoundsOfGrid(gameWorldGrid);
            //set color of blockable layer
            var color = Color.red;
            color.a = 0.5f;
            var renderer = tileMap.GetComponent<TilemapRenderer>();
            Material tileRenderMaterial = (Material)Resources.Load("Materials/TileRenderMaterial", typeof(Material));
            renderer.material = tileRenderMaterial;
            renderer.material.color = color;

            for (var x = currentBounds.xMin - 1; x < currentBounds.xMax + 1; x++)
            {
                for (var y = currentBounds.yMin - 1; y < currentBounds.yMax + 1; y++)
                {
                    if (!Tilemapper.IsGridPassableAtCoordinate(gameWorldGrid, new Vector2Int(x, y)))
                    {
                        tileMap.SetTile(new Vector3Int(x, y, 0), blockTile);
                    }
                }
            }

        }

        public static void GenerateGridCoordinateText(Grid overlayGrid, Grid gameWorldGrid)
        {
            var currentBounds = Tilemapper.GetMaxBoundsOfGrid(gameWorldGrid);
            for (var x = currentBounds.xMin - 1; x < currentBounds.xMax + 1; x++)
            {
                for (var y = currentBounds.yMin - 1; y < currentBounds.yMax + 1; y++)
                {
                    var local = overlayGrid.CellToLocalInterpolated(new Vector3Int(x, y, 0));
                    local.z = -7;
                    local.y += 1;
                    var textGameObject = new GameObject("coord_text_" + x + "_" + y);
                    textGameObject.transform.SetParent(overlayGrid.transform);
                    var textMesh = textGameObject.AddComponent<TextMesh>();
                    textMesh.text = x + "," + y;
                    Font textMeshFont = textMesh.font;
                    textMesh.characterSize = 0.5f;
                    textGameObject.transform.SetPositionAndRotation(local, Quaternion.identity);
                }
            }
        }
    }
}