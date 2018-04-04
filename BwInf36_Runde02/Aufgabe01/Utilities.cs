using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Aufgabe01
{
    /// <summary>
    /// Helfer Methoden fuer Aufgabe 1
    /// </summary>
    public static class Utilities
    {

        /// <summary>
        /// Display the timer frequency and resolution
        /// </summary>
        public static void DisplayTimerProperties()
        {
            Console.WriteLine();
            Console.WriteLine(Stopwatch.IsHighResolution
                ? "Operations timed using the system's high-resolution performance counter."
                : "Operations timed using the DateTime class.");

            var frequency = Stopwatch.Frequency;
            Console.WriteLine("  Timer frequency in ticks per second = {0}",
                frequency);
            var nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
            Console.WriteLine("  Timer is accurate within {0} nanoseconds",
                nanosecPerTick);
            Console.WriteLine();
        }

        /// <summary>
        /// Baut eine neue Mauer aus <paramref name="mauer"/> und <paramref name="reihe"/>
        /// </summary>
        /// <param name="mauer">Die alte Mauer, die beinhaltet sein soll</param>
        /// <param name="reihe">Die Reihe, die beinhaltet sein soll</param>
        /// <returns>Ein NEUES Mauer Objekt</returns>
        public static Mauer MergeMauerWithRow(Mauer mauer, Reihe reihe)
        {
            var newMauer = new Mauer(mauer.Reihen.Length, mauer.FreieFugen.Length);

            var reihen = new List<Reihe>();
            reihen.AddRange(mauer.Reihen);
            reihen.Add(reihe);

            for (var i = 0; i < reihen.Count; i++)
            {
                if (reihen[i].IsInitialized()) newMauer.AddReihe(reihen[i]);
            }

            return newMauer;
        }

        /// <summary>
        /// Ueberprueft, ob die Reihen <paramref name="a"/> und <paramref name="b"/> kompatibel sind.
        /// </summary>
        /// <param name="a">Die erste Reihe</param>
        /// <param name="b">Die zweite Reihe</param>
        /// <param name="matrix">Die gesamt Matrix aller einzelnen Reihen</param>
        /// <param name="reihen">Die gesamt Liste aller einzelnen Reihen</param>
        /// <returns>True wenn die Reihen kompatibel sind</returns>
        public static bool ReihenSindKompatibel(Reihe a, Reihe b, byte[][] matrix, List<Reihe> reihen)
        {
            if (!a.IsInitialized()) return true;                        // Weil die Mauer ja direkt die maximal Hoehe hat
            return matrix[reihen.IndexOf(a)][reihen.IndexOf(b)] == 2;
        }

        /// <summary>
        /// Ueberprueft, ob die Mauer aus <paramref name="mauer"/> und <paramref name="neueReihe"/> bereits existiert.
        /// </summary>
        /// <param name="mauer">Die aktuelle Mauer</param>
        /// <param name="neueReihe">Die Reihe die zur <paramref name="mauer"/> hinzugefuegt werden soll</param>
        /// <param name="bisherigeMauern">Die Liste von bisher gebauten Mauern</param>
        /// <returns></returns>
        public static bool MauerIstNeu(Mauer mauer, Reihe neueReihe, List<Mauer> bisherigeMauern)
        {
            var rowIds = mauer.ReihenIds.Copy();
            rowIds.Add(neueReihe.Id);
            rowIds.Sort();

            return !bisherigeMauern.Exists(m => m.Id == rowIds.Print());
        }

        /// <summary>
        /// Berechnet alle Permutationen der Elemente in einer Collection
        /// </summary>
        /// <typeparam name="T">Der Typ der Collection</typeparam>
        /// <param name="items">Die Collection, deren Elemente permutiert werden sollen</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items)
        {
            if (items.Count() > 1)
            {
                return items.SelectMany(item => GetPermutations(items.Where(i => !i.Equals(item))),
                                       (item, permutation) => new[] { item }.Concat(permutation));
            }
            else
            {
                return new[] { items };
            }
        }

        /// <summary>
        /// Berechnet die Fakultaet einer Zahl
        /// </summary>
        /// <param name="n">Der Wert von dem die Fakultaet berechnet werden soll</param>
        /// <returns>Die Fakultaet</returns>
        public static BigInteger FakultaetBerechnen(int n)
        {
            if (n == 0) return 1;
            BigInteger value = 1;
            for (var i = 1; i <= n; i++)
            {
                value *= i;
            }

            return value;
        }

        /// <summary>
        /// Fuellt ein Array mit einem Wert
        /// </summary>
        /// <typeparam name="T">Der Typ des Arrays</typeparam>
        /// <param name="array">Das Array das gefuellt wird</param>
        /// <param name="value">Der Wert mit dem das Array gefuellt werden soll</param>
        public static void FillArray<T>(this T[] array, T value)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /// <summary>
        /// Fuellt ein zweidimensionales Array mit einem Wert
        /// </summary>
        /// <typeparam name="T">Der Typ des Arrays</typeparam>
        /// <param name="array">Das Array das gefuellt wird</param>
        /// <param name="value">Der Wert mit dem das Array gefuellt werden soll</param>
        public static void Fill2DArray<T>(this T[,] array, T value)
        {
            for (var x = 0; x < array.GetLength(0); x++)
            {
                for (var y = 0; y < array.GetLength(1); y++)
                {
                    array[x, y] = value;
                }
            }
        }

        /// <summary>
        /// Copies a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>The copied list</returns>
        public static List<T> Copy<T>(this List<T> list)
        {
            var copied = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                copied.Add(list[i]);
            }

            return copied;
        }

        /// <summary>
        /// Returns a string with all the elements of the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>All the elements in the list in one string</returns>
        public static string Print<T>(this List<T> list)
        {
            var output = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                output.Append(list[i].ToString());
            }

            return output.ToString();
        }
    }
}