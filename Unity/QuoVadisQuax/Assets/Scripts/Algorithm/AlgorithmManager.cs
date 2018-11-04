using System;
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
        
        public delegate void FinishedAlgorithmEventHandler(bool foundPath, int quadcopterFlights, TimeSpan algorithmTime);

        public event FinishedAlgorithmEventHandler FinishedAlgorithm;
        
        [SerializeField] private OptionsManager _optionsManager;
        [SerializeField] private ContainerManager _containerManager;

        public static readonly string PREPARING_ALGORITHM_MSG_ID = "preparing_algorithm";
        public static readonly string SEARCHING_PATH_MSG_ID = "searching_path";

        private bool _foundPath;
        private int _quadcopterFlights;
        private Stopwatch _stopwatch;

        #endregion

        #region Methods

        private void Start()
        {
            Application.targetFrameRate = 300;
            
            _stopwatch = new Stopwatch();
            
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

            QuadtreeManager.Instance.CreatedNode += (a) => { _quadcopterFlights++; };
        }

        public void StartAlgorithm(Vector2Int quaxPos, Vector2Int cityPos)
        {
            //Debug.Log("AlgorithmManager - SetupAlgorithm");
            _foundPath = false;
            _quadcopterFlights = 0;
            _stopwatch.Reset();
            QuadtreeManager.Instance.SetupQuadtree();
            PathfindingManager.Instance.SetupPathfinding(quaxPos, cityPos);
        }

        public void FinishAlgorithm()
        {
            ulong time = (ulong) QuadtreeManager.Instance.QuadtreeTime +
                         (ulong) PathfindingManager.Instance.TotalTimeUpdateGrid +
                         (ulong) PathfindingManager.Instance.TotalTimePathfinding01 +
                         (ulong) PathfindingManager.Instance.TotalTimePathfinding02;

            var ts = TimeSpan.FromMilliseconds(time);
            
            _containerManager.DestroyMessage(SEARCHING_PATH_MSG_ID);
            _containerManager.CreateMessage(ts.Seconds + "s " + ts.Milliseconds + "ms", "algorithm_time", false, 5f);
            
            Debug.LogWarning("----- ALGORITHM TIME -----");
            Debug.LogWarning("Quadtree Searches: " + QuadtreeManager.Instance.QuadtreeTime);
            Debug.LogWarning("Updating A* Grid: " + PathfindingManager.Instance.TotalTimeUpdateGrid);
            Debug.LogWarning("Pathfinding 01: " + PathfindingManager.Instance.TotalTimePathfinding01);
            Debug.LogWarning("Pathfinding 02: " + PathfindingManager.Instance.TotalTimePathfinding02);
            Debug.LogWarning("WHOLE ALGORITHM: " + _stopwatch.Elapsed.TotalMilliseconds);


            if (FinishedAlgorithm != null)
                FinishedAlgorithm.Invoke(_foundPath, _quadcopterFlights, ts);
        }

        #endregion
    }
}