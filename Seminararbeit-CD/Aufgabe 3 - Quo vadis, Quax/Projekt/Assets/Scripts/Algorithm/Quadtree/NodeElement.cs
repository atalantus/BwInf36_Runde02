namespace Algorithm.Quadtree
{
    /// <summary>
    ///     Base class for the Nodes in the Quadtree
    /// </summary>
    public abstract class NodeElement
    {
        #region Properties

        /// <summary>
        ///     The map's square area that is covered by this Node Element
        /// </summary>
        public MapSquare MapSquare { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Searches for a given point inside the quadtree
        /// </summary>
        /// <param name="point">The point to look for</param>
        /// <returns>The <see cref="MapSquare" /> object that contains the point</returns>
        public abstract MapSquare FindPoint(Vector2Int point);

        /// <summary>
        ///     Checks if this Square Node touches a <see cref="Vector2Int" />
        /// </summary>
        /// <param name="other">The <see cref="Vector2Int" /></param>
        /// <returns>True if this Square Node touches the <see cref="Vector2Int" /></returns>
        public bool ContainsPoint(Vector2Int other)
        {
            return MapSquare.ContainsPoint(other);
        }

        #endregion
    }
}