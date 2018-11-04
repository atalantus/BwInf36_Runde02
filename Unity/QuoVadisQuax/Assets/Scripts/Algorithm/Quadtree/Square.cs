﻿using UnityEngine;

namespace Algorithm.Quadtree
{
    /// <summary>
    ///     Defines a Square
    /// </summary>
    public class Square
    {
        #region Properties

        /// <summary>
        ///     The South-West (Bottom-Left) point of the square
        /// </summary>
        public Vector2Int SW_Point { get; private set; }

        /// <summary>
        ///     The North-East (Top-Right) point of the square
        /// </summary>
        public Vector2Int NE_Point { get; private set; }

        /// <summary>
        ///     The width of the square
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        ///     The height of the square
        /// </summary>
        public int Height
        {
            get { return Width; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Instantiates a new <see cref="Square" /> object
        /// </summary>
        /// <param name="swPoint">The South-West (Bottom-Left) point</param>
        /// <param name="width">The width of the square</param>
        public Square(Vector2Int swPoint, int width)
        {
            SW_Point = swPoint;
            Width = width;
            NE_Point = new Vector2Int(SW_Point.x + Width - 1, Mathf.Abs(SW_Point.y + Height - 1));
        }

        /// <summary>
        ///     Checks if this Square touches a <see cref="Vector2Int" />
        /// </summary>
        /// <param name="other">The <see cref="Vector2Int" /></param>
        /// <returns>True if this Square touches the <see cref="Vector2Int" /></returns>
        public bool ContainsPoint(Vector2Int other)
        {
            return !(other.x > NE_Point.x || other.y > NE_Point.y ||
                     other.x < SW_Point.x || other.y < SW_Point.y);
        }

        #endregion
    }
}