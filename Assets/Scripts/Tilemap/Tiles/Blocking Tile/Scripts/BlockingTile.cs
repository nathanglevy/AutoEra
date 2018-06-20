using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Tilemaps;

[Serializable]
public class BlockingTile : Tile
{
    [SerializeField]
    public bool isImpassible = false;
    public int passingCost = 1;


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Blocking Tile")]
    public static void CreateBlockingTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Blocking Tile", "New Blocking Tile", "asset", "Save Blocking Tile", "Assets");
        if (path == "")
            return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BlockingTile>(), path);

    }

    [CanEditMultipleObjects]
    [CustomEditor(typeof(BlockingTile))]
    public class BlockingTileEditor : Editor
    {

        private BlockingTile tile { get { return (target as BlockingTile); }}
//        public override void OnInspectorGUI()
//        {
//            EditorGUI.BeginChangeCheck();
//
//            tile.sprite = (Sprite) EditorGUILayout.ObjectField("Preview ",
//                tile.sprite, typeof(Sprite), false, null);
//            //            EditorGUI.DrawPreviewTexture(new Rect(25, 60, 100, 100), tile.sprite.texture);
//            tile.isPassable = EditorGUILayout.Toggle("Is Passable", tile.isPassable, new GUILayoutOption[0]);
//            if (EditorGUI.EndChangeCheck())
//                EditorUtility.SetDirty(tile);
//        }

//        public override void OnPreviewGUI(Rect rect, GUIStyle backgroundStyle)
//        {
//            if (Event.current.type == EventType.Repaint) {
//                //GUI.DrawTexture(rect, (Texture) serializedObject.targetObject , ScaleMode.StretchToFill, true);
//            }
//        }

        public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
        {
            //            return (Texture2D)serializedObject.targetObject
            //            Texture2D newTexture2D = new Texture2D();
            //            newTexture2D.LoadImage()
            //            var spriteSheet = Resources.Load<Texture2D>("Tilemaps/SpriteSheet/");
            
            var texture = Resources.Load<Texture2D>("Tilemaps/SpriteSheet/Grassland@128x128");
            
            var pixels = texture.GetPixels((int)(tile.sprite.textureRect.x), (int)tile.sprite.textureRect.y, (int)tile.sprite.textureRect.width, (int)tile.sprite.textureRect.height);
            var texture2D = new Texture2D((int)tile.sprite.textureRect.width, (int)tile.sprite.textureRect.height);
            texture2D.SetPixels(pixels);
            texture2D.Apply();
            return texture2D;

        }
    }



#endif
}
