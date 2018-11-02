namespace Algorithm.Quadtree
{
	/// <summary>
	/// Base class for the Nodes in the Quadtree
	/// </summary>
	public abstract class NodeElement
	{
		#region Properties

		/// <summary>
		/// The map's square area that is covered by this Node Element
		/// </summary>
		public MapSquare MapSquare { get; set; }

		/// <summary>
		/// The unique Node ID
		/// </summary>
		public int NodeID { get; set; }

		#endregion

		#region Methods

		public abstract MapSquare FindPoint(Vector2Int point);

		/// <summary>
		/// Checks if this Square Node touches a <see cref="NodeElement"/>
		/// </summary>
		/// <param name="other">The other <see cref="NodeElement"/></param>
		/// <returns>True if this Square Node touches the other <see cref="NodeElement"/></returns>
		public bool TouchesSquareNode(NodeElement other)
		{
			return MapSquare.TouchesSquare(other.MapSquare);
		}

		/// <summary>
		/// Checks if this Square Node touches a <see cref="Vector2Int"/>
		/// </summary>
		/// <param name="other">The <see cref="Vector2Int"/></param>
		/// <returns>True if this Square Node touches the <see cref="Vector2Int"/></returns>
		public bool TouchesPoint(Vector2Int other)
		{
			return MapSquare.TouchesPoint(other);
		}

		#endregion
	}
}
