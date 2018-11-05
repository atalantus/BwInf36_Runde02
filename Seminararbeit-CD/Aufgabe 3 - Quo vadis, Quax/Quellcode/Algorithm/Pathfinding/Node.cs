namespace Algorithm.Pathfinding
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents a pathfinding node
    /// </summary>
    public class Node : IHeapItem<Node>
    {
        #region Properties

        /// <summary>
        ///     Distance to start node
        /// </summary>
        public int GCost;

        /// <summary>
        ///     Distance to target node (Manhattan distance)
        /// </summary>
        public int HCost;

        /// <summary>
        ///     The sum of <see cref="GCost" /> and <see cref="HCost" />
        /// </summary>
        private int FCost
        {
            get { return GCost + HCost; }
        }

        /// <summary>
        ///     The parent node
        /// </summary>
        public Node Parent;

        /// <summary>
        ///     The node type
        /// </summary>
        public NodeTypes NodeType { get; set; }

        /// <summary>
        ///     The position of the node in the grid
        /// </summary>
        public Vector2Int Position { get; private set; }

        /// <summary>
        ///     The heap index
        /// </summary>
        public int HeapIndex { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates a new node
        /// </summary>
        /// <param name="position">Position of the node in the grid</param>
        public Node(Vector2Int position)
        {
            NodeType = NodeTypes.Unknown;
            Position = position;
        }

        /// <summary>
        ///     Check if the node is walkable
        /// </summary>
        /// <param name="canWalkUnknown">Are unknown nodes considered walkable</param>
        /// <returns>True if the node is walkable</returns>
        public bool IsWalkable(bool canWalkUnknown)
        {
            return NodeType == NodeTypes.Walkable || NodeType == NodeTypes.Unknown && canWalkUnknown;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Node other)
        {
            var compare = FCost.CompareTo(other.FCost);
            if (compare == 0) compare = HCost.CompareTo(other.HCost);

            return -compare;
        }

        #endregion
    }
}