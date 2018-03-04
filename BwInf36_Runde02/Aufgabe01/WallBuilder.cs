using System;
using System.Diagnostics;
using System.Linq;

namespace Aufgabe01
{
    /// <summary>
    /// Die Hauptklasse fuer den Algorithmus
    /// </summary>
    public class WallBuilder
    {
        #region Fields

        private int _anzahlKloetze;
        private bool _isWorking;

        #endregion

        #region Properties

        /// <summary>
        /// Die aktuelle Mauer
        /// </summary>
        public int[,] Mauer { get; set; }

        /// <summary>
        /// Enthaelt die bisherigen Spalten, inklusive Anfang und Ende
        /// </summary>
        public bool[] Spalten { get; set; }

        /// <summary>
        /// Die Anzahl von Kloetze in einer Reihe
        /// </summary>
        public int AnzahlKloetze
        {
            get => _anzahlKloetze;
            set
            {
                if (value > 1 && !IsWorking)
                {
                    _anzahlKloetze = value;
                    SetUpWallProperties();
                }
            }
        }

        /// <summary>
        /// Kann der <see cref="WallBuilder"/> eine neue Mauer mit einer anderen Anzahl
        /// an Kloetzchen anfangen
        /// </summary>
        public bool IsWorking
        {
            get => _isWorking;
            private set { _isWorking = value; }
        }

        /// <summary>
        /// Die Breite der Mauer bzw. einer Reihe
        /// </summary>
        public int MauerBreite { get; set; }

        /// <summary>
        /// Die maximal moegliche Hoehe der Mauer
        /// </summary>
        public int MaxMauerHoehe { get; set; }

        /// <summary>
        /// Die Anzahl der Stellen in der Mauer, an denen eine Fuge moeglich waere
        /// </summary>
        public int AnzahlFugenStellen { get; set; }

        /// <summary>
        /// Die Anzahl an Fugen, die fuer eine Mauer der <see cref="MaxMauerHoehe"/> benutzt werden
        /// </summary>
        public int MaxFugenBenutzt { get; set; }

        #endregion

        #region Methods

        public void StartBruteForce()
        {
            IsWorking = true;
            for (int y = 0; y < Mauer.GetLength(0); y++)
            {
                for (int x = 0; x < Mauer.GetLength(1); x++)
                {
                    AddKlotz(y, x, x + 1);
                }
            }

            //for (int i = 0; i < Utilities.FakultaetBerechnen(AnzahlKloetze); i++)
            //{
            //    for (int n = 1; n <= AnzahlKloetze; n++)
            //    {
            //        for (int xPos = 0; xPos < AnzahlKloetze; xPos++)
            //        {
            //            AddKlotz(0, xPos, n);
            //        }
            //    }
            //}



            PrintMauer();
            IsWorking = false;
        }

        /// <summary>
        /// Berechnet alle von <see cref="AnzahlKloetze"/> abhaengigen Eigenschaften der Mauer
        /// </summary>
        private void SetUpWallProperties()
        {
            MauerBreite = (int)(Math.Pow(AnzahlKloetze, 2) + AnzahlKloetze) / 2; // Gausssche Summenformel
            AnzahlFugenStellen = MauerBreite - 1;
            MaxMauerHoehe = AnzahlFugenStellen / (AnzahlKloetze - 1);
            MaxFugenBenutzt = (AnzahlKloetze - 1) * MaxMauerHoehe;
            Mauer = new int[MaxMauerHoehe, AnzahlKloetze];
            Mauer.Fill2DArray(-1);
            Spalten = new bool[MauerBreite + 1];
        }

        /// <summary>
        /// Fuegt einen Klotz in die Mauer ein
        /// </summary>
        /// <param name="y">y-Position des Klotzes</param>
        /// <param name="x">x-Position des Klotzes</param>
        /// <param name="value">Der Wert des Klotzes</param>
        private void AddKlotz(int y, int x, int value)
        {
            Mauer[y, x] = value;
        }

        /// <summary>
        /// Berechnet die Summen von einem
        /// </summary>
        /// <param name="searchedY">y-Position des Klotzes</param>
        /// <param name="searchedX">x-Position des Klotzes</param>
        /// <returns><see cref="RowSum"/></returns>
        private RowSum GetRowSum(int searchedY, int searchedX)
        {
            var right = 0;
            var left = 0;

            for (int i = 0; i <= searchedX; i++)
            {
                if (Mauer[searchedY, i] == -1)
                {
                    right = -1;
                    break;
                }
                right += Mauer[searchedY, i];
            }

            for (int i = Mauer.GetLength(0); i >= searchedX; i--)
            {
                if (Mauer[searchedY, i] == -1)
                {
                    right = -1;
                    break;
                }
                left += Mauer[searchedY, i];
            }

            return new RowSum(right, left);
        }

        public void PrintMauer()
        {
            for (int y = 0; y < Mauer.GetLength(0); y++)
            {
                var reihe = "";
                for (int x = 0; x < Mauer.GetLength(1); x++)
                {
                    var placeholder = String.Concat(Enumerable.Repeat("0", Mauer[y, x]));
                    var value = Mauer[y, x].ToString(placeholder);
                    reihe += $"|{value}";
                }
                Console.WriteLine($"{reihe}|");
            }
        }

        #endregion

        #region Structs
        /// <summary>
        /// Speichert die Reihen-Summen eines Klotzes
        /// </summary>
        private struct RowSum
        {
            public int Right { get; }
            public int Left { get; }

            public RowSum(int right, int left)
            {
                Right = right;
                Left = left;
            }
        }

        #endregion
    }
}