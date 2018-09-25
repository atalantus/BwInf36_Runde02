using UnityEngine;

namespace Pathfinding.Quadtree
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
        public PixelPoint SW_Point { get; private set; }

        /// <summary>
        /// The North-East (Top-Right) point of the square
        /// </summary>
        public PixelPoint NE_Point { get; private set; }

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
        public PixelPoint Origin { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Instantiates a new <see cref="Square"/> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point</param>
        /// <param name="nePoint">The North-East (Top-Right) point</param>
        public Square(PixelPoint swPoint, PixelPoint nePoint)
        {
            SW_Point = swPoint;
            NE_Point = nePoint;
            Width = NE_Point.X - SW_Point.X;
            Origin = new PixelPoint(SW_Point.X + Width / 2, SW_Point.Y + Height / 2);
        }

        /// <summary>
        /// Instantiates a new <see cref="Square"/> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point</param>
        /// <param name="width">The width of the square</param>
        public Square(PixelPoint swPoint, int width)
        {
            SW_Point = swPoint;
            Width = width;
            NE_Point = new PixelPoint(SW_Point.X + Width, Mathf.Abs(SW_Point.Y + Height));
            Origin = new PixelPoint(SW_Point.X + Width / 2, SW_Point.Y + Height / 2);
        }

        /// <summary>
        /// Checks if this Square touches a <see cref="Square"/>
        /// </summary>
        /// <param name="other">The other <see cref="Square"/></param>
        /// <returns>True if this Square touches the other <see cref="Square"/></returns>
        public bool TouchesSquare(Square other)
        {
            return !(other.SW_Point.X > NE_Point.X || other.SW_Point.Y > NE_Point.Y ||
                     other.NE_Point.X < SW_Point.X || other.NE_Point.Y < SW_Point.Y);
        }

        /// <summary>
        /// Checks if this Square touches a <see cref="PixelPoint"/>
        /// </summary>
        /// <param name="other">The <see cref="PixelPoint"/></param>
        /// <returns>True if this Square touches the <see cref="PixelPoint"/></returns>
        public bool TouchesPoint(PixelPoint other)
        {
            return !(other.X > NE_Point.X || other.Y > NE_Point.Y ||
                     other.X < SW_Point.X || other.Y < SW_Point.Y);
        }

        #endregion
    }
}