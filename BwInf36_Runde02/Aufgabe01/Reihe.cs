using System;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aufgabe01
{
    public class Reihe : IComparable
    {
        #region Properties

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

        /// <summary>
        /// Die Mauer der Reihe
        /// </summary>
        public Mauer Mauer { get; private set; }

        #endregion

        #region Methods

        public Reihe(Mauer mauer, int n)
        {
            Mauer = mauer;
            Nummern = new bool[n];
            Nummern.FillArray(true);
            Kloetze = new int[n];
            //Kloetze.FillArray(0);
            Kloetze.FillArray(1); // HACK: Fuer BruteForce
        }

        /// <summary>
        /// HACK: Konstruktor um eine Reihe zu kopieren
        /// Fuer BruteForce
        /// </summary>
        /// <param name="klotze"></param>
        public Reihe(Mauer mauer, int[] klotze, int breite)
        {
            Mauer = mauer;
            Nummern = new bool[klotze.Length];
            Nummern.FillArray(true);
            Kloetze = new int[klotze.Length];
            for (int i = 0; i < klotze.Length; i++)
            {
                Kloetze[i] = klotze[i];
                if (!Mauer.FreieFugen[GetRowSum(i) + klotze[i] - 1])
                    throw new FugenUeberlappungException("lul die fugen ueberlappen sich.");
                Mauer.FreieFugen[GetRowSum(i) + klotze[i] - 1] = false;
            }
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

        public override string ToString()
        {
            var reihe = new StringBuilder("|");
            foreach (var klotz in Kloetze)
            {
                reihe.Append($"{klotz}|");
            }
            return reihe.ToString();
        }
    }
}