using UnityEngine;

namespace Algorithm.Quadtree
{
	/// <summary>
	/// Represents the possible smallest Quadtree Node
	/// </summary>
	public class EndNode : NodeElement
	{
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
		
		public override MapSquare FindPoint(Vector2Int point)
		{
			if (MapSquare.MapType == MapTypes.UNKNOWN)
				MapSquare.GetMapTyp();

			//Debug.Log("Reached EndNode SW " + MapSquare.SW_Point + " NE " + MapSquare.NE_Point + " MapType: " + MapSquare.MapType);
			return MapSquare;
		}
		
		#endregion
	}
}
