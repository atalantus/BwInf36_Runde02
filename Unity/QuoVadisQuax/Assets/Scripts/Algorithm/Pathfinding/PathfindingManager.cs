using System;
using System.Collections.Generic;
using System.Threading;
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

        public Grid PathfindingGrid;
        private Node startNode, targetNode;
        private Vector2Int startPos, targetPos;

        private bool createdGrid;
        private bool _updatedGrid;
        private bool _tryPathfindingWithUnknownWalkable;

        #endregion

        #region Methods

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            QuadtreeManager.Instance.UpdatedQuadtree += (square) =>
            {
                ThreadQueuer.Instance.StartThreadedAction(() => { UpdateGrid(square, ref _updatedGrid); });
            };
        }

        public void SetupPathfinding(Vector2Int start, Vector2Int goal)
        {
            //Debug.Log("PathfindingManager - SetupPathfinding");
            startPos = start;
            targetPos = goal;
            ThreadQueuer.Instance.StartThreadedAction(() => { PathfindingGrid.CreateGrid(ref createdGrid); });
        }

        public void UpdateGrid(MapSquare square, ref bool updated)
        {
            Debug.LogWarning("PathfindingManager - UpdateGrid");
            var mapType = square.MapType;

            if (mapType != MapTypes.GROUND && mapType != MapTypes.WATER)
                ThreadQueuer.Instance.QueueMainThreadAction(() =>
                {
                    throw new Exception("Unexpected MapType: " + mapType);
                });

            for (int x = square.SW_Point.x; x <= square.NE_Point.x; x++)
            {
                for (int y = square.SW_Point.y; y <= square.NE_Point.y; y++)
                {
                    if (PathfindingGrid.NodeGrid.GetLength(0) - 1 >= x &&
                        PathfindingGrid.NodeGrid.GetLength(1) - 1 >= y)
                    {
                        Debug.Log("(" + x + ", " + y + ") NodeType: " +
                                  (mapType == MapTypes.GROUND ? NodeTypes.WALKABLE : NodeTypes.UNWALKABLE));
                        PathfindingGrid.NodeGrid[x, y].NodeType =
                            mapType == MapTypes.GROUND ? NodeTypes.WALKABLE : NodeTypes.UNWALKABLE;
                    }
                    else
                    {
                        Debug.LogWarning(x + ", " + y + " liegt außerhalb des A* Grids (" +
                                         PathfindingGrid.NodeGrid.GetLength(0) + ", " +
                                         PathfindingGrid.NodeGrid.GetLength(1) + ")!");
                        // Quadtree Bereich außerhalb des A* Grids (Eh wasser also nicht wert zu untersuchen)
                    }
                }
            }

            updated = true;
        }

        private void Update()
        {
            if (createdGrid)
            {
                //Debug.Log("PathfindingManager - Update - createdGrid");
                startNode = PathfindingGrid.NodeGrid[startPos.x, startPos.y];
                targetNode = PathfindingGrid.NodeGrid[targetPos.x, targetPos.y];

                //Debug.Log(startNode);
                //Debug.Log(targetNode);

                createdGrid = false;

                if (StartedPathfinding != null)
                    StartedPathfinding.Invoke();


                if (RequestedMapTile != null)
                    RequestedMapTile.Invoke(startPos);
            }

            if (_updatedGrid)
            {
                _updatedGrid = false;

                Debug.LogWarning("PathfindingManager - Update - SearchPath 01");
                ThreadQueuer.Instance.StartThreadedAction(() => { FindPath(false); });
            }

            if (_tryPathfindingWithUnknownWalkable)
            {
                _tryPathfindingWithUnknownWalkable = false;

                Debug.LogWarning("PathfindingManager - Update - SearchPath 02");
                ThreadQueuer.Instance.StartThreadedAction(() => { FindPath(true); });
            }
        }

        public void FindPath(bool canWalkUnknown)
        {
            Debug.Log("PathfindingManager - FindPath");

            Heap<Node> openSet =
                new Heap<Node>(PathfindingGrid.NodeGrid.GetLength(0) * PathfindingGrid.NodeGrid.GetLength(1));
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    // Found a path

                    var path = RetracePath(startNode, targetNode);

                    if (canWalkUnknown)
                    {
                        // Get the first unknown node in the path
                        for (int i = 0; i < path.Count; i++)
                        {
                            Debug.LogWarning("Checking Path Node " + path[i].Position + " Type: " + path[i].NodeType);
                            if (path[i].NodeType == NodeTypes.UNKNOWN)
                            {
                                // Request map information about this node
                                if (RequestedMapTile != null)
                                    RequestedMapTile.Invoke(path[i].Position);
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Found a valid path -> DONE!
                        if (FinishedPathfinding != null)
                            FinishedPathfinding.Invoke(path, true);
                    }

                    return;
                }

                foreach (var neighbour in PathfindingGrid.GetNeighbours(currentNode))
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

            // Couldn't find a path
            if (canWalkUnknown)
            {
                // There is no path
                if (FinishedPathfinding != null)
                    FinishedPathfinding.Invoke(null, false);
            }
            else
            {
                // Try again with Unknown nodes as walkable
                _tryPathfindingWithUnknownWalkable = true;
            }
        }

        List<Node> RetracePath(Node startNode, Node endNode)
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

            return path;
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