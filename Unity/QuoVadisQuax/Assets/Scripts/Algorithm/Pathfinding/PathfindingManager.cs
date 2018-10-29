using Algorithm.Quadtree;
using Algorithm.Quadtree;
using UnityEngine;

namespace Algorithm.Pathfinding
{
    public class PathfindingManager : MonoBehaviour
    {
        [SerializeField] private QuadtreeManager _quadtreeManager;

        private void Start()
        {
            _quadtreeManager.GeneratedQuadtree += FindPath;
        }

        public void FindPath(Vector2Int quaxPos, Vector2Int cityPos)
        {
            
        }
    }
}