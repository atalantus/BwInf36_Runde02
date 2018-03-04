using System.Numerics;

namespace Aufgabe01
{
    /// <summary>
    /// Extra Methoden fuer Aufgabe 1
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Berechnet die Fakultaet einer Zahl
        /// </summary>
        /// 
        /// Keine Rekursion wegen StackOverflowException bei zu grossen Zahlen!
        /// 
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