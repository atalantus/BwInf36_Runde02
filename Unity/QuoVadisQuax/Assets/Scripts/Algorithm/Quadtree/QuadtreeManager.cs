using UnityEngine;

namespace Algorithm.Quadtree
{
    public class QuadtreeManager : MonoBehaviour
    {
        #region Properties
        
        public delegate void GeneratedQuadtreeEventHandler(Vector2Int quaxPos, Vector2Int cityPos);
        public delegate void CreatedNodeEventHandler(NodeElement node);

        public event GeneratedQuadtreeEventHandler GeneratedQuadtree;
        public event CreatedNodeEventHandler CreatedNode;
        
        [SerializeField] private OptionsManager _optionsManager;
        public NodeElement _rootNode;
        
        #endregion

        #region Methods

        private void Start()
        {
            _optionsManager.StartedAlgorithm += GenerateQuadtree;
        }

        public void GenerateQuadtree(Vector2Int quaxPos, Vector2Int cityPos)
        {
            if (GeneratedQuadtree != null)
                GeneratedQuadtree.Invoke(quaxPos, cityPos);
        }

        #endregion
    }
}