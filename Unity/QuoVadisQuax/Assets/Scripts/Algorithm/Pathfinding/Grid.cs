using System.Collections.Generic;
using UnityEngine;

namespace Algorithm.Pathfinding
{
    public class Grid
    {
        public Node[,] NodeGrid { get; set; }
        private int _gridSizeX, _gridSizeY;

        public Grid(int width, int height)
        {
            NodeGrid = new Node[width, height];
            _gridSizeX = width;
            _gridSizeY = height;
        }

        public void CreateGrid(ref bool isFinished)
        {
            Debug.Log("Grid - CreateGrid");
            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    NodeGrid[x,y] = new Node(new Vector2Int(x,y));
                }
            }
            
            isFinished = true;
        }
        
        public List<Node> GetNeighbours(Node node) {
            //Debug.Log("Grid - GetNeighbours");
            
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.Position.x + x;
                    int checkY = node.Position.y + y;

                    if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY) {
                        neighbours.Add(NodeGrid[checkX,checkY]);
                    }
                }
            }

            return neighbours;
        }
    }
}