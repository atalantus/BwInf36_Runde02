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
		/// <param name="swPoint">The South-West (Bottom-Left) point of the Node`s Square</param>
		/// <param name="width">The width of the Node`s Square</param>
		public EndNode(PixelPoint swPoint, int width)
		{
			MapSquare = new MapSquare(swPoint, width);
		}
		
		#endregion
	}
}
