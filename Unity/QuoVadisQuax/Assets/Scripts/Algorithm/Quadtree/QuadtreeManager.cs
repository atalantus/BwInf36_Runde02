using Algorithm.Pathfinding;
using UnityEngine;

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
            this._rootNode = null;
            this._rootNode = new Node(new Vector2Int(0,0), new Vector2Int(MapDataManager.Instance.Dimensions.x, MapDataManager.Instance.Dimensions.y));
        }

        private void SearchForPoint(Vector2Int point)
        {
            var square = _rootNode.FindPoint(point);
            
            if (UpdatedQuadtree != null)
                UpdatedQuadtree.Invoke(square);
        }

        public void RegisterNewNode(MapSquare mapSquare)
        {
            if (CreatedNode != null)
                CreatedNode.Invoke(mapSquare);
        }

        #endregion
    }
}