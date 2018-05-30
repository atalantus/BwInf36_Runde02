using System;

namespace Aufgabe01_LR
{
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
        /// The current length of this row
        /// </summary>
        public int RowSum { get; set; }

        /// <summary>
        /// The RowSum if the lowest available brick is used next
        /// </summary>
        public int NextLowestRowSum { get; set; }

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
            RowSum = 0;
            NextLowestRowSum = 1;
        }

        /// <summary>
        /// Places the shortest available brick in the row
        /// </summary>
        public void PlaceShortestBrick()
        {
            int i;
            for (i = 0; i < Bricks.Length; i++)
            {
                if (Bricks[i])
                {
                    Bricks[i] = false;
                    RowSum += i + 1;
                    NextLowestRowSum = RowSum;
                    break;
                }
            }

            // Find NextLowestRowSum
            for (var j = i + 1; j < Bricks.Length; j++)
            {
                if (Bricks[j])
                {
                    NextLowestRowSum += j + 1;
                    break;
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
            rowClone.RowSum = RowSum;
            return rowClone;
        }

        /// <summary>
        /// Calculates <see cref="NextLowestRowSum"/>
        /// </summary>
        public void CalcNextLowestRowSum()
        {
            for (var i = 0; i < Bricks.Length; i++)
            {
                if (Bricks[i])
                {
                    NextLowestRowSum += i + 1;
                    return;
                }
            }
        }

        public int CompareTo(Row other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return RowSum.CompareTo(other.RowSum);
        }

        public override string ToString()
        {

        }

        #endregion
    }
}