using System;
using System.Collections.Generic;
using System.Text;

namespace Aufgabe1_DieKunstDerFuge
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a row.
    /// </summary>
    public class Row : IComparable<Row>
    {
        #region Properties

        /// <summary>
        /// All the Bricks in the row.
        /// Length of brick is determined by index + 1.
        /// True if the brick is still available.
        /// </summary>
        public bool[] Bricks { get; set; }

        /// <summary>
        /// The placed bricks in this row (ordered).
        /// </summary>
        public int[] PlacedBricks { get; set; }

        /// <summary>
        /// The current index of <see cref="PlacedBricks"/>.
        /// </summary>
        public int PlacedBricksIndex { get; private set; }

        /// <summary>
        /// The current length of this row.
        /// </summary>
        public int RowSum { get; set; }

        /// <summary>
        /// Contains all the possible RowSums after placing another brick.
        /// </summary>
        public List<NextPossibleRowSum> NextPossibleRowSums { get; set; }

        /// <summary>
        /// The index of the next brick to place to fill the current searched gap.
        /// </summary>
        public int NextBrickToPlace { get; set; }

        /// <summary>
        /// The cached last <see cref="NextPossibleRowSums"/>.
        /// </summary>
        private List<NextPossibleRowSum> _lastPossibleRowSums;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor.
        /// </summary>
        public Row()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="bricksPerRow">Number of bricks per row.</param>
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
        /// Places the <see cref="NextBrickToPlace"/> brick in the row.
        /// </summary>
        public void PlaceNextBrick()
        {
            Bricks[NextBrickToPlace] = false;
            RowSum += NextBrickToPlace + 1;
            PlacedBricks[PlacedBricksIndex] = NextBrickToPlace + 1;
            PlacedBricksIndex++;

            // Find NextPossibleRowSums
            _lastPossibleRowSums = new List<NextPossibleRowSum>(NextPossibleRowSums);
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
        /// Removes the last brick of the row.
        /// </summary>
        public void RemoveLastBrick()
        {
            PlacedBricksIndex--;
            PlacedBricks[PlacedBricksIndex] = 0;

            Bricks[NextBrickToPlace] = true;
            RowSum -= NextBrickToPlace + 1;

            NextPossibleRowSums = _lastPossibleRowSums;
        }

        /// <summary>
        /// Clones the <see cref="Row"/>.
        /// </summary>
        /// <returns>The cloned row instance.</returns>
        public Row Clone()
        {
            var rowClone = new Row {Bricks = new bool[Bricks.Length]};
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
    }
}