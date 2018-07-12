using System;
using System.Diagnostics;
using System.Linq;

namespace Aufgabe01_LR_Praesentation
{
    /// <summary>
    /// Baut die Mauer
    /// </summary>
    public class WallBuilder
    {
        #region Attribute

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

        #region Methoden

        public void BuildWall(int n)
        {
            AlgorithmStopwatch = new Stopwatch();
            AlgorithmStopwatch.Start();

            BricksPerRow = n;
            CalculateWallProperties();
            PrintWallProperties();

            var wall = new Wall(WallHeight, BricksPerRow);

            // Starte den Mauer-Bau!
            var buildWall = FillNextGap(0, wall, FreeGaps);

            AlgorithmStopwatch.Stop();
            PrintAlgorithmTime();

            if (buildWall == null) throw new Exception("Failed to build a wall");

            PrintWall(buildWall);
        }

        /// <summary>
        /// Versucht die naechste Luecke in der Mauer zu fuellen
        /// </summary>
        /// <param name="nextGap">Die Luecke, die als naechstes gefuellt werden muss</param>
        /// <param name="curWall">Die aktuelle Mauer</param>
        /// <param name="freeGaps">Nicht wichtig!</param>
        /// <returns>Das gueltige Mauer Objekt oder null</returns>
        public Wall FillNextGap(int nextGap, Wall curWall, int freeGaps)
        {
            // Ueberpruefe ob die Mauer fertig gebaut ist
            if (curWall.Rows.All(r => r.RowSum == WallLength))
            {
                return curWall;
            }
            #region -
            Wall wall = curWall.Clone();

            // Gibt es eine naechste Spalte
            int nextGapPos;
            if (nextGap < GapCount + 1)
                nextGapPos = nextGap + 1;
            else
                nextGapPos = nextGap;
            #endregion

            // Sammle alle Reihen, die die naechste Spalte fuellen koennen
            Row[] possibleRows = wall.Rows.Where(r => r.NextPossibleRowSums.Any(nrs => 
                ContainsPossibleRowSum(r, nrs, nextGapPos))).ToArray();

            if (possibleRows.Length == 0)
            {
                // Es gibt keine Reihe, die die naechste Spalte fuellen kann
                #region -
                if (freeGaps > 0)
                {
                    var result = FillNextGap(nextGapPos, wall, freeGaps - 1);
                    if (result != null) return result;
                }
                else
                {
                    #endregion

                // ZURUECK
                return null;
                #region -

                    

                   
                }
                #endregion
            }
            #region -

            // Get row with lowest row sum and call FillNextGap()
            Array.Sort(possibleRows);

            #endregion

       

            // BACKTRACKING
            for (int i = 0; i < possibleRows.Length; i++)
            {
                // Fuelle die naechste Spalte
                possibleRows[i].PlaceNextBrick();

                // Neuer Methodenaufruf: Versuche die wieder naechste Spalte zu fuellen
                Wall result = FillNextGap(nextGapPos, wall, freeGaps);

                if (result != null)
                {
                    // Wir haben eine gueltige Mauer!
                    return result;
                }
                else
                {
                    // Falsche Entscheidung!

                    // Den gesetzten Klotz wieder entfernen und...
                    possibleRows[i].RemoveLastBrick();

                    // ...die naechste Moeglichkeit ausprobieren
                }
            }

            // ZURUECK
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