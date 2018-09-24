using System;

namespace Pathfinding.Quadtree
{
	/// <summary>
	/// Represents a Quadtree Node
	/// </summary>
	public class Node : SquareNode
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
		/// <param name="nwPoint">The North-West (Top-Left) point of the Node`s Square</param>
		/// <param name="width">The width of the Node`s Square</param>
		public Node(PixelPoint nwPoint, int width)
		{
			MapSquare = new MapSquare(nwPoint, width);
			CalculateChildNodes();
		}

		/// <summary>
		/// Calculates the <see cref="ChildNodes"/> of this Node
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		private void CalculateChildNodes()
		{
			// TODO: Calculate Child Nodes
			throw new NotImplementedException();
		}

		#endregion
	}
}
