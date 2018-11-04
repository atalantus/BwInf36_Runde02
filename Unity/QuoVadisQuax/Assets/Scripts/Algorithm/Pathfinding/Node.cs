namespace Algorithm.Pathfinding
{
    public class Node : IHeapItem<Node>
    {
        public int gCost;
        public int hCost;
        public Node parent;

        public Node(Vector2Int position)
        {
            NodeType = NodeTypes.UNKNOWN;
            Position = position;
        }

        public NodeTypes NodeType { get; set; }
        public Vector2Int Position { get; private set; }

        public int fCost
        {
            get { return gCost + hCost; }
        }

        public int CompareTo(Node other)
        {
            var compare = fCost.CompareTo(other.fCost);
            if (compare == 0) compare = hCost.CompareTo(other.hCost);

            return -compare;
        }

        public int HeapIndex { get; set; }

        public bool IsWalkable(bool canWalkUnknown)
        {
            //Debug.Log("Node - IsWalkable");
            var result = NodeType == NodeTypes.WALKABLE || NodeType == NodeTypes.UNKNOWN && canWalkUnknown;
            //Debug.Log("NodeType: " + NodeType + "; canWalkUnknown: " + canWalkUnknown + "; result: " + result);
            return result;
        }

        public override string ToString()
        {
            return "PathfindingNode Pos: (" + Position.x + ", " + Position.y + ")";
        }
    }
}