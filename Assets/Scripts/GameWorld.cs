using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEditor;

public class GameWorld : MonoBehaviour, IGameWorld
{
    [SerializeField]
    private Grid gameWorldBaseGrid;
    [SerializeField]
    private Grid overlayGrid;
    [SerializeField]
    private Grid itemGrid;
    [SerializeField]
    private GameObject itemPrefab;
    public AStarCalculator pathCalculator;

    public List<ItemObject> GetAllItemsInBoundary(BoundsInt boundary)
    {
        var allItems = new List<ItemObject>(itemGrid.GetComponentsInChildren<ItemObject>());
        var allItemsInBoundary = allItems.Where(it => boundary.Contains(it.GetLocation()));
        return allItemsInBoundary.ToList();
    }

    public List<ItemObject> GetAllItemsInBoundary(BoundsInt boundary, ItemType itemType)
    {
        var allItemsInBoundary = GetAllItemsInBoundary(boundary);
        var filteredItems = allItemsInBoundary.Where(it => it.ItemType == itemType);
        return filteredItems.ToList();
    }

    public ItemObject GetItemAtLocation(Vector3Int location)
    {
        var itemList = GetAllItemsInBoundary(new BoundsInt(location, Vector3Int.one));
        if (itemList.Count == 0)
        {
            return null;
        }
        else
        {
            return itemList[0];
        }
    }

    public bool IsTilePassable(Vector2Int tileCoords)
    {
        return Tilemapper.IsGridPassableAtCoordinate(gameWorldBaseGrid, tileCoords);
    }

    public BoundsInt GetWorldBoundary()
    {
        return Tilemapper.GetMaxBoundsOfGrid(gameWorldBaseGrid);
    }

    public ItemObject AddNewItemToLocation(Vector3Int location, ItemType itemType, int amount)
    {
        ItemObject itemAtCurrentLocation = GetItemAtLocation(location);
        if (itemAtCurrentLocation != null)
            return itemAtCurrentLocation;

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(itemPrefab);
//        TextMesh textMesh = instance.GetComponentInChildren<TextMesh>();
//        textMesh.text = amount.ToString();
        var itemObject = instance.GetComponentInChildren<ItemObject>();
        itemObject.AddAmount(amount);
        itemObject.ItemType = itemType;
        instance.transform.SetParent(itemGrid.transform);
        instance.transform.position = itemGrid.LocalToWorld(itemGrid.CellToLocalInterpolated(location + new Vector3(.5f, .5f, .5f)));
        return itemObject;
    }

    // Use this for initialization
    void Start()
    {
        pathCalculator = new AStarCalculator(gameWorldBaseGrid);
    }

    // Update is called once per frame
    void Update()
    {

    }


}

interface IGameWorld
{
    List<ItemObject> GetAllItemsInBoundary(BoundsInt boundary);
    List<ItemObject> GetAllItemsInBoundary(BoundsInt boundary, ItemType itemType);
    ItemObject AddNewItemToLocation(Vector3Int location, ItemType itemType, int amount);
    bool IsTilePassable(Vector2Int tileCoords);
    BoundsInt GetWorldBoundary();
    ItemObject GetItemAtLocation(Vector3Int location);
}