using System;
using Util;

namespace Algorithm.Quadtree
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents a square cutout of the map
    /// </summary>
    public class MapSquare : Square
    {
        #region Properties

        /// <summary>
        ///     The Map Type of the map's cutout
        /// </summary>
        public MapTypes MapType { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        ///     Instantiates a new <see cref="T:Algorithm.Quadtree.MapSquare" /> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point of the Node`s Square</param>
        /// <param name="width">The width of the Node`s Square</param>
        public MapSquare(Vector2Int swPoint, int width) : base(swPoint, width)
        {
            MapType = MapTypes.Unknown;
        }

        /// <summary>
        ///     Get`s the <see cref="MapType" /> of the map's cutout
        /// </summary>
        /// <exception cref="Exception">Can't find a high enough prime</exception>
        public void GetMapTyp()
        {
            var containsWater = false;
            var containsLand = false;

            // Get the pixels
            var pixels = MapDataManager.Instance.MapTexture.GetPixels(SW_Point.x, SW_Point.y, Width, Height);
            var pixelsSize = pixels.Length;

            var prime = Utilities.GetHigherPrime(pixelsSize);
            if (prime < 0) throw new Exception("Couldn't find a higher prime");

            var i = prime % pixelsSize;

            for (var j = 1; j <= pixelsSize; j++)
            {
                var pixelType = pixels[i].GetMapType();

                if (pixelType == MapTypes.Water && !containsWater)
                    containsWater = true;
                else if (pixelType != MapTypes.Water && !containsLand) containsLand = true;

                if (containsWater && containsLand) break;

                i = (i + prime) % pixelsSize;
            }

            if (containsLand && containsWater)
                MapType = MapTypes.Mixed;
            else if (containsLand)
                MapType = MapTypes.Ground;
            else
                MapType = MapTypes.Water;

            QuadtreeManager.Instance.RegisterNewNode(this);
        }

        #endregion
    }
}