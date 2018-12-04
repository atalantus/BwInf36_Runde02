using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
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
            if (a == null) return true;                        // Weil die Mauer ja direkt die maximal Hoehe hat

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
        /// Heaps Algorithmus zur Findung aller Permutationen
        /// </summary>
        /// <param name="items">Elemente deren Permutationen gefunden werden sollen</param>
        /// <param name="naechstePermutation">Returned true wenn die naechste Permutation gefunden werden soll</param>
        /// <returns>Returned false wenn nicht alle Permutationen gefunden wurden</returns> 
        public static bool SammlePermutationen(byte[] items, Func<byte[], bool> naechstePermutation)
        {
            var countOfItem = items.Length;

            if (countOfItem <= 1)
            {
                return naechstePermutation(items);
            }

            var indexes = new int[countOfItem];
            for (var i = 0; i < countOfItem; i++)
            {
                indexes[i] = 0;
            }

            if (!naechstePermutation(items))
            {
                return false;
            }

            for (var i = 1; i < countOfItem;)
            {
                if (indexes[i] < i)
                {
                    if ((i & 1) == 1)
                    {
                        Tausch(ref items[i], ref items[indexes[i]]);
                    }
                    else
                    {
                        Tausch(ref items[i], ref items[0]);
                    }

                    if (!naechstePermutation(items))
                    {
                        return false;
                    }

                    indexes[i]++;
                    i = 1;
                }
                else
                {
                    indexes[i++] = 0;
                }
            }

            return true;
        }

        /// <summary>
        /// Tauscht 2 Elemente des selben Typs
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private static void Tausch(ref byte a, ref byte b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}