using System.Collections.Generic;

namespace Algorithm.Pathfinding
{
    public class Grid
    {
        private readonly int _gridSizeX;
        private readonly int _gridSizeY;

        public Grid(int width, int height)
        {
            NodeGrid = new Node[width, height];
            _gridSizeX = width;
            _gridSizeY = height;
        }

        public Node[,] NodeGrid { get; set; }

        public void CreateGrid(ref bool isFinished)
        {
            //Debug.Log("Grid - CreateGrid");
            for (var x = 0; x < _gridSizeX; x++)
            for (var y = 0; y < _gridSizeY; y++)
                NodeGrid[x, y] = new Node(new Vector2Int(x, y));

            isFinished = true;
        }

        public List<Node> GetNeighbours(Node node)
        {
            //Debug.Log("Grid - GetNeighbours");

            var neighbours = new List<Node>();

            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                var checkX = node.Position.x + x;
                var checkY = node.Position.y + y;

                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                    neighbours.Add(NodeGrid[checkX, checkY]);
            }

            return neighbours;
        }
    }
}