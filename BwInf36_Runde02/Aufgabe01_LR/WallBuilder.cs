using System;
using System.Linq;
using System.Text;

namespace Aufgabe01_LR
{
    /// <summary>
    /// Builds the Wall.
    /// </summary>
    public class WallBuilder
    {
        #region Fields

        /// <summary>
        /// All the Gaps in the Wall.
        /// True if the Gap is still available.
        /// </summary>
        public bool[] Gaps { get; set; }

        /// <summary>
        /// The maximum count of rows in the <see cref="Wall"/>
        /// </summary>
        public int WallHeight { get; set; }

        /// <summary>
        /// The Userinput N
        /// </summary>
        public int BricksPerRow { get; set; }

        /// <summary>
        /// The maximum length of the <see cref="Wall"/>
        /// </summary>
        public int WallLength { get; set; }

        /// <summary>
        /// The number of gaps inside the wall
        /// </summary>
        public int GapCount { get; set; }

        /// <summary>
        /// The number of gaps that will be used
        /// </summary>
        public int UsedGapCount { get; set; }

        /// <summary>
        /// The number of gaps that won't be used
        /// </summary>
        public int FreeGaps { get; set; }

        #endregion

        #region Methods

        public void BuildWall(int n)
        {
            BricksPerRow = n;
            CalculateWallProperties();
            PrintWallProperties();

            var wall = new Wall(WallHeight, BricksPerRow);

            var buildWall = FillNextGap(0, wall, FreeGaps);

            if (buildWall == null) throw new Exception("Build wall is NULL");
        }

        public Wall FillNextGap(int nextGap, Wall curWall, int freeGaps)
        {
            var wall = curWall.Clone();

            // Get all rows that can reach the next gap
            var nextGapPos = nextGap + 1;
            var possibleRows = wall.Rows.Where(r => r.NextLowestRowSum == nextGapPos).ToArray();

            // If no row can reach next gap and FreeGaps > 0 continue to next gap
            if (possibleRows.Length == 0)
            {
                if (freeGaps > 0)
                {
                    var result = FillNextGap(nextGap + 1, wall, freeGaps - 1);
                    if (result != null) return result;
                }
                else
                {
                    // track back
                    return null;
                }
            }

            // Get row with lowest row sum and call FillNextGap()
            Array.Sort(possibleRows);

            // Backtracking
            for (var i = 0; i < possibleRows.Length; i++)
            {
                possibleRows[i].PlaceShortestBrick();
                var result = FillNextGap(nextGap + 1, wall, freeGaps);
                if (result != null) return result;
                
                // try next branch
            }

            // This should never happen
            throw new Exception("Couldn't find a solution");
        }

        /// <summary>
        /// Calculates the properties of the wall
        /// </summary>
        private void CalculateWallProperties()
        {
            WallLength = (int)((Math.Pow(BricksPerRow, 2) + BricksPerRow) / 2); // Gausssche Summenformel
            GapCount = WallLength - 1;
            WallHeight = GapCount / (BricksPerRow - 1);
            UsedGapCount = (BricksPerRow - 1) * WallHeight;
            FreeGaps = GapCount - UsedGapCount;
        }

        /// <summary>
        /// Prints the properties of the wall to the console
        /// </summary>
        private void PrintWallProperties()
        {
            Console.WriteLine();
            Console.WriteLine($"Anzahl Kloetzchen in einer Reihe: {BricksPerRow}");
            Console.WriteLine($"Breite der Mauer: {WallLength}");
            Console.WriteLine($"Maximale Hoehe der Mauer: {WallHeight}");
            Console.WriteLine($"Anzahl verfuegbarer Stellen fuer Fugen: {GapCount}");
            Console.WriteLine($"Anzahl benoetigter Fugen fuer Mauer der maximalen Hoehe: {UsedGapCount}");
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Prints a wall to the console
        /// </summary>
        /// <param name="wall">The wall to print</param>
        private void PrintWall(Wall wall)
        {
            for (var i = 0; i < wall.Rows.Length; i++)
            {
                var stringBuilder = new StringBuilder();
                for (int j = 0; j < wall.Rows[i].Bricks.Length; j++)
                {
                    
                }
            }
        }

        #endregion
    }
}