using UnityEngine;

namespace Algorithm.Quadtree
{
    public class QuadtreeManager : MonoBehaviour
    {
        #region Properties
        
        public delegate void UpdatedQuadtreeEventHandler(NodeElement[] updatedNodes);
        public delegate void CreatedNodeEventHandler(NodeElement node);

        public event UpdatedQuadtreeEventHandler UpdatedQuadtree;
        public event CreatedNodeEventHandler CreatedNode;
        
        public NodeElement _rootNode;
        
        #endregion

        #region Methods

        

        #endregion
    }
}