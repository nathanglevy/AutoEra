using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tests : MonoBehaviour
{

    public Tile[] allTiles;

    public Grid gameWorldGrid;
    public Grid overlayGrid;
    public Grid itemGrid;
    public Vector2Int source;
    public Vector2Int target;

    public Tile[] arrowTile = new Tile[8];
    public Tile blockTile;
    
	// Use this for initialization
	void Start () {
	    for (var i = 0; i < gameWorldGrid.transform.childCount; i++)
	    {
	        Tilemap child = gameWorldGrid.transform.GetChild(i).GetComponent<Tilemap>();


//	        var color = Color.white;
//	        var renderer = child.GetComponent<TilemapRenderer>();
//	        Material tileRenderMaterial = (Material)Resources.Load("Materials/TileRenderMaterial", typeof(Material));
//	        renderer.material = tileRenderMaterial;
//	        renderer.material.color = color;

            child.CompressBounds();
	        Debug.Log(child.cellBounds);
	    }

        //testing ASTAR
	    AStarCalculator aStarCalculator = new AStarCalculator(gameWorldGrid);
	    var result = aStarCalculator.FindShortestPath(source, new List<Vector2Int> { target }, null);
//	    var resultPath = result.getPositionsInPath();
        GeneratePathGraphic(overlayGrid, result);
	    GenerateBlockedCellGraphic(overlayGrid,gameWorldGrid);
        GenerateGridCoordinateText(overlayGrid,gameWorldGrid);
//        EnumerateAllItems(itemGrid);
//	    while (resultPath.MoveNext()) 
//	    {
//	        Debug.Log(resultPath.Current);
//        }





	    //	    allTiles = Resources.LoadAll<Tile>("Tilemaps/Tiles/");
	    //        foreach (Tile tile in allTiles)
	    //        {
	    //            BlockingTile newTile = ScriptableObject.CreateInstance<BlockingTile>();
	    //            newTile.sprite = tile.sprite;
	    //            newTile.name = tile.name;
	    //            
	    //            AssetDatabase.CreateAsset(newTile, "Assets/Resources/Tilemaps/BlockableTiles/"+tile.name+".asset");
	    //            Debug.Log(AssetDatabase.GetAssetPath(tile));
	    //        }

	    // Print the path of the created asset

	    //print total amount of tiles which are blocked and which are not
	    //	    for (var i = 0; i < gameWorldGrid.transform.childCount; i++) {
	    //	        Tilemap child = gameWorldGrid.transform.GetChild(i).GetComponent<Tilemap>();
	    //            child.CompressBounds();
	    //	        TileBase[] tiles = child.GetTilesBlock(child.cellBounds);
	    //            foreach (TileBase tile in tiles)
	    //            {
	    //                BlockingTile convertedTile = tile as BlockingTile;
	    //                if (convertedTile != null)
	    //                {
	    //                    Debug.Log("Found a blocking tile");
	    //                    Debug.Log("Tile is impassible: " + convertedTile.isImpassible);
	    //                }
	    //                else
	    //                {
	    //                    Debug.Log("Non blocking tile");
	    //                }
	    //            }
	    //            
	    //	    }
	}

    // Update is called once per frame
    void Update () {
		
	}

    Tilemap SetupTileMapObject(Grid parent, string tileMapName, int level)
    {
        GameObject tileMapObject = GameObject.Find(tileMapName);
        if (tileMapObject == null) {
            tileMapObject = new GameObject(tileMapName);
            tileMapObject.transform.SetParent(parent.transform);
        }

        tileMapObject.transform.SetPositionAndRotation(Vector3.back * level, Quaternion.identity);

        var tileMap = tileMapObject.GetComponent<Tilemap>();
        if (tileMap == null) {
            tileMap = tileMapObject.AddComponent<Tilemap>();
            tileMapObject.AddComponent<TilemapRenderer>();
        }

        return tileMap;
    }

    BoundsInt GetMaxBoundsOfGrid(Grid grid)
    {
        var currentBounds = new BoundsInt();

        for (var i = 0; i < grid.transform.childCount; i++) {
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

    void GenerateBlockedCellGraphic(Grid overlayGrid, Grid gameWorldGrid)
    {
        var tileMap = SetupTileMapObject(overlayGrid, "Blocked Cell Overlay", 3);
        tileMap.ClearAllTiles();
        //get max bounds of the game world
        var currentBounds = GetMaxBoundsOfGrid(gameWorldGrid);
        //set color of blockable layer
        var color = Color.red;
        color.a = 0.5f;
        var renderer = tileMap.GetComponent<TilemapRenderer>();
        Material tileRenderMaterial = (Material)Resources.Load("Materials/TileRenderMaterial", typeof(Material));
        renderer.material = tileRenderMaterial;
        renderer.material.color = color;

        for (var x = currentBounds.xMin-1; x < currentBounds.xMax+1; x++)
        {
            for (var y = currentBounds.yMin-1; y < currentBounds.yMax+1; y++) {
                if (!Tilemapper.isGridPassableAtCoordinate(gameWorldGrid, new Vector2Int(x, y)))
                {
                   
                    tileMap.SetTile(new Vector3Int(x, y, 0), blockTile);
                }
            }
        }
        
    }

    void GenerateGridCoordinateText(Grid overlayGrid, Grid gameWorldGrid)
    {
        var currentBounds = GetMaxBoundsOfGrid(gameWorldGrid);
        for (var x = currentBounds.xMin - 1; x < currentBounds.xMax + 1; x++) {
            for (var y = currentBounds.yMin - 1; y < currentBounds.yMax + 1; y++) {
                var local = overlayGrid.CellToLocalInterpolated(new Vector3Int(x, y, 0));
                local.z = -7;
                local.y += 1;
                var textGameObject = new GameObject("coord_text_"+x+"_"+y);
                textGameObject.transform.SetParent(overlayGrid.transform);
                var textMesh = textGameObject.AddComponent<TextMesh>();
                textMesh.text = x + "," + y;
                Font textMeshFont = textMesh.font;
                textMesh.characterSize = 0.5f;
                textGameObject.AddComponent<MeshRenderer>();
                textGameObject.transform.SetPositionAndRotation(local, Quaternion.identity);
            }
        }



    }

    private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
    {
        int childCount = parent.childCount;
        Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
        Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
        Bounds bounds = new Bounds((max + min) * .5f, max - min);

        for (int i = 0; i < childCount; i++) {
            Transform child = parent.GetChild(i);
            if (bounds.Contains(child.position))
                return child;
        }
        return null;
    }

    void EnumerateAllItems(Grid itemGrid)
    {
        var childCount = itemGrid.transform.childCount;
        for (int i = 0; i < childCount; i++) {
            Transform child = itemGrid.transform.GetChild(i);
            TextMesh textMesh = child.GetComponentInChildren<TextMesh>();
            Debug.Log("Item with: " + textMesh.text);
        }
    }

    void GeneratePathGraphic(Grid overlayGrid, MovementPath movementPath)
    {
        var tileMap = SetupTileMapObject(overlayGrid, "Arrow Overlay", 4);

        var renderer = tileMap.GetComponent<TilemapRenderer>();
        Material tileRenderMaterial = (Material)Resources.Load("Materials/TileRenderMaterial", typeof(Material));
        renderer.material = tileRenderMaterial;

        tileMap.ClearAllTiles();


        var pathEnumerator = movementPath.getPositionsInPath();
        pathEnumerator.MoveNext();
        var previousCell = pathEnumerator.Current;
        while (pathEnumerator.MoveNext()) {
            Debug.Log(pathEnumerator.Current);
            var currentCell = pathEnumerator.Current;
            var diff = currentCell - previousCell;
            Debug.Log(diff);
            var arrowIndex = arrowMap[diff];
            var arrowTileSelected = arrowTile[arrowIndex];
            tileMap.SetTile(new Vector3Int(previousCell.x, previousCell.y, 0), arrowTileSelected);
            previousCell = currentCell;
        }

        tileMap.RefreshAllTiles();
    }

    public Dictionary<Vector2Int, int> arrowMap = new Dictionary<Vector2Int, int>()
    {
        { Vector2Int.right,0 },
        { Vector2Int.left, 1 },
        { Vector2Int.down, 2 },
        { Vector2Int.up,   3 },
        { Vector2Int.up+Vector2Int.left, 4 },
        { Vector2Int.down+Vector2Int.left, 5 },
        { Vector2Int.up+Vector2Int.right, 6 },
        { Vector2Int.down+Vector2Int.right, 7 },
    };
}