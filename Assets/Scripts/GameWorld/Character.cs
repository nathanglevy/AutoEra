using System.Collections.Generic;
using Assets.Scripts.Movement;
using UnityEngine;

namespace Assets.Scripts.GameWorld
{
    public class Character : MonoBehaviour {
        private MovementInstance currentPath;
        private MovementPath pendingPath = null;
        [SerializeField]
        private Vector3Int currentCell;
        [SerializeField]
        private Vector3Int targetCell;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float timeStarted;
        [SerializeField] private Direction currentDirection = Direction.Down;

        public Inventory Inventory = new Inventory();
        //TODO -- Inventory system

        public void SetPosition(Vector3Int position)
        {
            transform.position = GetWorldLocation(position);
        }

        public void SetMovementPath(MovementPath pathToSet)
        {
            pendingPath = pathToSet;
        }

        public Vector3Int GetLocation()
        {
            var localCoords = this.gameObject.transform.position;
            return this.gameObject.GetComponentInParent<Grid>().LocalToCell(localCoords);
        }

        private Vector3 GetWorldLocation()
        {
            return this.gameObject.transform.position;
        }

        private Vector3 GetWorldLocation(Vector3Int location)
        {
            return this.gameObject.GetComponentInParent<Grid>().GetCellCenterWorld(location);
        }

        private void UpdateMovement()
        {
            if (currentPath == null && pendingPath != null)
            {
                currentPath = new MovementInstance(pendingPath);
                pendingPath = null;
                targetCell = currentCell;
            }

            while (currentPath != null && targetCell == currentCell)
            {
                bool isNotDone = currentPath.Enumerator.MoveNext();
                if (isNotDone)
                {
                    Vector3Int sourceCell = currentPath.Enumerator.Current.ToVec3();
                    StartWalking(sourceCell - targetCell);
                    timeStarted = Time.time;
                    targetCell = sourceCell;
                }
                else
                {
                    StopWalking();
                    currentPath = null;
                }
            }

            if (currentPath != null)
            {
                float distanceTravelled = (Time.time - timeStarted) * speed;
                float distanceTotal = Vector3.Distance(GetWorldLocation(currentCell), GetWorldLocation(targetCell));
                float fraction = distanceTravelled / distanceTotal;
                if (fraction > 1)
                {
                    currentCell = targetCell;
                    if (pendingPath != null)
                        currentPath = null;
                    return;
                }
                Vector3 newVector3 = Vector3.Lerp(GetWorldLocation(currentCell), GetWorldLocation(targetCell), fraction);
                Debug.Log("Current Pos:" + newVector3);
                transform.position = newVector3;
            }
        }

        private void StartWalking(Vector3Int direction)
        {
            //var characterSetup = gameObject.GetComponent<CharacterSetup>();
        
            Direction directionEnum;
            if (!directionMap.TryGetValue(direction.ToVec2(), out directionEnum))
                return;
            string walkAnimationName = GetAnimationName("Walk",directionEnum);
            if (directionEnum != currentDirection)
                SetDirection(directionEnum);
            GetDirectionGameObject(directionEnum).GetComponent<Animator>().Play(walkAnimationName);
        }

        private string GetAnimationName(string animation, Direction direction)
        {
            if (direction == Direction.Up || direction == Direction.Down) 
                return direction + " " + animation;
            return "Side " + animation;
        }

        private void StopWalking()
        {
            string idleAnimationName = GetAnimationName("Idle", currentDirection);
            GetDirectionGameObject(currentDirection).GetComponent<Animator>().Play(idleAnimationName); ;
        }

        private GameObject GetDirectionGameObject(Direction direction)
        {
            var characterSetup = gameObject.GetComponent<CharacterSetup>();
            switch (direction)
            {
                case Direction.Up:
                    return characterSetup.upDirection;
                case Direction.Down:
                    return characterSetup.downDirection;
                case Direction.Left:
                    return characterSetup.leftDirection;
                case Direction.Right:
                    return characterSetup.rightDirection;
                default:
                    return characterSetup.downDirection;
            }
        }

        private void SetDirection(Direction direction)
        {
            currentDirection = direction;
            var characterSetup = gameObject.GetComponent<CharacterSetup>();
            characterSetup.downDirection.gameObject.SetActive(false);
            characterSetup.upDirection.gameObject.SetActive(false);
            characterSetup.leftDirection.gameObject.SetActive(false);
            characterSetup.rightDirection.gameObject.SetActive(false);
            GetDirectionGameObject(direction).SetActive(true);
        }



        // Use this for initialization
        void Start ()
        {
            currentCell = GetLocation();
            timeStarted = Time.time;
        
        }
	
        // Update is called once per frame
        void Update ()
        {
            UpdateMovement();
        }

        private Dictionary<Vector2Int,Direction> directionMap = new Dictionary<Vector2Int, Direction>()
        {
            {Vector2Int.down ,  Direction.Down },
            {Vector2Int.down + Vector2Int.left ,  Direction.Left },
            {Vector2Int.down + Vector2Int.right ,  Direction.Right },
            {Vector2Int.up ,    Direction.Up },
            {Vector2Int.up + Vector2Int.left ,    Direction.Up },
            {Vector2Int.up + Vector2Int.right,    Direction.Up },
            {Vector2Int.left ,  Direction.Left },
            {Vector2Int.right , Direction.Right },
        };
    }

    enum Direction
    {
        Up, Down, Left, Right
    }
}