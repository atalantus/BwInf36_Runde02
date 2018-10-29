using System;

namespace Algorithm.Quadtree
{
    /// <summary>
    /// Represents a square cutout of the map
    /// </summary>
    public class MapSquare : Square
    {
        #region Properties

        /// <summary>
        /// The Map Type of the map's cutout
        /// </summary>
        public MapTypes MapType { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Instantiates a new <see cref="MapSquare"/> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point of the Node`s Square</param>
        /// <param name="width">The width of the Node`s Square</param>
        public MapSquare(Vector2Int swPoint, int width) : base(swPoint, width)
        {
        }

        /// <summary>
        /// Get`s the <see cref="MapType"/> of the map's cutout
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void GetMapTyp()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
