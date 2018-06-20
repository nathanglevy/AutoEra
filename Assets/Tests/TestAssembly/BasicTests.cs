using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameWorld;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class TestClass
{
    [UnityTest]
    public IEnumerator MonoBehaviourTest_Works_TEST()
    {
        var gameWorldPrefab = Resources.Load("Tests/GameWorldTop");
        Debug.Log("gameworld is: " + gameWorldPrefab);
        var prefabbed = (GameObject)PrefabUtility.InstantiatePrefab(gameWorldPrefab);
        yield return null;
    }

    [UnityTest]
    public IEnumerator MonoBehaviourTest_TestGetAllItemsInBoundary()
    {
        var gameWorldPrefab = Resources.Load("Tests/GameWorldTop");
        var prefabbed = (GameObject)PrefabUtility.InstantiatePrefab(gameWorldPrefab);
        var gameWorld = prefabbed.GetComponent<GameWorld>();
        yield return null;
        var item = gameWorld.AddNewItemToLocation(new Vector3Int(3, 3, 0), ItemType.Wood, 10);
        var worldBoundary = gameWorld.GetWorldBoundary();
        Debug.Log(worldBoundary);
        var items = gameWorld.GetAllItemsInBoundary(gameWorld.GetWorldBoundary());
        var itemLocations = items.Select(it => it.Location);
        if (!itemLocations.Contains(new Vector3Int(3,3,0)))
            Debug.LogError("Could not find the object!");
        var itemSpecific = items.First(it => it.Location == new Vector3Int(3,3,0));
        if (item != itemSpecific)
            Debug.LogError("Found the wrong object!");

        //        UnityEngine.TestTools.LogAssert.Expect();
    }
}