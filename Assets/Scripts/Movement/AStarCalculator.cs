using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.TileHandling;
using Automate.Model.Utility;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class AStarCalculator
    {
        private Grid _gameWorld;

        public AStarCalculator(Grid gameWorld)
        {
            _gameWorld = gameWorld;
        }

        public MovementPath FindShortestPath(Vector2Int source, List<Vector2Int> validDestinations, PathOptions pathOptions)
        {
            SortedList<double, Vector2Int> toVisitList = new SortedList<double, Vector2Int>(new DuplicateKeyComparer<double>());
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int> originPosition = new Dictionary<Vector2Int, Vector2Int>();
            //instantiate the to-visit list
            toVisitList.Add(0, source);


            while ((toVisitList.Count > 0) && (!visited.Intersect(validDestinations).Any()))
            {
                var currentPosition = toVisitList.Values[0];
                var currentCost = toVisitList.Keys[0];
                toVisitList.RemoveAt(0);
                Debug.Log("visiting " + currentPosition);

                if (visited.Contains(currentPosition))
                    continue;
                visited.Add(currentPosition);
                var neighbours = GetAllNeighbours(currentPosition);
                foreach (Vector2Int neighbour in neighbours)
                {
                    if (visited.Contains(neighbour) || toVisitList.ContainsValue(neighbour))
                        continue;
                    if (!Tilemapper.IsGridPassableAtCoordinate(_gameWorld,neighbour))
                        continue;
                    originPosition.Add(neighbour,currentPosition);
                    double newCost = currentCost + CalculateCost(currentPosition, neighbour, 1f);
                    toVisitList.Add(newCost, neighbour);
                }
            }

            if (visited.Intersect(validDestinations).Any())
            {
                //build the return path
                List<Vector2Int> listOfCoords = new List<Vector2Int>();
                var currentPosition = visited.Intersect(validDestinations).First();
                var timeout = 0;
                while (currentPosition != source || timeout > 5000)
                {
                    listOfCoords.Insert(0,currentPosition);
                    currentPosition = originPosition[currentPosition];
                    timeout++;
                }
                listOfCoords.Insert(0, currentPosition);

                if (timeout >= 5000)
                    throw new TimeoutException("Finding source of path timed out -- unexpected!");
                return new MovementPath(listOfCoords);
            }
            return null;
        }

        public double CalculateCost(Vector2Int source, Vector2Int target, double baseCost)
        {
            var diff = source - target;
            //TODO: we can easily optimize this to be a mapping of positions as below ==> numbers!!!
            var normalized = Math.Sqrt(Math.Pow(diff.x, 2) + Math.Pow(diff.y, 2));
            return normalized * baseCost;
        }

        public List<Vector2Int> GetAllNeighbours(Vector2Int position)
        {
            return new List<Vector2Int>()
            {
                position + Vector2Int.down,
                position + Vector2Int.up,
                position + Vector2Int.left,
                position + Vector2Int.right,
                position + Vector2Int.down + Vector2Int.left,
                position + Vector2Int.down + Vector2Int.right,
                position + Vector2Int.up + Vector2Int.left,
                position + Vector2Int.up + Vector2Int.right,
            };
        }
    }
}