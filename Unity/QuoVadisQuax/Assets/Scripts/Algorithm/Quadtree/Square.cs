using UnityEngine;

namespace Algorithm.Quadtree
{
    /// <summary>
    /// Defines a Square
    /// </summary>
    public class Square
    {
        #region Properties

        /// <summary>
        /// The South-West (Bottom-Left) point of the square
        /// </summary>
        public Vector2Int SW_Point { get; private set; }

        /// <summary>
        /// The North-East (Top-Right) point of the square
        /// </summary>
        public Vector2Int NE_Point { get; private set; }

        /// <summary>
        /// The width of the square
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the square
        /// </summary>
        public int Height
        {
            get { return Width; }
        }

        /// <summary>
        /// The origin of the square
        /// </summary>
        public Vector2Int Origin { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Instantiates a new <see cref="Square"/> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point</param>
        /// <param name="nePoint">The North-East (Top-Right) point</param>
        public Square(Vector2Int swPoint, Vector2Int nePoint)
        {
            SW_Point = swPoint;
            NE_Point = nePoint;
            Width = NE_Point.x - SW_Point.x;
            Origin = new Vector2Int(SW_Point.x + Width / 2, SW_Point.y + Height / 2);
        }

        /// <summary>
        /// Instantiates a new <see cref="Square"/> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point</param>
        /// <param name="width">The width of the square</param>
        public Square(Vector2Int swPoint, int width)
        {
            SW_Point = swPoint;
            Width = width;
            NE_Point = new Vector2Int(SW_Point.x + Width, Mathf.Abs(SW_Point.y + Height));
            Origin = new Vector2Int(SW_Point.x + Width / 2, SW_Point.y + Height / 2);
        }

        /// <summary>
        /// Checks if this Square touches a <see cref="Square"/>
        /// </summary>
        /// <param name="other">The other <see cref="Square"/></param>
        /// <returns>True if this Square touches the other <see cref="Square"/></returns>
        public bool TouchesSquare(Square other)
        {
            return !(other.SW_Point.x > NE_Point.x || other.SW_Point.y > NE_Point.y ||
                     other.NE_Point.x < SW_Point.x || other.NE_Point.y < SW_Point.y);
        }

        /// <summary>
        /// Checks if this Square touches a <see cref="PixelPoint"/>
        /// </summary>
        /// <param name="other">The <see cref="PixelPoint"/></param>
        /// <returns>True if this Square touches the <see cref="PixelPoint"/></returns>
        public bool TouchesPoint(Vector2Int other)
        {
            return !(other.x > NE_Point.x || other.y > NE_Point.y ||
                     other.x < SW_Point.x || other.y < SW_Point.y);
        }

        #endregion
    }
}