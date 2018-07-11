using System;
using System.Diagnostics;
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

        /// <summary>
        /// Stopwatch for messuring the algorithm execution time
        /// </summary>
        public Stopwatch AlgorithmStopwatch { get; set; }

        #endregion

        #region Methods

        public void BuildWall(int n)
        {
            AlgorithmStopwatch = new Stopwatch();
            AlgorithmStopwatch.Start();

            BricksPerRow = n;
            CalculateWallProperties();
            PrintWallProperties();

            var wall = new Wall(WallHeight, BricksPerRow);

            var buildWall = FillNextGap(0, wall, FreeGaps);

            AlgorithmStopwatch.Stop();
            PrintAlgorithmTime();

            if (buildWall == null) throw new Exception("Failed to build a wall");

            PrintWall(buildWall);
        }

        public Wall FillNextGap(int nextGap, Wall curWall, int freeGaps)
        {
            // Check if wall is finished
            if (curWall.Rows.All(r => r.RowSum == WallLength))
                return curWall;

            var wall = curWall.Clone();

            // Check if there is a next gap
            int nextGapPos;
            if (nextGap < GapCount + 1)
                nextGapPos = nextGap + 1;
            else
                nextGapPos = nextGap;

            // Get all rows that can reach the next gap
            var possibleRows = wall.Rows.Where(r => r.NextPossibleRowSums.Any(nrs => ContainsPossibleRowSum(r, nrs, nextGapPos))).ToArray();

            // If no row can reach next gap and FreeGaps > 0 continue to next gap
            if (possibleRows.Length == 0)
            {
                if (freeGaps > 0)
                {
                    var result = FillNextGap(nextGapPos, wall, freeGaps - 1);
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
                possibleRows[i].PlaceNextBrick();
                var result = FillNextGap(nextGapPos, wall, freeGaps);
                if (result != null) return result;

                // try next branch

                // remove wrong placed brick
                possibleRows[i].RemoveLastBrick();
            }

            // This should never happen
            //throw new Exception("Couldn't find a solution");
            return null;
        }

        /// <summary>
        /// Checks whetever a <see cref="NextPossibleRowSum"/> fits the next open gap
        /// </summary>
        /// <param name="row">The row that contains the <param name="nprs"></param></param>
        /// <param name="nprs">The current <see cref="NextPossibleRowSum"/> to check</param>
        /// <param name="nextGapPos">The required gap position</param>
        /// <returns>True if the <param name="nprs"></param> and <param name="nextGapPos"></param> matches</returns>
        private bool ContainsPossibleRowSum(Row row, NextPossibleRowSum nprs, int nextGapPos)
        {
            if (nprs.PossibleRowSum == nextGapPos)
            {
                row.NextBrickToPlace = nprs.UsedBrickIndex;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculates the properties of the wall
        /// </summary>
        public void CalculateWallProperties()
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (var i = 0; i < wall.Rows.Length; i++)
            {
                Console.WriteLine($"    {wall.Rows[i]}");
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        /// <summary>
        /// Prints the execution time of the algorithm
        /// </summary>
        private void PrintAlgorithmTime()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"    The algorithm took {AlgorithmStopwatch.ElapsedMilliseconds}ms to complete.");
            Console.WriteLine();
            Console.ResetColor();
        }

        #endregion
    }
}