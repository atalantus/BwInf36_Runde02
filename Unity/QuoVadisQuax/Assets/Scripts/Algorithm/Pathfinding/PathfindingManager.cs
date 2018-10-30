using Algorithm.Quadtree;
using Algorithm.Quadtree;
using UnityEngine;

namespace Algorithm.Pathfinding
{
    public class PathfindingManager : MonoBehaviour
    {
        #region Properties
        
        public delegate void RequestMapTileEventHandler(Vector2Int tilePos);

        public event RequestMapTileEventHandler RequestedMapTile;

        public Grid PresetGrid;
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
            ThreadQueuer.Instance.StartThreadedAction(() => {PresetGrid.CreateGrid(ref createdGrid);});
        }

        private void Update()
        {
            if (createdGrid)
            {
                Debug.Log("PathfindingManager - Update - createdGrid");
                startNode = PresetGrid.NodeGrid[startPos.x, startPos.y];
                targetNode = PresetGrid.NodeGrid[targetPos.x, targetPos.y];
                
                Debug.Log(startNode);
                Debug.Log(targetNode);

                createdGrid = false;
            }
        }

        public void FindPath()
        {
            
        }

        public void StartPathSearch(bool canWalkUnknown)
        {
            
        }

        #endregion
    }
}