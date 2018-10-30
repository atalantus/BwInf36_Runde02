using UnityEngine;

namespace Algorithm.Pathfinding
{
    public class Node
    {
        public NodeTypes NodeType { private get; set; }
        public Vector2Int Position { get; private set; }

        public int gCost;
        public int hCost;
        public Node parent;

        public int fCost
        {
            get { return gCost + hCost; }
        }

        public Node(Vector2Int position)
        {
            NodeType = NodeTypes.UNKNOWN;
            Position = position;
        }

        public bool IsWalkable(bool canWalkUnknown)
        {
            Debug.Log("Node - IsWalkable");
            var result = NodeType == NodeTypes.WALKABLE || NodeType == NodeTypes.UNKNOWN && canWalkUnknown;
            Debug.Log("NodeType: " + NodeType + "; canWalkUnknown: " + canWalkUnknown + "; result: " + result);
            return result;
        }

        public override string ToString()
        {
            return "PathfindingNode Pos: (" + Position.x + ", " + Position.y + ")";
        }
    }
}