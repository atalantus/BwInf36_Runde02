using System.Diagnostics;
using Algorithm.Pathfinding;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Algorithm.Quadtree
{
    public class QuadtreeManager : MonoBehaviour
    {
        #region Properties
        
        private static QuadtreeManager _instance;
        /// <summary>
        /// The Singleton Instance
        /// </summary>
        public static QuadtreeManager Instance
        {
            get { return _instance; }
        }

        [SerializeField] private OptionsManager _optionsManager;
        
        public delegate void UpdatedQuadtreeEventHandler(MapSquare mapSquare);
        public delegate void CreatedNodeEventHandler(MapSquare mapSquare);

        public event UpdatedQuadtreeEventHandler UpdatedQuadtree;
        public event CreatedNodeEventHandler CreatedNode;
        
        public NodeElement _rootNode;

        public double QuadtreeTime;
        
        #endregion

        #region Methods

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            PathfindingManager.Instance.RequestedMapTile += SearchForPoint;
        }

        public void SetupQuadtree()
        {
            _rootNode = null;
            _rootNode = new Node(new Vector2Int(0,0), MapDataManager.Instance.Dimensions.x);
            QuadtreeTime = 0;
        }

        private void SearchForPoint(Vector2Int point)
        {
            //Debug.Log("QuadtreeManager - FindPoint " + point);
            
            // TODO: Performance: don't run whole quadtree on main thread! only GetPixel in GetMatType() in MapSquare.cs
            ThreadQueuer.Instance.QueueMainThreadAction(() =>
            {
                var s = new Stopwatch();
                s.Start();
                
                var square = _rootNode.FindPoint(point);
                
                s.Stop();
                Debug.LogWarning("Quadtree Search took: " + s.Elapsed.TotalMilliseconds + "ms");
                QuadtreeTime += s.Elapsed.TotalMilliseconds;
            
                if (UpdatedQuadtree != null)
                    UpdatedQuadtree.Invoke(square);
            });
        }

        public void RegisterNewNode(MapSquare mapSquare)
        {
            if (CreatedNode != null)
                CreatedNode.Invoke(mapSquare);
        }

        #endregion
    }
}