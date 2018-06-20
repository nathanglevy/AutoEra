using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Movement;
using Assets.Scripts.TileHandling;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.GameWorld
{
    public class GameWorld : MonoBehaviour, IGameWorld
    {
        [SerializeField]
        private Grid gameWorldBaseGrid;
        [SerializeField]
        private Grid overlayGrid;
        [SerializeField]
        private Grid itemGrid;
        [SerializeField]
        private Grid characterGrid;
        [SerializeField]
        private GameObject itemPrefab;
        [SerializeField]
        private GameObject characterPrefab;
        private AStarCalculator pathCalculator;

        public AStarCalculator PathCalculator
        {
            get { return pathCalculator; }
        }

        public void DisplayBlockedOverlay()
        {
            OverlayGridGenerator.GenerateBlockedCellGraphic(overlayGrid,gameWorldBaseGrid);
        }

        public void DisplayPathOverlay(MovementPath path)
        {
            OverlayGridGenerator.GeneratePathGraphic(overlayGrid, path);
        }

        public void DisplayCoordinates()
        {
            OverlayGridGenerator.GenerateGridCoordinateText(overlayGrid,gameWorldBaseGrid);
        }

        public void ClearOverlay()
        {
            OverlayGridGenerator.ClearOverlay(overlayGrid);
        }

        public List<ItemObject> GetAllItemsInBoundary(BoundsInt boundary)
        {
            var allItems = new List<ItemObject>(itemGrid.transform.GetComponentsInChildren<ItemObject>());
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
                //TODO -- add an assertion that there really is JUST ONE.
                return itemList[0];
            }
        }

        public Character AddNewCharacterToLocation(Vector3Int location)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(characterPrefab);
            instance.transform.SetParent(characterGrid.transform);
            Character character = instance.GetComponent<Character>();
            character.SetPosition(location);
            return character;
        }

        public List<Character> GetCharactersAtLocation(Vector3Int location)
        {
            return GetCharactersAtBoundary(new BoundsInt(location, Vector3Int.one));
        }

        public List<Character> GetCharactersAtBoundary(BoundsInt boundary)
        {
            var allCharacters = new List<Character>(characterGrid.GetComponentsInChildren<Character>());
            var allCharactersInBoundary = allCharacters.Where(it => boundary.Contains(it.GetLocation()));
            return allCharactersInBoundary.ToList();
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
            {
                //will throw exception if not of same type
                itemAtCurrentLocation.AddToCurrentAmount(itemType,amount);
                return itemAtCurrentLocation;
            }

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(itemPrefab);
            var itemObject = instance.GetComponentInChildren<ItemObject>();
            itemObject.AddToCurrentAmount(itemType,amount);
            itemObject.ItemType = itemType;
            instance.transform.SetParent(itemGrid.transform);
            instance.transform.position = itemGrid.LocalToWorld(itemGrid.CellToLocalInterpolated(location + new Vector3(.5f, .5f, .5f)));
            return itemObject;
        }

        void Awake()
        {
            pathCalculator = new AStarCalculator(gameWorldBaseGrid);
        }
        // Use this for initialization
        void Start()
        {
            DisplayCoordinates();
        }

        // Update is called once per frame
        void Update()
        {
        }


    }

    interface IGameWorld
    {
        void DisplayBlockedOverlay();
        void DisplayPathOverlay(MovementPath path);
        void DisplayCoordinates();
        List<ItemObject> GetAllItemsInBoundary(BoundsInt boundary);
        List<ItemObject> GetAllItemsInBoundary(BoundsInt boundary, ItemType itemType);
        ItemObject GetItemAtLocation(Vector3Int location);
        ItemObject AddNewItemToLocation(Vector3Int location, ItemType itemType, int amount);
        Character AddNewCharacterToLocation(Vector3Int location);
        List<Character> GetCharactersAtLocation(Vector3Int location);


        List<Character> GetCharactersAtBoundary(BoundsInt boundary);
        bool IsTilePassable(Vector2Int tileCoords);
        BoundsInt GetWorldBoundary();
        void ClearOverlay();
    }
}