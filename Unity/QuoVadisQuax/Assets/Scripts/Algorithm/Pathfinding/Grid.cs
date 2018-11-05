using System.Collections.Generic;

namespace Algorithm.Pathfinding
{
    /// <summary>
    ///     The pathfinding grid
    /// </summary>
    public class Grid
    {
        #region Properties

        /// <summary>
        ///     Grid width
        /// </summary>
        private readonly int _gridSizeX;

        /// <summary>
        ///     Grid height
        /// </summary>
        private readonly int _gridSizeY;

        /// <summary>
        ///     Nodes of the grid
        /// </summary>
        public Node[,] NodeGrid { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates a new grid
        /// </summary>
        /// <param name="width">Width of the grid</param>
        /// <param name="height">Height of the grid</param>
        public Grid(int width, int height)
        {
            NodeGrid = new Node[width, height];
            _gridSizeX = width;
            _gridSizeY = height;
        }

        /// <summary>
        ///     Setup the grid
        /// </summary>
        /// <param name="isFinished"></param>
        public void SetupGrid(ref bool isFinished)
        {
            for (var x = 0; x < _gridSizeX; x++)
            for (var y = 0; y < _gridSizeY; y++)
                NodeGrid[x, y] = new Node(new Vector2Int(x, y));

            isFinished = true;
        }

        /// <summary>
        ///     Get the neighbour <see cref="Node" />s
        /// </summary>
        /// <param name="node">Node to get the neighbours from</param>
        /// <returns>List with neighbour nodes</returns>
        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                var checkX = node.Position.X + x;
                var checkY = node.Position.Y + y;

                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                    neighbours.Add(NodeGrid[checkX, checkY]);
            }

            return neighbours;
        }

        #endregion
    }
}