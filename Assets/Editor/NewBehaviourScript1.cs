using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.SerializableGame;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class MyWindow : EditorWindow
{
    string worldNameSave = "exampleWorld.json";
    string worldNameLoad = "exampleWorld.json";
    string saveRoot;

    bool groupEnabled;
//    bool myBool = true;
//    float myFloat = 1.23f;
    private static Object source;

    //    private Object source;
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/WorldSaver")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MyWindow window = (MyWindow)EditorWindow.GetWindow(typeof(MyWindow));
        window.Show();
        setDefaultGameWorldEditor();

    }

    static void setDefaultGameWorldEditor()
    {
        var gameWorldEditor = GameObject.Find("GameWorldEditor");
        if (gameWorldEditor != null) {
            source = gameWorldEditor;
            Debug.Log("Found it");
        }
    }

    void OnEnable()
    {
        saveRoot = Application.dataPath + "/StreamingAssets/";
    }

    void OnGUI()
    {
        GUILayout.Label("General Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("GameWorldGrid");
        source = EditorGUILayout.ObjectField(source, typeof(GameObject), true);
        var defaultButton = GUILayout.Button("default");
        EditorGUILayout.EndHorizontal();
        if (defaultButton)
        {
            setDefaultGameWorldEditor();
        }

        saveRoot = EditorGUILayout.TextField("Root folder", saveRoot);

        GUILayout.Label("Save Game Map", EditorStyles.boldLabel);
        worldNameSave = EditorGUILayout.TextField("Name of map to save", worldNameSave);
        
        var saveButton = GUILayout.Button("SAVE");
        if (saveButton)
        {
            Debug.Log("Trying to save the game");
            var gameWorldGrid = source as GameObject;
            Directory.CreateDirectory(saveRoot);
            var saveFileName = saveRoot + "/" + worldNameSave + ".json";
            saveGameWorld(saveFileName, gameWorldGrid);
        }
        GUILayout.Label("Load Game Map", EditorStyles.boldLabel);
        worldNameLoad = EditorGUILayout.TextField("Name of map to load", worldNameLoad);
        var loadButton = GUILayout.Button("LOAD");
        if (loadButton) {
            Debug.Log("Trying to load the game");
            var gameWorldGrid = source as GameObject;
            var loadFileName = saveRoot + "/" + worldNameLoad + ".json";
            loadGameWorld(loadFileName, gameWorldGrid);
        }

        //use this to control 'overwrite?'
        //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        //myBool = EditorGUILayout.Toggle("Toggle", myBool);
        //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        //EditorGUILayout.EndToggleGroup();
    }

    void saveGameWorld(string pathToSaveTo, GameObject gameWorldGrid)
    {
        Dictionary<string, Tilemap> tileMaps = new Dictionary<string, Tilemap>();

        for (var i = 0; i < gameWorldGrid.transform.childCount; i++) {
            var child = gameWorldGrid.transform.GetChild(i).GetComponent<Tilemap>();
            tileMaps.Add(child.name, child);
        }

        var gameWorld = new GameMap(tileMaps);
        var tilesJson = JsonConvert.SerializeObject(gameWorld);
        Debug.Log(pathToSaveTo);
        File.WriteAllText(pathToSaveTo, tilesJson);
    }

    void loadGameWorld(string pathToLoadFrom, GameObject gameWorldGrid)
    {
//        Dictionary<string, Tilemap> tileMaps = new Dictionary<string, Tilemap>();
        var loadedJson = File.ReadAllText(pathToLoadFrom);
        var gameWorld = JsonConvert.DeserializeObject<GameMap>(loadedJson);
        convertGameMapToTileMaps(gameWorld.tileLayerDictionary);
    }

    void convertGameMapToTileMaps(Dictionary<string, List<TileObject>> gameWorldTileLayerDictionary)
    {
        foreach (var keyValue in gameWorldTileLayerDictionary)
        {
            var layerName = keyValue.Key;
            var tileList = keyValue.Value;
            var vectorArray = tileList.Select(i => new Vector3Int(i.x, i.y, 0)).ToArray();
            var tileArray = tileList.Select(i => createTile(i.tileName)).ToArray();
            var newGameObject = new GameObject(layerName+"_new");
            var tileMap = newGameObject.AddComponent<Tilemap>();
            
            newGameObject.AddComponent<TilemapRenderer>();
            var parent = source as GameObject;
            if (parent != null)
                newGameObject.transform.SetParent(parent.transform);
            else
                Debug.Log("parent was null");
            tileMap.SetTiles(vectorArray,tileArray);
            tileMap.RefreshAllTiles();
        }
    }

    TileBase createTile(string tileName)
    {

        //TODO -- figure out how to load a tile
        //return new Tile();
        //var newTile = ScriptableObject.CreateInstance<Tile>();
        //newTile.sprite = Resources.Load("2D Hand Painted - Grassland Tileset/Tilemaps/" + tileName) as Sprite;
        //return newTile;
        //        Tile tile = ScriptableObject.CreateInstance<Tile>();
        //        tile.sprite = shit;
        //        Debug.Log(tileName);
        //        Debug.Log(Application.dataPath);
        //        Debug.Log(File.Exists(Application.dataPath+ "2D Hand Painted - Grassland Tileset/Tilemaps/" + tileName));
        //        shit = AssetDatabase.LoadAssetAtPath<Sprite>(Application.dataPath + "2D Hand Painted - Grassland Tileset/Tilemaps/" + tileName + ".asset");
        Tile tile = Resources.Load<Tile>("Tilemaps/Tiles/"+tileName);


        //        MonoBehaviour t = (MonoBehaviour)AssetDatabase.LoadAssetAtPath("Assests/2D Hand Painted - Grassland Tileset/Tilemaps/" + tileName, typeof(MonoBehaviour));
        //        Debug.Log("HERE");
        return tile;
        //        ScriptableObject objPrefab = Resources.Load("2D Hand Painted - Grassland Tileset/Tilemaps/" + tileName) as ScriptableObject;
        //        //GameObject obj = Instantiate(objPrefab) as GameObject;
        //        Tile tileBase = ScriptableObject.CreateInstance<Tile>();
        //        tileBase.gameObject = obj;
        //        return tileBase;
    }



}