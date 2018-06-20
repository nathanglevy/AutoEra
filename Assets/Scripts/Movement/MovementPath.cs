using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class MovementPath
    {
        private readonly List<Vector3Int> positionsInPath;
        public MovementPath(List<Vector3Int> positionsInPathList)
        {
            this.positionsInPath = positionsInPathList;
        }
        public List<Vector3Int>.Enumerator getPositionsInPath()
        {
            return positionsInPath.GetEnumerator();
        }
        public Vector3Int Source
        {
            get { return positionsInPath.First(); }
        }

        public Vector3Int Destination
        {
            get { return positionsInPath.Last(); }
        }
    }

    public class MovementInstance
    {
        public readonly MovementPath movementPathSource;
        public List<Vector3Int>.Enumerator Enumerator;

        public MovementInstance(MovementPath path)
        {
            this.movementPathSource = path;
            this.Enumerator = path.getPositionsInPath();
        }

    }
}