using System.Collections.Generic;
using System.Diagnostics;
using Algorithm.Pathfinding;
using Algorithm.Quadtree;
using UnityEngine;
using Debug = UnityEngine.Debug;
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

        private Stopwatch _stopwatch;

        #endregion

        #region Methods

        private void Start()
        {
            _stopwatch = new Stopwatch();
            
            _optionsManager.StartedAlgorithm += (a, b) =>
            {
                _containerManager.CreateMessage("Preparing Quadcopter", PREPARING_ALGORITHM_MSG_ID, true);
            };

            PathfindingManager.Instance.StartedPathfinding += () =>
            {
                _containerManager.DestroyMessage(PREPARING_ALGORITHM_MSG_ID);
                _containerManager.CreateMessage("Searching Path", SEARCHING_PATH_MSG_ID, true);
                _stopwatch.Reset();
                _stopwatch.Start();
            };

            PathfindingManager.Instance.FinishedPathfinding += (a, b) => { _stopwatch.Stop(); };
        }

        public void SetupAlgorithm(Vector2Int quaxPos, Vector2Int cityPos)
        {
            Debug.Log("AlgorithmManager - SetupAlgorithm");
            QuadtreeManager.Instance.SetupQuadtree();
            PathfindingManager.Instance.SetupPathfinding(quaxPos, cityPos);
        }

        public void FinishedAlgorithm()
        {
            _containerManager.DestroyMessage(SEARCHING_PATH_MSG_ID);
            _containerManager.CreateMessage(_stopwatch.Elapsed.Seconds + "s " + _stopwatch.Elapsed.Milliseconds + "ms", "algorithm_time", false, 5f);
        }

        #endregion
    }
}