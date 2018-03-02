using System;
using System.Windows.Media;

namespace Aufgabe03.Classes
{
    /// <summary>
    /// This struct represents one pixel of the image / a 10x10 area on the map
    /// </summary>
    public struct Node
    {
        public enum NodeTypes
        {
            QuaxPos,
            City,
            Water,
            Land
        }

        private NodeTypes _nodeType;

        public NodeTypes NodeType
        {
            get { return _nodeType; }
            private set { _nodeType = value; }
        }

        private Color _color;

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                SetNodeType();
            }
        }

        private int _xCoordinate;

        public int XCoordinate
        {
            get { return _xCoordinate; }
            private set { _xCoordinate = value; }
        }

        private int _yCoordinate;

        public int YCoordinate
        {
            get { return _yCoordinate; }
            private set { _yCoordinate = value; }
        }

        public Node(Color color, int xCoordinate, int yCoordinate) : this()
        {
            Color = color;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
        }

        private void SetNodeType()
        {
            if (Color.AreClose(Color, Colors.Black))
                NodeType = NodeTypes.Land;
            else if (Color.AreClose(Color, Colors.White))
                NodeType = NodeTypes.Water;
            else if (Color.AreClose(Color, Colors.Red))
                NodeType = NodeTypes.QuaxPos;
            else if (Color.AreClose(Color, Colors.Green))
                NodeType = NodeTypes.City;
            else
            {
                throw new Exception("Unexpected color value: " + Color);
            }

        }
    }
}