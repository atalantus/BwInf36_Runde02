using System;
using UnityEngine;

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
		/// Instantiates a new <see cref="Node"/> object
		/// </summary>
		/// <param name="swPoint">The South-West (Bottom-Left) point of the Node`s Square</param>
		/// <param name="nePoint">The North-East (Top-Right) point of the Node`s Square</param>
		public Node(Vector2Int swPoint, Vector2Int nePoint)
		{
			MapSquare = new MapSquare(swPoint, nePoint);
			CalculateChildNodes();
		}

		/// <summary>
		/// Calculates the <see cref="ChildNodes"/> of this Node
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		private void CalculateChildNodes()
		{
			var newWidth = Mathf.CeilToInt(MapSquare.Width / 2);
			NodeElement[] childNodes;

			if (newWidth > 2)
			{
				childNodes = new[]
				{
					new Node(
						new Vector2Int(MapSquare.SW_Point.x, MapSquare.SW_Point.y), newWidth),
					new Node(
						new Vector2Int(MapSquare.SW_Point.x + newWidth, MapSquare.SW_Point.y), newWidth),
					new Node(
						new Vector2Int(MapSquare.SW_Point.x + newWidth, MapSquare.SW_Point.y + newWidth), newWidth),
					new Node(
						new Vector2Int(MapSquare.SW_Point.x, MapSquare.SW_Point.y + newWidth), newWidth),
				};
			}
			else
			{
				childNodes = new[]
				{
					new EndNode(
						new Vector2Int(MapSquare.SW_Point.x, MapSquare.SW_Point.y), newWidth),
					new EndNode(
						new Vector2Int(MapSquare.SW_Point.x + newWidth, MapSquare.SW_Point.y), newWidth),
					new EndNode(
						new Vector2Int(MapSquare.SW_Point.x + newWidth, MapSquare.SW_Point.y + newWidth), newWidth),
					new EndNode(
						new Vector2Int(MapSquare.SW_Point.x, MapSquare.SW_Point.y + newWidth), newWidth),
				};
			}
			
			ChildNodes = new ChildNodes(childNodes);
		}

		#endregion
	}
}
