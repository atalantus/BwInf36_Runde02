using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Aufgabe01
{
    public class Reihe : IComparable
    {
        #region Properties

        public int ReihenIndex { get; private set; }

        /// <summary>
        /// Index des zuletzt gesetzten Klotzes
        /// </summary>
        public int LastKlotzIndex { get; private set; } = -1;

        /// <summary>
        /// Enthaelt die noch verfuegbaren Zahlen in der Reihe |
        /// Index - 1 = Zahl;
        /// True = Noch verfuegbar
        /// </summary>
        public bool[] Nummern { get; }

        /// <summary>
        /// Enthaelt die Kloetze in dieser Reihe
        /// </summary>
        public int[] Kloetze { get; }

        /// <summary>
        /// Die RowSum der gesamten Reihe
        /// </summary>
        public int WholeRowSum => GetRowSum(Kloetze.Length);

        #endregion

        #region Methods

        public Reihe(int n)
        {
            Nummern = new bool[n];
            Nummern.FillArray(true);
            Kloetze = new int[n];
            Kloetze.FillArray(0);
        }

        public void SetKlotz(int x, int nummer)
        {
            Kloetze[x] = nummer;
            LastKlotzIndex = x;
            SetNummer(nummer);
        }

        private void SetNummer(int number)
        {
            Nummern[number - 1] = false;
        }

        public bool GetNummer(int nummer)
        {
            return Nummern[nummer - 1];
        }

        public int GetRowSum(int x)
        {
            var rowSum = 0;
            for (var i = 0; i < x; i++)
            {
                rowSum += Kloetze[i];
            }

            return rowSum;
        }

        #endregion

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Reihe otherReihe)
            {
                if (WholeRowSum > otherReihe.WholeRowSum) return 1;
                return WholeRowSum == otherReihe.WholeRowSum ? 0 : -1;
            }

            return 1;
        }
    }
}