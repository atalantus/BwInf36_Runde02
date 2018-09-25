namespace Pathfinding.Quadtree
{
    /// <summary>
    /// Represents a Point in the Map
    /// </summary>
    public struct PixelPoint
    {
        /// <summary>
        /// The Y-Coordinate of the Point
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// The X-Coordinate of the Point
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Instantiates a new <see cref="PixelPoint"/> struct object
        /// </summary>
        /// <param name="y"><see cref="Y"/></param>
        /// <param name="x"><see cref="X"/></param>
        public PixelPoint(int y, int x) : this()
        {
            Y = y;
            X = x;
        }
    }
}