using System;
using System.Threading;

namespace Algorithm.Quadtree
{
    /// <summary>
    /// Contains the Child Nodes of a parent Node Element
    /// </summary>
    public class ChildNodes
    {
        #region Properties

        /// <summary>
        /// The 4 Child Nodes beginning with the North-East (Top-Right) Node, going clockwise
        /// </summary>
        public NodeElement[] Nodes { get; private set; }

        /// <summary>
        /// The North-East (Top-Right) Child Node
        /// </summary>
        public NodeElement NE_Node
        {
            get { return Nodes[2]; }
        }

        /// <summary>
        /// The South-East (Bottom-Right) Child Node
        /// </summary>
        public NodeElement SE_Node
        {
            get { return Nodes[1]; }
        }

        /// <summary>
        /// The South-West (Bottom-Left) Child Node
        /// </summary>
        public NodeElement SW_Node
        {
            get { return Nodes[0]; }
        }

        /// <summary>
        /// The North-West (Top-Left) Child Node
        /// </summary>
        public NodeElement NW_Node
        {
            get { return Nodes[3]; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new <see cref="ChildNodes"/> object
        /// </summary>
        /// <param name="childNodes">The Child Node objects</param>
        public ChildNodes(NodeElement[] childNodes)
        {
            Nodes = childNodes;

            foreach (var node in childNodes)
            {
                if (node.MapSquare.Width < 2)
                    throw new Exception("Node with width: " + node.MapSquare.Width);
            }
        }

        #endregion
    }
}