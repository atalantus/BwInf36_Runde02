using System;

namespace Pathfinding.Quadtree
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
        public MapTypes MapTyp { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Instantiates a new <see cref="MapSquare"/> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point of the Node`s Square</param>
        /// <param name="width">The width of the Node`s Square</param>
        public MapSquare(PixelPoint swPoint, int width) : base(swPoint, width)
        {
        }

        /// <summary>
        /// Get`s the <see cref="MapTyp"/> of the map's cutout
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void GetMapTyp()
        {
            // TODO: Get Map type
            throw new NotImplementedException();
        }

        #endregion
    }
}
