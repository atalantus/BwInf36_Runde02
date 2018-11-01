﻿using System;
using UnityEngine;
using Util;

namespace Algorithm.Quadtree
{
    /// <summary>
    /// Represents a square cutout of the map
    /// </summary>
    public class MapSquare : Square
    {
        #region Properties

        /// <summary>
        /// The Map Type of the map's cutout
        /// </summary>
        public MapTypes MapType { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Instantiates a new <see cref="MapSquare"/> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point of the Node`s Square</param>
        /// <param name="width">The width of the Node`s Square</param>
        public MapSquare(Vector2Int swPoint, int width) : base(swPoint, width)
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="MapSquare"/> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point of the Node`s Square</param>
        /// <param name="nePoint">The North-East (Top-Right) point of the Node`s Square</param>
        public MapSquare(Vector2Int swPoint, Vector2Int nePoint) : base(swPoint, nePoint)
        {
            
        }

        /// <summary>
        /// Get`s the <see cref="MapType"/> of the map's cutout
        /// </summary>
        /// <returns>The Map Type</returns>
        public MapTypes GetMapTyp()
        {
            var containsWater = false;
            var containsLand = false;

            var pixels = MapDataManager.Instance.MapTexture.GetPixels(SW_Point.x, SW_Point.y, Width, Height);
            var pixelsSize = pixels.Length;
            var prime = Utilities.GetHigherPrime(pixelsSize);
            if (prime < 0) throw new Exception("Couldn't find a higher prime");

            var i = prime % pixelsSize;
            
            Debug.LogWarning("Pixels Size: " + pixelsSize);

            for (var j = 1; j <= pixelsSize; j++)
            {
                var pixelType = pixels[i].GetMapType();
                
                Debug.LogWarning("Get Pixel " + i + " type: " + pixelType + " color: " + pixels[i]);

                if (pixelType == MapTypes.WATER && !containsWater)
                {
                    Debug.LogWarning("----- FOUND WATER on " + (float)j /pixelsSize * 100 + "% -----");
                    containsWater = true;
                }
                else if (pixelType != MapTypes.WATER && !containsLand)
                {
                    Debug.LogWarning("----- FOUND LAND on " + (float)j /pixelsSize * 100 + "% -----");
                    containsLand = true;
                }

                if (containsWater && containsLand)
                {
                    Debug.LogWarning("----- STOP FOUND BOTH WATER AND LAND on " + (float)j /pixelsSize * 100 + "% -----");
                    break;
                }

                i = (i + prime) % pixelsSize;
            }

            if (containsLand && containsWater && Width > 2)
                MapType = MapTypes.UNKNOWN;
            else if (containsWater && containsLand && Width <= 2 || !containsWater)
                MapType = MapTypes.GROUND;
            else
                MapType = MapTypes.WATER;
            
            QuadtreeManager.Instance.RegisterNewNode(this);

            return MapType;
        }

        #endregion
    }
}
