using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

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
        /// Erstellt eine neue Mauer aus der <paramref name="first"/> Mauer und der <paramref name="second"/> Mauer
        /// </summary>
        /// <param name="first">Die erste Mauer</param>
        /// <param name="second">Die zweite Mauer</param>
        /// <returns>Ein neues Mauer Objekt, dass die Reihen der <paramref name="first"/> Mauer und der <paramref name="second"/> Mauer enthaelt</returns>
        public static Mauer MergeMauer(Mauer first, Mauer second)
        {
            var newMauer = new Mauer(first.Reihen.Length, first.FreieFugen.Length);
            var oldMauern = new[] { first, second };

            for (var n = 0; n < oldMauern.Length; n++)
            {
                for (var i = 0; i < newMauer.Reihen.Length; i++)
                {
                    if (!newMauer.Reihen[i].IsInitialized()) newMauer.AddReihe(oldMauern[n].Reihen[i]);
                }
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
        public static bool ReihenSindKompatibel(Reihe a, Reihe b, byte[,] matrix, List<Reihe> reihen)
        {
            if (!a.IsInitialized()) return true;                        // Weil die Mauer ja direkt die maximal Hoehe hat
            return matrix[reihen.IndexOf(a), reihen.IndexOf(b)] == 2;
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
    }
}