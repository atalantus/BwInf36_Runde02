namespace Algorithm.Quadtree
{
	/// <summary>
	/// Represents the possible smallest Quadtree Node
	/// </summary>
	public class EndNode : NodeElement
	{
		#region Properties

		public bool IsIndivisible { get; private set; }

		#endregion
		
		#region Methods

		/// <summary>
		/// Instantiates a new <see cref="EndNode"/> object
		/// </summary>
		/// <param name="swPoint">The South-West (Bottom-Left) point of the Node`s Square</param>
		/// <param name="width">The width of the Node`s Square</param>
		public EndNode(Vector2Int swPoint, int width)
		{
			MapSquare = new MapSquare(swPoint, width);
		}
		
		#endregion
	}
}
