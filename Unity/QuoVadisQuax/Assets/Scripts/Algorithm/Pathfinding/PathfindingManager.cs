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

        public delegate void RequestMapTileEventHandler(Vector2Int tilePos);

        public delegate void FinishedPathfindingEventHandler(List<Node> path, bool foundPath);

        public event RequestMapTileEventHandler RequestedMapTile;
        public event FinishedPathfindingEventHandler FinishedPathfinding;

        public Grid AStarGrid;
        private Node startNode, targetNode;
        private Vector2Int startPos, targetPos;
        private bool createdGrid;

        #endregion

        #region Methods

        public void SetupPathfinding(Vector2Int start, Vector2Int goal)
        {
            Debug.Log("PathfindingManager - SetupPathfinding");
            startPos = start;
            targetPos = goal;
            ThreadQueuer.Instance.StartThreadedAction(() => { AStarGrid.CreateGrid(ref createdGrid); });
        }

        private void Update()
        {
            if (createdGrid)
            {
                Debug.Log("PathfindingManager - Update - createdGrid");
                startNode = AStarGrid.NodeGrid[startPos.x, startPos.y];
                targetNode = AStarGrid.NodeGrid[targetPos.x, targetPos.y];

                Debug.Log(startNode);
                Debug.Log(targetNode);

                createdGrid = false;

                ThreadQueuer.Instance.StartThreadedAction(() => { FindPath(true); });
            }
        }

        public void FindPath(bool canWalkUnknown)
        {
            Debug.Log("PathfindingManager - FindPath");

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost ||
                        openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
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
            Debug.Log("PathfindingManager - GetDistance");

            int dstX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
            int dstY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        #endregion
    }
}