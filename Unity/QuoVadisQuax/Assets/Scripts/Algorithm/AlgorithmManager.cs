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
        [SerializeField] private PathfindingManager _pathfindingManager;
        [SerializeField] private QuadtreeManager _quadtreeManager;
        [SerializeField] private ContainerManager _containerManager;

        public static readonly string SEARCHING_PATH_MSG_ID = "searching_path";

        #endregion

        #region Methods

        private void Start()
        {
            _optionsManager.StartedAlgorithm += (a, b) =>
            {
                _containerManager.CreateMessage("Searching Path", SEARCHING_PATH_MSG_ID, true);
            };
            
            _pathfindingManager.FinishedPathfinding += (a, b) =>
            {
                ThreadQueuer.Instance.QueueMainThreadAction(() =>
                {
                    _containerManager.DestroyMessage(SEARCHING_PATH_MSG_ID);
                });
            };
        }

        public void SetupAlgorithm(Vector2Int quaxPos, Vector2Int cityPos)
        {
            Debug.Log("AlgorithmManager - SetupAlgorithm");
            _pathfindingManager.SetupPathfinding(quaxPos, cityPos);
        }

        #endregion
    }
}