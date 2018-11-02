using System.Collections.Generic;
using Algorithm.Quadtree;
using Algorithm.Quadtree;
using UnityEngine;
using UnityEngine.Serialization;

namespace Algorithm.Pathfinding
{
    public class PathfindingManager : MonoBehaviour
    {
        #region Properties
        
        private static PathfindingManager _instance;
        /// <summary>
        /// The Singleton Instance
        /// </summary>
        public static PathfindingManager Instance
        {
            get { return _instance; }
        }

        public delegate void RequestMapTileEventHandler(Vector2Int tilePos);
        public delegate void StartedPathfindingEventHandler();
        public delegate void FinishedPathfindingEventHandler(List<Node> path, bool foundPath);

        public event StartedPathfindingEventHandler StartedPathfinding;
        public event RequestMapTileEventHandler RequestedMapTile;
        public event FinishedPathfindingEventHandler FinishedPathfinding;

        public Grid AStarGrid;
        private Node startNode, targetNode;
        private Vector2Int startPos, targetPos;
        private bool createdGrid;

        #endregion

        #region Methods
        
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(gameObject);
        }

        public void SetupPathfinding(Vector2Int start, Vector2Int goal)
        {
            Debug.Log("PathfindingManager - SetupPathfinding");
            startPos = start;
            targetPos = goal;
            ThreadQueuer.Instance.StartThreadedAction(() => { AStarGrid.CreateGrid(ref createdGrid); });
            
            // TODO: REMOVE
            if (RequestedMapTile != null)
                RequestedMapTile.Invoke(start);
        }

        private void Update()
        {
            if (createdGrid)
            {
                //Debug.Log("PathfindingManager - Update - createdGrid");
                startNode = AStarGrid.NodeGrid[startPos.x, startPos.y];
                targetNode = AStarGrid.NodeGrid[targetPos.x, targetPos.y];

                //Debug.Log(startNode);
                //Debug.Log(targetNode);

                createdGrid = false;
                
                if (StartedPathfinding != null)
                    StartedPathfinding.Invoke();

                ThreadQueuer.Instance.StartThreadedAction(() => { FindPath(true); });
            }
        }

        public void FindPath(bool canWalkUnknown)
        {
            Debug.Log("PathfindingManager - FindPath");

            Heap<Node> openSet = new Heap<Node>(AStarGrid.NodeGrid.GetLength(0) * AStarGrid.NodeGrid.GetLength(1));
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (var neighbour in AStarGrid.GetNeighbours(currentNode))
                {
                    if (!neighbour.IsWalkable(canWalkUnknown) || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        void RetracePath(Node startNode, Node endNode)
        {
            Debug.Log("PathfindingManager - RetracePath");

            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();

            if (FinishedPathfinding != null)
                FinishedPathfinding.Invoke(path, true);
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            //Debug.Log("PathfindingManager - GetDistance");

            int dstX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
            int dstY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        #endregion
    }
}