using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Algorithm.Quadtree;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

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

        public delegate void RequestSpecialSquareEventHandler(Vector2Int lw_point);

        public delegate void StartedPathfindingEventHandler();

        public delegate void FinishedPathfindingEventHandler(List<Node> path, bool foundPath);

        public event StartedPathfindingEventHandler StartedPathfinding;
        public event RequestMapTileEventHandler RequestedMapTile;
        public event RequestSpecialSquareEventHandler RequestSpecialSquare;
        public event FinishedPathfindingEventHandler FinishedPathfinding;

        public Grid PathfindingGrid;
        private Node startNode, targetNode;
        private Vector2Int startPos, targetPos;

        private bool createdGrid;
        private bool _updatedGrid;
        private bool _tryPathfindingWithUnknownWalkable;

        private Heap<Node> _cachedOpenSet;
        private HashSet<Node> _cachedClosedSet;
        private Node _cachedLastPathNode;

        public double TotalTimeUpdateGrid;
        public double TotalTimePathfinding01;
        public double TotalTimePathfinding02;

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
            QuadtreeManager.Instance.UpdatedQuadtree += (square, lw_point) =>
            {
                ThreadQueuer.Instance.StartThreadedAction(() =>
                {
                    var s = new Stopwatch();
                    s.Start();
                    try
                    {
                        UpdateGrid(square, lw_point, ref _updatedGrid);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }

                    s.Stop();
                    Debug.LogWarning("Updating Grid took: " + s.Elapsed.TotalMilliseconds + "ms");
                    TotalTimeUpdateGrid += s.Elapsed.TotalMilliseconds;
                });
            };

            QuadtreeManager.Instance.CheckedSpecialSquare += (isWalkable, sw_point) =>
            {
                PathfindingGrid.NodeGrid[sw_point.x, sw_point.y].NodeType =
                    isWalkable ? NodeTypes.WALKABLE : NodeTypes.UNWALKABLE;
                _updatedGrid = true;
            };
        }

        public void SetupPathfinding(Vector2Int start, Vector2Int goal)
        {
            //Debug.Log("PathfindingManager - SetupPathfinding");
            startPos = start;
            targetPos = goal;

            _cachedOpenSet =
                new Heap<Node>(PathfindingGrid.NodeGrid.GetLength(0) * PathfindingGrid.NodeGrid.GetLength(1));
            _cachedClosedSet = new HashSet<Node>();
            _cachedLastPathNode = null;

            TotalTimeUpdateGrid = 0;
            TotalTimePathfinding01 = 0;
            TotalTimePathfinding02 = 0;

            ThreadQueuer.Instance.StartThreadedAction(() => { PathfindingGrid.CreateGrid(ref createdGrid); });
        }

        public void UpdateGrid(List<MapSquare> updatedSquares, Vector2Int sw_point, ref bool updated)
        {
            //Debug.LogWarning("PathfindingManager - UpdateGrid");

            var isWalkable = true;

            if (updatedSquares.TrueForAll(s => s.MapType == MapTypes.GROUND))
                isWalkable = true;
            else if (updatedSquares.TrueForAll(s => s.MapType == MapTypes.WATER))
                isWalkable = false;
            else
            {
                //Debug.LogWarning(sw_point + " nicht eindeutig!");
                if (RequestSpecialSquare != null)
                    RequestSpecialSquare.Invoke(sw_point);
                return;
            }


            PathfindingGrid.NodeGrid[sw_point.x, sw_point.y].NodeType =
                isWalkable ? NodeTypes.WALKABLE : NodeTypes.UNWALKABLE;

            updated = true;
        }

        private void Update()
        {
            if (createdGrid)
            {
                //Debug.Log("PathfindingManager - Update - createdGrid");
                startNode = PathfindingGrid.NodeGrid[startPos.x, startPos.y];
                targetNode = PathfindingGrid.NodeGrid[targetPos.x, targetPos.y];

                _cachedLastPathNode = startNode;

                //Debug.Log(startNode);
                //Debug.Log(targetNode);

                createdGrid = false;

                if (StartedPathfinding != null)
                    StartedPathfinding.Invoke();

                startNode.NodeType = NodeTypes.WALKABLE;
                targetNode.NodeType = NodeTypes.WALKABLE;

                _updatedGrid = true;
            }

            if (_updatedGrid)
            {
                _updatedGrid = false;

                //Debug.LogWarning("PathfindingManager - Update - SearchPath 01");
                ThreadQueuer.Instance.StartThreadedAction(() =>
                {
                    var s = new Stopwatch();
                    s.Start();

                    try
                    {
                        FindPath(false);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }

                    s.Stop();
                    Debug.LogWarning("Pathfinding 01 took: " + s.Elapsed.TotalMilliseconds + "ms");
                    TotalTimePathfinding01 += s.Elapsed.TotalMilliseconds;
                });
            }

            if (_tryPathfindingWithUnknownWalkable)
            {
                _tryPathfindingWithUnknownWalkable = false;

                //Debug.LogWarning("PathfindingManager - Update - SearchPath 02");
                ThreadQueuer.Instance.StartThreadedAction(() =>
                {
                    var s = new Stopwatch();
                    s.Start();

                    try
                    {
                        FindPath(true);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }

                    s.Stop();
                    Debug.LogWarning("Pathfinding 02 took: " + s.Elapsed.TotalMilliseconds + "ms");
                    TotalTimePathfinding02 += s.Elapsed.TotalMilliseconds;
                });
            }
        }

        public void FindPath(bool canWalkUnknown)
        {
            //Debug.Log("PathfindingManager - FindPath - canWalkUnknown: " + canWalkUnknown);

            Heap<Node> openSet;
            HashSet<Node> closedSet;

            if (!canWalkUnknown)
            {
                //Debug.LogWarning("Use Cached Sets");
                openSet = _cachedOpenSet;
                closedSet = _cachedClosedSet;
                openSet.Add(_cachedLastPathNode);
            }
            else
            {
                openSet =
                    new Heap<Node>(PathfindingGrid.NodeGrid.GetLength(0) * PathfindingGrid.NodeGrid.GetLength(1));
                closedSet = new HashSet<Node>();
                //closedSet = new HashSet<Node>(_cachedClosedSet);
                //openSet.Add(_cachedLastPathNode);
                openSet.Add(startNode);
            }

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
                            //Debug.LogWarning("Checking Path Node " + path[i].Position + " Type: " + path[i].NodeType);
                            if (path[i].NodeType == NodeTypes.UNKNOWN)
                            {
                                // cache last path node
                                if (i - 1 < 0)
                                {
                                    //Debug.LogWarning("SET CACHED NODE TO START NODE");
                                    _cachedLastPathNode = startNode;
                                }
                                else
                                {
                                    _cachedLastPathNode = path[i - 1];
                                }

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
            //Debug.Log("PathfindingManager - RetracePath");

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