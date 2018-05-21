using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Movement
{
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

    public class MovementInstance
    {
        private readonly MovementPath movementPath;
        public List<Vector2Int>.Enumerator Enumerator;

        public MovementInstance(MovementPath path)
        {
            this.movementPath = path;
            this.Enumerator = path.getPositionsInPath();
        }

    }
}