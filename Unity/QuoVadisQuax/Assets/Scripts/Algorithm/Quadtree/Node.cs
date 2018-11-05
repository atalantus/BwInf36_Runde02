using System;
using UnityEngine;

namespace Algorithm.Quadtree
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents a Quadtree Node
    /// </summary>
    public class Node : NodeElement
    {
        #region Properties

        /// <summary>
        ///     The Child Nodes of this Node
        /// </summary>
        private NodeElement[] ChildNodes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Instantiates a new <see cref="Node" /> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point of the Node`s Square</param>
        /// <param name="width">The width of the Node`s Square</param>
        public Node(Vector2Int swPoint, int width)
        {
            MapSquare = new MapSquare(swPoint, width);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        /// <exception cref="T:System.Exception"></exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
        public override MapSquare FindPoint(Vector2Int point)
        {
            // If the the map type is still unknown, get the map type
            if (MapSquare.MapType == MapTypes.Unknown)
                MapSquare.GetMapTyp();

            if (!ContainsPoint(point))
            {
                // Point is outside of the quadtree boundaries
                var extraMapSquare = new MapSquare(point, 1) {MapType = MapTypes.Water};
                return extraMapSquare;
            }

            switch (MapSquare.MapType)
            {
                case MapTypes.Water:
                    return MapSquare;
                case MapTypes.Ground:
                    return MapSquare;
                case MapTypes.Mixed:
                    // If child nodes doesn't exist yet, create them
                    if (ChildNodes == null)
                        CalculateChildNodes();

                    NodeElement targetNode = null;

                    // Find a child node that contains the point
                    foreach (var childNode in ChildNodes)
                        if (childNode.ContainsPoint(point))
                            targetNode = childNode;

                    if (targetNode != null) return targetNode.FindPoint(point);

                    throw new Exception("Can't search for point " + point +
                                        " in the quadtree because it doesn't lay inside it's boundaries");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Calculates the <see cref="ChildNodes" /> of this Node
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void CalculateChildNodes()
        {
            var newWidth = Mathf.CeilToInt(MapSquare.Width / 2f);

            if (newWidth < 2)
                throw new Exception("New width is " + newWidth + " | (" + MapSquare.Width + "/2)");

            if (MapSquare.Width / 2 == newWidth)
            {
                /**
                 * Child nodes won't overlap
                 */

                if (newWidth > 2)
                    ChildNodes = new[]
                    {
                        new Node(
                            new Vector2Int(MapSquare.SW_Point.X, MapSquare.SW_Point.Y), newWidth),
                        new Node(
                            new Vector2Int(MapSquare.SW_Point.X + newWidth, MapSquare.SW_Point.Y), newWidth),
                        new Node(
                            new Vector2Int(MapSquare.SW_Point.X + newWidth, MapSquare.SW_Point.Y + newWidth), newWidth),
                        new Node(
                            new Vector2Int(MapSquare.SW_Point.X, MapSquare.SW_Point.Y + newWidth), newWidth)
                    };
                else
                    ChildNodes = new[]
                    {
                        new EndNode(
                            new Vector2Int(MapSquare.SW_Point.X, MapSquare.SW_Point.Y), newWidth),
                        new EndNode(
                            new Vector2Int(MapSquare.SW_Point.X + newWidth, MapSquare.SW_Point.Y), newWidth),
                        new EndNode(
                            new Vector2Int(MapSquare.SW_Point.X + newWidth, MapSquare.SW_Point.Y + newWidth), newWidth),
                        new EndNode(
                            new Vector2Int(MapSquare.SW_Point.X, MapSquare.SW_Point.Y + newWidth), newWidth)
                    };
            }
            else
            {
                /**
                 * Child nodes are going to overlap
                 */

                if (newWidth > 2)
                    ChildNodes = new[]
                    {
                        new Node(
                            new Vector2Int(MapSquare.SW_Point.X, MapSquare.SW_Point.Y), newWidth),
                        new Node(
                            new Vector2Int(MapSquare.SW_Point.X + newWidth - 1, MapSquare.SW_Point.Y), newWidth),
                        new Node(
                            new Vector2Int(MapSquare.SW_Point.X + newWidth - 1, MapSquare.SW_Point.Y + newWidth - 1),
                            newWidth),
                        new Node(
                            new Vector2Int(MapSquare.SW_Point.X, MapSquare.SW_Point.Y + newWidth - 1), newWidth)
                    };
                else
                    ChildNodes = new[]
                    {
                        new EndNode(
                            new Vector2Int(MapSquare.SW_Point.X, MapSquare.SW_Point.Y), newWidth),
                        new EndNode(
                            new Vector2Int(MapSquare.SW_Point.X + newWidth - 1, MapSquare.SW_Point.Y), newWidth),
                        new EndNode(
                            new Vector2Int(MapSquare.SW_Point.X + newWidth - 1, MapSquare.SW_Point.Y + newWidth),
                            newWidth),
                        new EndNode(
                            new Vector2Int(MapSquare.SW_Point.X, MapSquare.SW_Point.Y + newWidth - 1), newWidth)
                    };
            }
        }

        #endregion
    }
}