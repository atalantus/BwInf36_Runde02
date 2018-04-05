using System;
using System.Collections.Generic;
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

        private byte _anzahlKloetze;
        private Stopwatch _stopwatch;
        private long _algorithmusZeit;

        #endregion

        #region Properties

        /// <summary>
        /// Alle moeglichen Reihen (Permutationen)
        /// </summary>
        public Reihe[] MoeglicheReihen { get; set; }

        /// <summary>
        /// Gefundene Mauer
        /// </summary>
        public Mauer RichtigeMauer { get; private set; }

        /// <summary>
        /// Die Anzahl von Nummern in einer Reihe
        /// </summary>
        public byte AnzahlKloetze
        {
            get => _anzahlKloetze;
            private set
            {
                _anzahlKloetze = value;
                SetUpWallProperties();
            }
        }

        /// <summary>
        /// Die Breite der Mauer bzw. einer Reihe
        /// </summary>
        public byte MauerBreite { get; set; }

        /// <summary>
        /// Die maximal moegliche Hoehe der Mauer
        /// </summary>
        public byte MaxMauerHoehe { get; set; }

        /// <summary>
        /// Die Anzahl der Stellen in der Mauer, an denen eine Fuge moeglich waere
        /// </summary>
        public byte AnzahlFugenStellen { get; set; }

        /// <summary>
        /// Die Anzahl an Fugen, die fuer eine Mauer der <see cref="MaxMauerHoehe"/> benutzt werden
        /// </summary>
        public byte MaxFugenBenutzt { get; set; }

        #endregion

        #region Methods

        public void SetUpWallBuilder(byte anzahlKloetze)
        {
            AnzahlKloetze = anzahlKloetze;
            PrintWallProperties();
        }

        public void StartAlgorithmus()
        {
            /**
             * Starte Algorithmus
             */
            _algorithmusZeit = 0L;

            /**
             * Initialisiere Felder
             */
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            /**
             * Erstelle Permutations Array
             */
            var values = new List<byte>();
            for (var n = 1; n <= AnzahlKloetze; n++)
            {
                values.Add((byte) n);
            }
            
            _algorithmusZeit += _stopwatch.ElapsedMilliseconds;
            _stopwatch.Restart();

            /**
             * Erstelle Permutationen
             */
            var rows = Utilities.GetPermutations(values).ToArray();
            MoeglicheReihen = new Reihe[rows.Length];

            using (var progress = new ProgressBar("Sammle Permutationen"))
            {
                for (var i = 0; i < rows.Length; i++)
                {
                    progress.Report((double)i / rows.Length);
                    var row = rows[i];
                    var r = new Reihe(row.ToArray(), (uint)i);
                    MoeglicheReihen[i] = r;
                }
            }
     
            _stopwatch.Stop();

            Console.WriteLine($"{rows.Length} Permutationen der Reihe in {_stopwatch.ElapsedMilliseconds}ms gefunden!");
            Console.WriteLine();

            _algorithmusZeit += _stopwatch.ElapsedMilliseconds;

            /**
             * Baue Mauern auf
             */
            _stopwatch.Restart();
            RichtigeMauer = FindMauerRekursion(MoeglicheReihen);
            _stopwatch.Stop();
            _algorithmusZeit += _stopwatch.ElapsedMilliseconds;

            /**
             * Ausgabe der Mauer
             */
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine($"Moegliche Mauer in {_stopwatch.ElapsedMilliseconds}ms gefunden");
            Console.ResetColor();

            Console.WriteLine(RichtigeMauer.ToString());

            Console.WriteLine();

            /**
             * Ausgabe der Zeit
             */
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Komplette Algorithmus Laufzeit: {_algorithmusZeit}ms");
            Console.WriteLine();
            Console.ResetColor();
        }

        /// <summary>
        /// Bildet eine moegliche Mauer
        /// </summary>
        /// <param name="allMoeglicheReihen">Die Liste aller moeglicher Reihen</param>
        /// <returns>Die zuerst gefundene moegliche Mauer</returns>
        private Mauer FindMauerRekursion(Reihe[] allMoeglicheReihen)
        {
            var reihenMatrix = new byte[MoeglicheReihen.Length][];

            for (var i = 0; i < allMoeglicheReihen.Length; i++)
            {
                var startMauer = new Mauer(MaxMauerHoehe);
                startMauer.AddReihe(allMoeglicheReihen[i]);

                var mauer = BaueMauerRekursiv(reihenMatrix, allMoeglicheReihen, startMauer);
                if (mauer.Fertig)
                {
                    // Mauer gefunden
                    return mauer;
                }
            }
            throw new Exception("Es wurde keine Mauer gefunden");
        }

        private Mauer BaueMauerRekursiv(byte[][] reihenMatrix, Reihe[] allMoeglicheReihen, Mauer aktuelleMauer)
        {
            // Reihen der aktuellen Mauer
            var reihen = aktuelleMauer.Reihen.ToList();
            // Reihen, die zu ALLEN Reihen der aktuellen Mauer passen
            var dazuMoeglicheReihen = allMoeglicheReihen.Where(r => reihen.All(pr =>
                Utilities.ReihenSindKompatibel(pr, r, ref reihenMatrix))).ToList();

            for (var i = 0; i < dazuMoeglicheReihen.Count; i++)
            {
                // Es gibt noch moegliche Mauern
                var mauer = new Mauer(aktuelleMauer);
                mauer.AddReihe(dazuMoeglicheReihen[i]);

                if (mauer.Fertig)
                    return mauer;

                var neueMauer = BaueMauerRekursiv(reihenMatrix, allMoeglicheReihen, mauer);
                if (neueMauer.Fertig)
                    return neueMauer;
            }
            
            // Mauer funktioniert nicht
            return aktuelleMauer;
        }

        /// <summary>
        /// Berechnet alle von <see cref="AnzahlKloetze"/> abhaengigen Eigenschaften der Mauer
        /// </summary>
        private void SetUpWallProperties()
        {
            MauerBreite = (byte) ((Math.Pow(AnzahlKloetze, 2) + AnzahlKloetze) / 2); // Gausssche Summenformel
            AnzahlFugenStellen = (byte) (MauerBreite - 1);
            MaxMauerHoehe = (byte) (AnzahlFugenStellen / (AnzahlKloetze - 1));
            MaxFugenBenutzt = (byte) ((AnzahlKloetze - 1) * MaxMauerHoehe);
        }

        /// <summary>
        /// Gibt die Werte der Mauer aus
        /// </summary>
        public void PrintWallProperties()
        {
            Console.WriteLine();
            Console.WriteLine($"Anzahl Kloetzchen in einer Reihe: {AnzahlKloetze}");
            Console.WriteLine($"Breite der Mauer: {MauerBreite}");
            Console.WriteLine($"Maximale Hoehe der Mauer: {MaxMauerHoehe}");
            Console.WriteLine($"Anzahl verfuegbarer Stellen fuer Fugen: {AnzahlFugenStellen}");
            Console.WriteLine($"Anzahl benoetigter Fugen fuer Mauer der maximalen Hoehe: {MaxFugenBenutzt}");
            Console.WriteLine();
            Console.WriteLine();
        }

        #endregion
    }
}