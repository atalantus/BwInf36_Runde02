using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe1_DieKunstDerFuge
{
    /// <summary>
    /// Execution class of the program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point for execution.
        /// </summary>
        /// <param name="args">Arguments.</param>
        private static void Main(string[] args)
        {
            /**
             * WallBuilder
             */
            var wb = new WallBuilder();


            /**
             * Header
             */
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("BwInf36 | Runde 2 | Aufgabe 1 (Die Kunst der Fuge)");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.ResetColor();


            /**
             * Input: Number of bricks per row
             */
            Console.Write("Anzahl der Kloetzchen in einer Reihe: ");
            int.TryParse(Console.ReadLine(), out var bricksPerRow);
            Console.ForegroundColor = ConsoleColor.Red;
            while (bricksPerRow <= 1)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Die Anzahl der Kloetze muss zwischen 2 (eingeschlossen) und 22 (eingeschlossen) liegen!");
                Console.Write("Bitte waehlen Sie eine andere Anzahl von Kloetzchen in einer Reihe: ");
                int.TryParse(Console.ReadLine(), out bricksPerRow);
            }

            Console.ResetColor();
            Console.WriteLine();


            /**
             * Algorithm
             */
            var end = false;
            while (!end)
            {
                try
                {
                    wb.BuildWall(bricksPerRow);
                    end = true;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("ERROR:");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine();
                    Console.ResetColor();

                    end = true;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Druecke ENTER um das Programm zu beenden");
            Console.ReadLine();
        }
    }
}