using System.Collections.Generic;
using UnityEngine;

public class MovementPath
{
    private readonly List<Vector2Int> positionsInPath;
    public MovementPath(List<Vector2Int> positionsInPathList)
    {
        this.positionsInPath = positionsInPathList;
    }
    public List<Vector2Int>.Enumerator getPositionsInPath()
    {
        return positionsInPath.GetEnumerator();
    }
}