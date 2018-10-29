using System;

namespace Algorithm.Quadtree
{
	/// <summary>
	/// Represents a Quadtree Node
	/// </summary>
	public class Node : NodeElement
	{
		#region Properties
		
		/// <summary>
		/// The Child Nodes of this Node
		/// </summary>
		public ChildNodes ChildNodes { get; set; }
		
		#endregion

		#region Methods

		/// <summary>
		/// Instantiates a new <see cref="Node"/> object
		/// </summary>
		/// <param name="swPoint">The South-West (Bottom-Left) point of the Node`s Square</param>
		/// <param name="width">The width of the Node`s Square</param>
		public Node(Vector2Int swPoint, int width)
		{
			MapSquare = new MapSquare(swPoint, width);
			CalculateChildNodes();
		}

		/// <summary>
		/// Calculates the <see cref="ChildNodes"/> of this Node
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		private void CalculateChildNodes()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
