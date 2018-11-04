using System;
using System.Diagnostics;
using Algorithm.Pathfinding;
using Algorithm.Quadtree;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Algorithm
{
    /// <inheritdoc />
    /// <summary>
    ///     General algorithm Manager
    /// </summary>
    public class AlgorithmManager : MonoBehaviour
    {
        #region Properties

        /// <summary>
        ///     Finished algorithm delegate
        /// </summary>
        /// <param name="foundPath">Was a path found or not</param>
        /// <param name="quadcopterFlights">Amount of Quadcopter flights</param>
        /// <param name="algorithmTime">Algorithm execution time</param>
        public delegate void FinishedAlgorithmEventHandler(bool foundPath, int quadcopterFlights,
            TimeSpan algorithmTime);

        /// <summary>
        ///     Finished algorithm event
        /// </summary>
        public event FinishedAlgorithmEventHandler FinishedAlgorithm;

        /// <summary>
        ///     OptionsManager reference
        /// </summary>
        [SerializeField] private OptionsManager _optionsManager;

        /// <summary>
        ///     ContainerManager reference
        /// </summary>
        [SerializeField] private ContainerManager _containerManager;

        /// <summary>
        ///     Preparing algorithm message ID
        /// </summary>
        private const string PREPARING_ALGORITHM_MSG_ID = "preparing_algorithm";

        /// <summary>
        ///     Searching path message ID
        /// </summary>
        private const string SEARCHING_PATH_MSG_ID = "searching_path";

        /// <summary>
        ///     Found a path
        /// </summary>
        private bool _foundPath;

        /// <summary>
        ///     Counter for Quadcopter flights
        /// </summary>
        private int _quadcopterFlights;

        /// <summary>
        ///     General algorithm Stopwatch
        /// </summary>
        private Stopwatch _stopwatch;

        #endregion

        #region Methods

        /// <summary>
        ///     Unity Start Method
        /// </summary>
        private void Start()
        {
            Application.targetFrameRate = 300;

            _stopwatch = new Stopwatch();

            /**
             * Subscribe to events
             */
            _optionsManager.StartedAlgorithm += (a, b) =>
            {
                _containerManager.CreateMessage("Preparing Quadcopter", PREPARING_ALGORITHM_MSG_ID, true);
            };

            PathfindingManager.Instance.StartedPathfinding += () =>
            {
                _containerManager.DestroyMessage(PREPARING_ALGORITHM_MSG_ID);
                _containerManager.CreateMessage("Finding Path", SEARCHING_PATH_MSG_ID, true);
                _stopwatch.Start();
            };

            PathfindingManager.Instance.FinishedPathfinding += (a, foundPath) =>
            {
                _stopwatch.Stop();
                _foundPath = foundPath;
            };

            QuadtreeManager.Instance.CreatedNode += a => { _quadcopterFlights++; };
        }

        /// <summary>
        ///     Start algorithm
        /// </summary>
        /// <param name="quaxPos"></param>
        /// <param name="cityPos"></param>
        public void StartAlgorithm(Vector2Int quaxPos, Vector2Int cityPos)
        {
            _foundPath = false;
            _quadcopterFlights = 0;
            _stopwatch.Reset();

            // Setup Quadtree and Pathfinding
            QuadtreeManager.Instance.SetupQuadtree();
            PathfindingManager.Instance.SetupPathfinding(quaxPos, cityPos);
        }

        /// <summary>
        ///     Finished algorithm
        /// </summary>
        public void FinishAlgorithm()
        {
            // Get pure algorithm time
            var time = (ulong) QuadtreeManager.Instance.QuadtreeTime +
                       (ulong) PathfindingManager.Instance.TotalTimeUpdateGrid +
                       (ulong) PathfindingManager.Instance.TotalTimePathfinding01 +
                       (ulong) PathfindingManager.Instance.TotalTimePathfinding02;

            var ts = TimeSpan.FromMilliseconds(time);

            _containerManager.DestroyMessage(SEARCHING_PATH_MSG_ID);
            _containerManager.CreateMessage(ts.Seconds + "s " + ts.Milliseconds + "ms", "algorithm_time", false, 5f);

            Debug.Log("----- ALGORITHM TIMES -----");
            Debug.Log("Quadtree Searches: " + QuadtreeManager.Instance.QuadtreeTime);
            Debug.Log("Updating A* Grid: " + PathfindingManager.Instance.TotalTimeUpdateGrid);
            Debug.Log("Pathfinding 01: " + PathfindingManager.Instance.TotalTimePathfinding01);
            Debug.Log("Pathfinding 02: " + PathfindingManager.Instance.TotalTimePathfinding02);
            Debug.Log("WHOLE ALGORITHM: " + _stopwatch.Elapsed.TotalMilliseconds);

            if (FinishedAlgorithm != null)
                FinishedAlgorithm.Invoke(_foundPath, _quadcopterFlights, ts);
        }

        #endregion
    }
}