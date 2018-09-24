namespace Pathfinding.Quadtree
{
	/// <summary>
	/// Base class for square Node Elements of the Quadtree
	/// </summary>
	public abstract class SquareNode : NodeElement
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

		/// <summary>
		/// Checks if this Square Node touches a <see cref="SquareNode"/>
		/// </summary>
		/// <param name="other">The other <see cref="SquareNode"/></param>
		/// <returns>True if this Square Node touches the other <see cref="SquareNode"/></returns>
		public bool TouchesSquareNode(SquareNode other)
		{
			return MapSquare.TouchesSquare(other.MapSquare);
		}

		/// <summary>
		/// Checks if this Square Node touches a <see cref="PixelPoint"/>
		/// </summary>
		/// <param name="other">The <see cref="PixelPoint"/></param>
		/// <returns>True if this Square Node touches the <see cref="PixelPoint"/></returns>
		public bool TouchesPoint(PixelPoint other)
		{
			return MapSquare.TouchesPoint(other);
		}

		#endregion
	}
}
