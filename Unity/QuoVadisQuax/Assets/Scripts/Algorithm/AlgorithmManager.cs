using System.Collections.Generic;
using Algorithm.Pathfinding;
using Algorithm.Quadtree;
using UnityEngine;
using Node = Algorithm.Pathfinding.Node;

namespace Algorithm
{
    public class AlgorithmManager : MonoBehaviour
    {
        #region Properties

        [SerializeField] private OptionsManager _optionsManager;
        [SerializeField] private ContainerManager _containerManager;

        public static readonly string PREPARING_ALGORITHM_MSG_ID = "preparing_algorithm";
        public static readonly string SEARCHING_PATH_MSG_ID = "searching_path";

        #endregion

        #region Methods

        private void Start()
        {
            _optionsManager.StartedAlgorithm += (a, b) =>
            {
                _containerManager.CreateMessage("Preparing Quadcopter", PREPARING_ALGORITHM_MSG_ID, true);
            };

            PathfindingManager.Instance.StartedPathfinding += () =>
            {
                _containerManager.DestroyMessage(PREPARING_ALGORITHM_MSG_ID);
                _containerManager.CreateMessage("Searching Path", SEARCHING_PATH_MSG_ID, true);
            };
        }

        public void SetupAlgorithm(Vector2Int quaxPos, Vector2Int cityPos)
        {
            Debug.Log("AlgorithmManager - SetupAlgorithm");
            QuadtreeManager.Instance.SetupQuadtree();
            PathfindingManager.Instance.SetupPathfinding(quaxPos, cityPos);
        }

        #endregion
    }
}