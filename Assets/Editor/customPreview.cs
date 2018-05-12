using UnityEngine;
using UnityEditor;

class TextureImporterExample : AssetPostprocessor
{
    //void OnPreprocessTexture()
    //{
    //    TextureImporter importer = assetImporter as TextureImporter;
    //    importer.textureType = TextureImporterType.Default;
    //    importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
    //    importer.isReadable = true;
    //    importer.filterMode = FilterMode.Point;
    //    importer.npotScale = TextureImporterNPOTScale.None;
    //
    //    Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
    //    if (asset) {
    //        EditorUtility.SetDirty(asset); 
    //    } else {
    //        importer.textureType = TextureImporterType.Default;
    //    }
    //    Debug.Log("Preprocess");
    //}
}