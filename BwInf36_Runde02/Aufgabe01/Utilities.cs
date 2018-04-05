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
        /// Ueberprueft ob zwei Reihen kompatibel sind
        /// </summary>
        /// <param name="a">Die erste Reihe</param>
        /// <param name="b">Die zweite Reihe</param>
        /// <param name="matrix">Die bisher gebildete Reihen Matrix</param>
        /// <returns>True wenn die Reihen kompatibel sind</returns>
        public static bool ReihenSindKompatibel(Reihe a, Reihe b, ref byte[][] matrix)
        {
            if (!a.IsInitialized()) return true;                        // Weil die Mauer ja direkt die maximal Hoehe hat

            var indexA = a.Id;
            var indexB = b.Id;

            if (matrix[indexA] != null && matrix[indexA][indexB] != 0)  // Bereits eingetragen
                return matrix[indexA][indexB] == 2;
            if (matrix[indexB] != null && matrix[indexB][indexA] != 0)  // Bereits eingetragen (gespiegelt)
                return matrix[indexB][indexA] == 2;

            // Noch nicht eingetragen
            if (matrix[indexA] == null)
                matrix[indexA] = new byte[matrix.Length];

            // Reihen checken
            var fugenUeberlappungen = a.BesetzteFugen.Intersect(b.BesetzteFugen);
            var kompatibel = !fugenUeberlappungen.Any();

            // Reihen eintragen
            if (kompatibel) matrix[indexA][indexB] = 2;
            else matrix[indexA][indexB] = 1;

            return kompatibel;
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
                return items.SelectMany(item => GetPermutations(items.Where(i => !i.Equals(item)).ToArray()).ToArray(),
                                       (item, permutation) => new[] { item }.Concat(permutation).ToArray()).ToArray();
            }
            else
            {
                return new[] { items }.ToArray();
            }
        }
    }
}