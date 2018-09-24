namespace Pathfinding.Quadtree
{
	/// <summary>
	/// Represents the possible smallest Quadtree Node
	/// </summary>
	public class EndNode : SquareNode
	{
		#region Methods

		/// <summary>
		/// Instantiates a new <see cref="EndNode"/> object
		/// </summary>
		/// <param name="nwPoint">The North-West (Top-Left) point of the Node`s Square</param>
		/// <param name="width">The width of the Node`s Square</param>
		public EndNode(PixelPoint nwPoint, int width)
		{
			MapSquare = new MapSquare(nwPoint, width);
		}
		
		#endregion
	}
}
