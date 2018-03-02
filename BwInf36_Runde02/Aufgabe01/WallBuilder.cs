using System;

namespace Aufgabe01
{
    public class WallBuilder
    {
        #region Fields

        private int _anzahlKloetzchen;

        #endregion

        #region Properties

        /// <summary>
        /// Die Anzahl von Kloetzchen in einer Reihe
        /// </summary>
        public int AnzahlKloetzchen
        {
            get => _anzahlKloetzchen;
            set
            {
                if (value > 1)
                {
                    _anzahlKloetzchen = value;
                    CalculateWallProperties();
                }
            }
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

        #endregion

        #region Private Methods

        /// <summary>
        /// Berechnet alle von <see cref="AnzahlKloetzchen"/> abhaengigen Eigenschaften der Mauer
        /// </summary>
        private void CalculateWallProperties()
        {
            MauerBreite = (int)(Math.Pow(AnzahlKloetzchen, 2) + AnzahlKloetzchen) / 2; // Gausssche Summenformel
            AnzahlFugenStellen = MauerBreite - 1;
            MaxMauerHoehe = AnzahlFugenStellen / (AnzahlKloetzchen - 1);
        }

        #endregion

        #region Public Methods



        #endregion
    }
}