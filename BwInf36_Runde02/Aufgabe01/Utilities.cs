﻿using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static Aufgabe01.WallBuilder;

namespace Aufgabe01
{
    /// <summary>
    /// Hilf Methoden fuer Aufgabe 1
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Erstellt eine neue Mauer aus der <paramref name="first"/> Mauer und der <paramref name="second"/> Mauer
        /// </summary>
        /// <param name="first">Die erste Mauer</param>
        /// <param name="second">Die zweite Mauer</param>
        /// <returns></returns>
        public static Mauer MergeMauer(Mauer first, Mauer second)
        {
            Mauer newMauer = new Mauer(first.Reihen.Length, first.FreieFugen.Length);
            Mauer[] oldMauern = new Mauer[] { first, second };

            for (int n = 0; n < oldMauern.Length; n++)
            {
                for (int i = 0; i < newMauer.Reihen.Length; i++)
                {
                    if (!newMauer.Reihen[i].IsInitialized()) newMauer.Reihen[i] = oldMauern[n].Reihen[i];
                }
            }
           
            return newMauer;
        }

        public static bool ReihenSindKompatibel(Reihe a, int aIndex, Reihe b, int bIndex, int[,] matrix, List<Reihe> reihen, bool[,] bereitsKombiniertMatrix)
        {
            if (bereitsKombiniertMatrix[aIndex, bIndex]) return false; // Reihen Bereits kombiniert
            //if (a == null) return true; // Weil die Mauer ja direkt die maximal Hoehe hat
            return matrix[reihen.IndexOf(a), reihen.IndexOf(b)] == 1;
        }

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
            for (int i = 1; i <= n; i++)
            {
                value *= i;
            }

            return value;
        }

        /// <summary>
        /// Ueberprueft, ob eine Zahl gerade oder ungerade ist
        /// </summary>
        /// <param name="n">Die zu ueberpruefende Zahl</param>
        /// <returns>True wenn die Zahl gerade ist</returns>
        public static bool IsNumberEven(int n)
        {
            return n % 2 == 0;
        }

        /// <summary>
        /// Fuellt ein Array mit einem Wert
        /// </summary>
        /// <typeparam name="T">Der Typ des Arrays</typeparam>
        /// <param name="array">Das Array das gefuellt wird</param>
        /// <param name="value">Der Wert mit dem das Array gefuellt werden soll</param>
        public static void FillArray<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
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
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    array[x, y] = value;
                }
            }
        }
    }
}