using System;
using System.Collections.Generic;
using System.Text;

namespace Aufgabe01_LR
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a row
    /// </summary>
    public class Row : IComparable<Row>
    {
        #region Fields

        /// <summary>
        /// All the Bricks in the row.
        /// Length of brick is determined by index + 1.
        /// True if the brick is still available.
        /// </summary>
        public bool[] Bricks { get; set; }

        /// <summary>
        /// The placed bricks in this row (ordered)
        /// </summary>
        public int[] PlacedBricks { get; set; }

        /// <summary>
        /// The current index of <see cref="PlacedBricks"/>
        /// </summary>
        public int PlacedBricksIndex { get; private set; }

        /// <summary>
        /// The current length of this row
        /// </summary>
        public int RowSum { get; set; }

        /// <summary>
        /// Contains all the possible RowSums after placing another brick
        /// </summary>
        public List<NextPossibleRowSum> NextPossibleRowSums { get; set; }

        /// <summary>
        /// The index of the next brick to place to fill the currend searched gap
        /// </summary>
        public int NextBrickToPlace { get; set; }

        #endregion

        #region Methods

        private Row()
        {
        
        }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="bricksPerRow">Number of bricks per row</param>
        public Row(int bricksPerRow)
        {
            Bricks = new bool[bricksPerRow];
            Bricks.FillArray(true);
            PlacedBricks = new int[bricksPerRow];
            PlacedBricksIndex = 0;
            RowSum = 0;
            NextPossibleRowSums = new List<NextPossibleRowSum>(bricksPerRow);
            for (var i = 0; i < bricksPerRow; i++)
            {
                NextPossibleRowSums.Add(new NextPossibleRowSum(i + 1, i));
            }
        }

        /// <summary>
        /// Places the <see cref="NextBrickToPlace"/> brick in the row
        /// </summary>
        public void PlaceNextBrick()
        {
            Bricks[NextBrickToPlace] = false;
            RowSum += NextBrickToPlace + 1;
            PlacedBricks[PlacedBricksIndex] = NextBrickToPlace + 1;
            PlacedBricksIndex++;

            // Find NextPossibleRowSums
            // TODO: Improve performance
            NextPossibleRowSums.Clear();
            for (var j = 0; j < Bricks.Length; j++)
            {
                if (Bricks[j])
                {
                    NextPossibleRowSums.Add(new NextPossibleRowSum(RowSum + j + 1, j));
                }
            }
        }

        /// <summary>
        /// Clones the <see cref="Row"/>
        /// </summary>
        /// <returns>The cloned row instance</returns>
        public Row Clone()
        {
            var rowClone = new Row { Bricks = new bool[Bricks.Length] };
            Bricks.CopyTo(rowClone.Bricks, 0);
            rowClone.PlacedBricks = new int[Bricks.Length];
            PlacedBricks.CopyTo(rowClone.PlacedBricks, 0);
            rowClone.PlacedBricksIndex = PlacedBricksIndex;
            rowClone.RowSum = RowSum;
            rowClone.NextPossibleRowSums = new List<NextPossibleRowSum>(NextPossibleRowSums);
            rowClone.NextBrickToPlace = NextBrickToPlace;
            return rowClone;
        }

        public int CompareTo(Row other)
        {
            if (ReferenceEquals(this, other)) return 0;
            return other is null ? 1 : RowSum.CompareTo(other.RowSum);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("|");
            for (var i = 0; i < PlacedBricks.Length; i++)
            {
                sb.Append($" {PlacedBricks[i]} |");
            }

            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// Represents a NextPossibleRowSum type
        /// </summary>
        public struct NextPossibleRowSum
        {
            /// <summary>
            /// The next possible row sum
            /// </summary>
            public int PossibleRowSum { get; set; }

            /// <summary>
            /// The block that needs to be placed next for this <see cref="PossibleRowSum"/>
            /// </summary>
            public int UsedBrickIndex { get; set; }

            public NextPossibleRowSum(int possibleRowSum, int usedBrickIndex)
            {
                PossibleRowSum = possibleRowSum;
                UsedBrickIndex = usedBrickIndex;
            }
        }
    }
}