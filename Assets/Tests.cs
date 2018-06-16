using System.Collections.Generic;
using Assets.Scripts.GameWorld;
using Assets.Scripts.Movement;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tests : MonoBehaviour
{

    public Tile[] allTiles;

    public Grid gameWorldGrid;
    public Grid overlayGrid;
    public Grid itemGrid;
    public GameWorld gameWorld;

    public Character character;

    
    
	// Use this for initialization
	void Start () {
	    for (var i = 0; i < gameWorldGrid.transform.childCount; i++)
	    {
	        Tilemap child = gameWorldGrid.transform.GetChild(i).GetComponent<Tilemap>();
            child.CompressBounds();
	        Debug.Log(child.cellBounds);
	    }
        SetNewMovementPath(character);

//	    gameWorld.AddNewCharacterToLocation(new Vector3Int(-5, 0, 0));
	    Character character2 = gameWorld.AddNewCharacterToLocation(new Vector3Int(-5, -2, 0));
	    var item = gameWorld.GetItemAtLocation(new Vector3Int(-2, -1, 0));
        item.AssignAmount(160);
        item.RemoveAssignedAmount(160);
        
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


    private void SetNewMovementPath(Character character)
    {
        var startLocation = this.character.GetLocation().ToVec2();
        var targetLocation = new Vector2Int(2,1);
        AStarCalculator aStarCalculator = new AStarCalculator(gameWorldGrid);
        var result = aStarCalculator.FindShortestPath(startLocation, new List<Vector2Int> { targetLocation }, null);
        character.SetMovementPath(result);
        gameWorld.DisplayPathOverlay(result);
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

    
}