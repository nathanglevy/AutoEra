using UnityEngine;
using System.Collections;

public static class VectorTranforms
{
    public static Vector3Int ToVec3(this Vector2Int vector2)
    {
        return new Vector3Int(vector2.x,vector2.y,0);
    }

    public static Vector2Int ToVec2(this Vector3Int vector3)
    {
        return new Vector2Int(vector3.x, vector3.y);
    }
}
