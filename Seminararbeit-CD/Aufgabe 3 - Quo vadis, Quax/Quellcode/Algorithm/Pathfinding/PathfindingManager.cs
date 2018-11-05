using System;
using System.Collections.Generic;
using System.Diagnostics;
using Algorithm.Quadtree;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Algorithm.Pathfinding
{
    /// <inheritdoc />
    /// <summary>
    ///     General pathfinding manager
    /// </summary>
    public class PathfindingManager : MonoBehaviour
    {
        #region Properties

        /// <summary>
        ///     The Singleton Instance
        /// </summary>
        public static PathfindingManager Instance { get; private set; }

        /// <summary>
        ///     Request map tile delegate
        /// </summary>
        /// <param name="sw_point">The South-West (Bottom-Left) position of the requested map tile</param>
        public delegate void RequestMapTileEventHandler(Vector2Int sw_point);

        /// <summary>
        ///     Request special square delegate
        /// </summary>
        /// <param name="sw_point">The South-West (Bottom-Left) position of the requested map tile</param>
        public delegate void RequestSpecialSquareEventHandler(Vector2Int sw_point);

        /// <summary>
        ///     Started pathfinding delegate
        /// </summary>
        public delegate void StartedPathfindingEventHandler();

        /// <summary>
        ///     Finished pathfinding delegate
        /// </summary>
        /// <param name="path">List of nodes forming the path</param>
        /// <param name="foundPath">Was a path found</param>
        public delegate void FinishedPathfindingEventHandler(List<Node> path, bool foundPath);

        /// <summary>
        ///     Started pathfinding event
        /// </summary>
        public event StartedPathfindingEventHandler StartedPathfinding;

        /// <summary>
        ///     Request map tile of quadtree event
        /// </summary>
        public event RequestMapTileEventHandler RequestedMapTile;

        /// <summary>
        ///     Request map tile outside of quadtree event
        /// </summary>
        public event RequestSpecialSquareEventHandler RequestSpecialSquare;

        /// <summary>
        ///     Finished pathfinding event
        /// </summary>
        public event FinishedPathfindingEventHandler FinishedPathfinding;

        /// <summary>
        ///     The grid used for pathfinding
        /// </summary>
        public Grid PathfindingGrid;

        /// <summary>
        ///     The start node
        /// </summary>
        private Node _startNode;

        /// <summary>
        ///     The target node
        /// </summary>
        private Node _targetNode;

        /// <summary>
        ///     The start position
        /// </summary>
        private Vector2Int _startPos;

        /// <summary>
        ///     The target position
        /// </summary>
        private Vector2Int _targetPos;

        /// <summary>
        ///     Is <see cref="PathfindingGrid" /> created
        /// </summary>
        private bool _createdGrid;

        /// <summary>
        ///     Is <see cref="PathfindingGrid" /> updated
        /// </summary>
        private bool _updatedGrid;

        /// <summary>
        ///     Is pathfinding with unknown nodes as walkable
        /// </summary>
        private bool _tryPathfinding02;

        /// <summary>
        ///     Cached open set
        /// </summary>
        private Heap<Node> _cachedOpenSet;

        /// <summary>
        ///     Cached closed set
        /// </summary>
        private HashSet<Node> _cachedClosedSet;

        /// <summary>
        ///     The cached, last walkable node forming the path
        /// </summary>
        private Node _cachedLastPathNode;

        /// <summary>
        ///     The total execution time spent while updating the <see cref="PathfindingGrid" />
        /// </summary>
        public double TotalTimeUpdateGrid;

        /// <summary>
        ///     The total execution time spent pathfinding with unknown nodes as unwalkable
        /// </summary>
        public double TotalTimePathfinding01;

        /// <summary>
        ///     The total execution time spent pathfinding with unknown nodes as walkable
        /// </summary>
        public double TotalTimePathfinding02;

        #endregion

        #region Methods

        /// <summary>
        ///     Unity Awake
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        /// <summary>
        ///     Unity Start
        /// </summary>
        private void Start()
        {
            /**
             * Subscribing to events
             */
            QuadtreeManager.Instance.UpdatedQuadtree += (square, sw_point) =>
            {
                ThreadQueuer.Instance.StartThreadedAction(() =>
                {
                    var s = new Stopwatch();
                    s.Start();
                    try
                    {
                        UpdateGrid(square, sw_point, ref _updatedGrid);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }

                    s.Stop();
                    //Debug.Log("Updating Grid took: " + s.Elapsed.TotalMilliseconds + "ms");
                    TotalTimeUpdateGrid += s.Elapsed.TotalMilliseconds;
                });
            };

            QuadtreeManager.Instance.CheckedSpecialSquare += (isWalkable, sw_point) =>
            {
                PathfindingGrid.NodeGrid[sw_point.X, sw_point.Y].NodeType =
                    isWalkable ? NodeTypes.Walkable : NodeTypes.Unwalkable;
                _updatedGrid = true;
            };
        }

        /// <summary>
        ///     Setup pathfinding
        /// </summary>
        /// <param name="start">The start position</param>
        /// <param name="target">The target position</param>
        public void SetupPathfinding(Vector2Int start, Vector2Int target)
        {
            _startPos = start;
            _targetPos = target;

            _cachedOpenSet =
                new Heap<Node>(PathfindingGrid.NodeGrid.GetLength(0) * PathfindingGrid.NodeGrid.GetLength(1));
            _cachedClosedSet = new HashSet<Node>();
            _cachedLastPathNode = null;

            TotalTimeUpdateGrid = 0;
            TotalTimePathfinding01 = 0;
            TotalTimePathfinding02 = 0;

            // Start creating the pathfinding grid
            ThreadQueuer.Instance.StartThreadedAction(() => { PathfindingGrid.SetupGrid(ref _createdGrid); });
        }

        /// <summary>
        ///     Update the <see cref="PathfindingGrid" />
        /// </summary>
        /// <param name="updatedSquares">List of updated squares</param>
        /// <param name="sw_point">The South-West (Bottom-Left) point of the searched square</param>
        /// <param name="updated">Was the grid updated</param>
        public void UpdateGrid(List<MapSquare> updatedSquares, Vector2Int sw_point, ref bool updated)
        {
            bool isWalkable;

            if (updatedSquares.TrueForAll(s => s.MapType == MapTypes.Ground))
            {
                isWalkable = true;
            }
            else if (updatedSquares.TrueForAll(s => s.MapType == MapTypes.Water))
            {
                isWalkable = false;
            }
            else
            {
                // Need more accurate square information outside of quadtree
                if (RequestSpecialSquare != null)
                    RequestSpecialSquare.Invoke(sw_point);
                return;
            }

            PathfindingGrid.NodeGrid[sw_point.X, sw_point.Y].NodeType =
                isWalkable ? NodeTypes.Walkable : NodeTypes.Unwalkable;

            updated = true;
        }

        /// <summary>
        ///     Unity Update
        /// </summary>
        private void Update()
        {
            /**
             * Grid was created
             */
            if (_createdGrid)
            {
                _startNode = PathfindingGrid.NodeGrid[_startPos.X, _startPos.Y];
                _targetNode = PathfindingGrid.NodeGrid[_targetPos.X, _targetPos.Y];

                _cachedLastPathNode = _startNode;

                _createdGrid = false;

                if (StartedPathfinding != null)
                    StartedPathfinding.Invoke();

                _startNode.NodeType = NodeTypes.Walkable;
                _targetNode.NodeType = NodeTypes.Walkable;

                // Start the initial pathfinding
                _updatedGrid = true;
            }

            /**
             * Grid was updated
             */
            if (_updatedGrid)
            {
                _updatedGrid = false;

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
                    //Debug.Log("Pathfinding 01 took: " + s.Elapsed.TotalMilliseconds + "ms");
                    TotalTimePathfinding01 += s.Elapsed.TotalMilliseconds;
                });
            }

            /**
             * Execute pathfinding with unknown nodes as walkable
             */
            if (_tryPathfinding02)
            {
                _tryPathfinding02 = false;

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
                    //Debug.Log("Pathfinding 02 took: " + s.Elapsed.TotalMilliseconds + "ms");
                    TotalTimePathfinding02 += s.Elapsed.TotalMilliseconds;
                });
            }
        }

        /// <summary>
        ///     Try find a path to the <see cref="_targetNode" />
        /// </summary>
        /// <param name="canWalkUnknown">Are unknown nodes considered walkable</param>
        public void FindPath(bool canWalkUnknown)
        {
            Heap<Node> openSet;
            HashSet<Node> closedSet;

            if (!canWalkUnknown)
            {
                // Use cached sets
                openSet = _cachedOpenSet;
                closedSet = _cachedClosedSet;
                openSet.Add(_cachedLastPathNode);
            }
            else
            {
                // Start from the zero
                openSet =
                    new Heap<Node>(PathfindingGrid.NodeGrid.GetLength(0) * PathfindingGrid.NodeGrid.GetLength(1));
                closedSet = new HashSet<Node>();
                openSet.Add(_startNode);
            }

            while (openSet.Count > 0)
            {
                // Remove node from the open set and add it to the closed set
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == _targetNode)
                {
                    // Found a path to the target
                    var path = RetracePath(_startNode, _targetNode);

                    if (canWalkUnknown)
                    {
                        // Get the first unknown node in the path
                        for (var i = 0; i < path.Count; i++)
                            if (path[i].NodeType == NodeTypes.Unknown)
                            {
                                // cache the last node on the path that is walkable
                                _cachedLastPathNode = i - 1 < 0 ? _startNode : path[i - 1];

                                // Request map information about the first unknown node on the path
                                if (RequestedMapTile != null)
                                    RequestedMapTile.Invoke(path[i].Position);
                                break;
                            }
                    }
                    else
                    {
                        // Found a valid path from the start to the city

                        /**
                         * DONE
                         */
                        if (FinishedPathfinding != null)
                            FinishedPathfinding.Invoke(path, true);
                    }

                    return;
                }

                /**
                 * Check all neighbours
                 */
                foreach (var neighbour in PathfindingGrid.GetNeighbours(currentNode))
                {
                    // Is the neighbour important
                    if (!neighbour.IsWalkable(canWalkUnknown) || closedSet.Contains(neighbour)) continue;

                    // Calculate neighbours G-Cost
                    var newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        // Update the neighbour 
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, _targetNode);
                        neighbour.Parent = currentNode;

                        // Add neighbour to the open set if it's not already in there
                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }

            // Unable to find a path
            if (canWalkUnknown)
            {
                // There is no path
                if (FinishedPathfinding != null)
                    FinishedPathfinding.Invoke(null, false);
            }
            else
            {
                // Try again with unknown nodes as walkable
                _tryPathfinding02 = true;
            }
        }

        /// <summary>
        ///     Retraces the path from the target node back to the start node
        /// </summary>
        /// <param name="startNode">Start node</param>
        /// <param name="targetNode">Target node</param>
        /// <returns>The path from the start node to the target node</returns>
        private List<Node> RetracePath(Node startNode, Node targetNode)
        {
            var path = new List<Node>();
            var currentNode = targetNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();

            return path;
        }

        /// <summary>
        ///     Calculates the distance between to nodes
        /// </summary>
        /// <param name="nodeA">First node</param>
        /// <param name="nodeB">Second node</param>
        /// <returns>Distance between the two nodes</returns>
        private int GetDistance(Node nodeA, Node nodeB)
        {
            var dstX = Mathf.Abs(nodeA.Position.X - nodeB.Position.X);
            var dstY = Mathf.Abs(nodeA.Position.Y - nodeB.Position.Y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        #endregion
    }
}