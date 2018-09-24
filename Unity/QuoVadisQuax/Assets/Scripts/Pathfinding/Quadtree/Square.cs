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
        /// The North-West (Top-Left) point of the square
        /// </summary>
        public PixelPoint NW_Point { get; private set; }

        /// <summary>
        /// The South-East (Bottom-Right) point of the square
        /// </summary>
        public PixelPoint SE_Point { get; private set; }

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
        /// <param name="nwPoint">The North-West (Top-Left) point</param>
        /// <param name="sePoint">The South-East (Bottom-Right) point</param>
        public Square(PixelPoint nwPoint, PixelPoint sePoint)
        {
            NW_Point = nwPoint;
            SE_Point = sePoint;
            Width = (int) (SE_Point.X - NW_Point.X);
            Origin = new PixelPoint(NW_Point.X + Width / 2, NW_Point.Y + Height / 2);
        }

        /// <summary>
        /// Instantiates a new <see cref="Square"/> object
        /// </summary>
        /// <param name="nwPoint">The North-West (Top-Left) point</param>
        /// <param name="width">The width of the square</param>
        public Square(PixelPoint nwPoint, int width)
        {
            NW_Point = nwPoint;
            Width = width;
            SE_Point = new PixelPoint(NW_Point.X + Width, Mathf.Abs(NW_Point.Y + Height));
            Origin = new PixelPoint(NW_Point.X + Width / 2, NW_Point.Y + Height / 2);
        }

        /// <summary>
        /// Checks if this Square touches a <see cref="Square"/>
        /// </summary>
        /// <param name="other">The other <see cref="Square"/></param>
        /// <returns>True if this Square touches the other <see cref="Square"/></returns>
        public bool TouchesSquare(Square other)
        {
            return !(other.NW_Point.X > SE_Point.X || other.NW_Point.Y > SE_Point.Y ||
                     other.SE_Point.X < NW_Point.X || other.SE_Point.Y < NW_Point.Y);
        }

        /// <summary>
        /// Checks if this Square touches a <see cref="PixelPoint"/>
        /// </summary>
        /// <param name="other">The <see cref="PixelPoint"/></param>
        /// <returns>True if this Square touches the <see cref="PixelPoint"/></returns>
        public bool TouchesPoint(PixelPoint other)
        {
            return !(other.X > SE_Point.X || other.Y > SE_Point.Y ||
                     other.X < NW_Point.X || other.Y < NW_Point.Y);
        }

        #endregion
    }
}