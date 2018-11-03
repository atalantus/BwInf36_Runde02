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
		
		public override MapSquare FindPoint(Vector2Int point)
		{
			if (MapSquare.MapType == MapTypes.UNKNOWN)
				MapSquare.GetMapTyp();

			switch (MapSquare.MapType)
			{
				case MapTypes.WATER:
					return MapSquare;
				case MapTypes.GROUND:
					return MapSquare;
				case MapTypes.MIXED:
					NodeElement targetNode = null;
					for (int i = 0; i < ChildNodes.Nodes.Length; i++)
					{
						if (ChildNodes.Nodes[i].TouchesPoint(point))
							targetNode = ChildNodes.Nodes[i];
					}

					if (targetNode == null)
					{
						foreach (var node in ChildNodes.Nodes)
						{
							Debug.LogWarning("Node: SW " + node.MapSquare.SW_Point + " | NE " + node.MapSquare.NE_Point);
						}

						throw new Exception("Unable to find point " + point + " in quadtree");
					}
					
					Debug.LogWarning("Touching Node: SW " + targetNode.MapSquare.SW_Point + " | NE " + targetNode.MapSquare.NE_Point + " | Width " + targetNode.MapSquare.Width + " | Height " + targetNode.MapSquare.Height);
					Debug.LogWarning("Touches Point: " + point);

					return targetNode.FindPoint(point);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Calculates the <see cref="ChildNodes"/> of this Node
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		private void CalculateChildNodes()
		{
			var newWidth = Mathf.CeilToInt(MapSquare.Width / 2f);
			Debug.Log(newWidth);
			
			if (newWidth < 2)
				throw new Exception("New width is " + newWidth + " | (" + MapSquare.Width + "/2)");
			NodeElement[] childNodes;

			if (MapSquare.Width / 2 == newWidth)
			{
				// kein überlappen
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
			}
			else
			{
				// überlappen
				if (newWidth > 2)
				{
					childNodes = new[]
					{
						new Node(
							new Vector2Int(MapSquare.SW_Point.x, MapSquare.SW_Point.y), newWidth),
						new Node(
							new Vector2Int(MapSquare.SW_Point.x + newWidth - 1, MapSquare.SW_Point.y), newWidth),
						new Node(
							new Vector2Int(MapSquare.SW_Point.x + newWidth - 1, MapSquare.SW_Point.y + newWidth - 1), newWidth),
						new Node(
							new Vector2Int(MapSquare.SW_Point.x, MapSquare.SW_Point.y + newWidth - 1), newWidth),
					};
				}
				else
				{
					childNodes = new[]
					{
						new EndNode(
							new Vector2Int(MapSquare.SW_Point.x, MapSquare.SW_Point.y), newWidth),
						new EndNode(
							new Vector2Int(MapSquare.SW_Point.x + newWidth - 1, MapSquare.SW_Point.y), newWidth),
						new EndNode(
							new Vector2Int(MapSquare.SW_Point.x + newWidth - 1, MapSquare.SW_Point.y + newWidth), newWidth),
						new EndNode(
							new Vector2Int(MapSquare.SW_Point.x, MapSquare.SW_Point.y + newWidth - 1), newWidth),
					};
				}
			}
			
			
			ChildNodes = new ChildNodes(childNodes);
		}

		#endregion
	}
}
