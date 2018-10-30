using Algorithm.Pathfinding;
using Algorithm.Quadtree;
using UnityEngine;

namespace Algorithm
{
    public class AlgorithmManager : MonoBehaviour
    {
        #region Properties

        [SerializeField] private OptionsManager _optionsManager;
        [SerializeField] private PathfindingManager _pathfindingManager;
        [SerializeField] private QuadtreeManager _quadtreeManager;

        #endregion
        
        #region Methods

        private void Start()
        {
            _optionsManager.StartedAlgorithm += SetupAlgorithm;
        }

        public void SetupAlgorithm(Vector2Int quaxPos, Vector2Int cityPos)
        {
            Debug.Log("AlgorithmManager - SetupAlgorithm");
            _pathfindingManager.SetupPathfinding(quaxPos, cityPos);
        }
        
        #endregion
    }
}