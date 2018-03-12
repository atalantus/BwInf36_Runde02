using System;
using System.Linq;
using System.Numerics;

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
        private bool _isDebug;

        #endregion

        #region Properties

        private int Counter { get; set; }

        /// <summary>
        /// Wurde der Algorithmus im Debug Mode gestartet
        /// </summary>
        public bool IsDebug
        {
            get => _isDebug;
            set
            {
                if (!IsWorking)
                    _isDebug = value;
            }
        }

        /// <summary>
        /// Die aktuelle Mauer
        /// </summary>
        public Reihe[] Mauer { get; set; }

        /// <summary>
        /// Enthaelt die bisherigen Spalten (Ohne Anfangs- und End-Spalte)
        /// </summary>
        public bool[] Spalten { get; set; }

        /// <summary>
        /// Die Anzahl von Nummern in einer Reihe
        /// </summary>
        public int AnzahlKloetze
        {
            get => _anzahlKloetze;
            private set
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

        public void SetUpWallBuilder(int anzahlKloetze, bool debug)
        {
            AnzahlKloetze = anzahlKloetze;
            IsDebug = debug;
        }

        public void StartAlgorithmus()
        {
            IsWorking = true;
            //for (int y = 0; y < Mauer.Length; y++)
            //{
            //    Reihe curReihe = Mauer[y];
            //    for (int x = 0; x < AnzahlKloetze; x++)
            //    {
            //        if (!AddKlotz(y, x, x + 1))
            //        {
            //            IsWorking = false;
            //            PrintMauer(false);
            //            throw new FugenUeberlappungException($"Spalten wuerden sich ueberlappen!\n{x+1}er Klotz in Reihe {y}, Platz {x}");
            //        }
            //    }
            //}
            Counter = 0;
            for (int n = 0; n < AnzahlKloetze; n++)
            {
                //Reihe[] sortedMauer = Mauer;
                for (int m = 0; m < Mauer.Length; m++)
                {
                    Counter++;
                    Array.Sort(Mauer);
                    FindNextKlotz(Mauer[0]);
                }
            }

            PrintMauer(true);
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
            Mauer = new Reihe[MaxMauerHoehe];

            for (var i = 0; i < Mauer.Length; i++)
            {
                Mauer[i] = new Reihe(AnzahlKloetze);
            }

            Spalten = new bool[AnzahlFugenStellen];
        }

        private void FindNextKlotz(Reihe reihe)
        {
            for (int n = 1; n <= AnzahlKloetze; n++)
            {
                if (reihe.Nummern[n - 1])
                    if (AddKlotz(reihe, reihe.LastKlotzIndex + 1, n)) return;
            }

            IsWorking = false;
            throw new KeinMoeglicherKlotzException("Es gibt keinen passenden Klotz mehr fuer die naechste Reihe!");
        }

        /// <summary>
        /// Fuegt einen Klotz in die Mauer ein
        /// </summary>
        /// <param name="reihe">Reihe des Klotzes</param>
        /// <param name="x">x-Position des Klotzes</param>
        /// <param name="value">Der Wert des Klotzes</param>
        private bool AddKlotz(Reihe reihe, int x, int value)
        {
            var letzterKlotz = !(x < AnzahlKloetze - 1);
            if (!letzterKlotz)
            {
                // Es ist nicht der letzte Klotz in der Reihe
                int rsRight = reihe.GetRowSum(x);
                rsRight += value;
                if (Spalten[rsRight - 1]) return false; // Spalte bereits belegt
                Spalten[rsRight - 1] = true; // Neue Spalte eintragen
            }
            
            reihe.SetKlotz(x, value);

            if (IsDebug)
            {
                Console.WriteLine();
                Console.WriteLine($"Step {Counter} - {value}er Klotz an der {x}ten Stelle der obersten Reihe");
                Console.WriteLine();
                PrintMauer(false);
            }

            return true;
        }

        public void PrintWallProperties()
        {
            Console.WriteLine();

            Console.WriteLine($"Anzahl Kloetzchen in einer Reihe: {AnzahlKloetze}");
            Console.WriteLine($"Breite der Mauer: {MauerBreite}");
            Console.WriteLine($"Maximale Hoehe der Mauer: {MaxMauerHoehe}");
            Console.WriteLine($"Anzahl verfuegbarer Stellen fuer Fugen: {AnzahlFugenStellen}");
            Console.WriteLine($"Anzahl benoetigter Fugen fuer Mauer der maximalen Hoehe: {MaxFugenBenutzt}");

            var varianten = BigInteger.Pow(Utilities.FakultaetBerechnen(AnzahlKloetze), 2);
            try
            {
                Console.WriteLine($"Anzahl an Mauer Varianten: {varianten.ToString()}");
            }
            catch (OutOfMemoryException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Die Anzahl an moeglichen Varianten für eine Mauer mit einer Kloetzchen Anzahl von {AnzahlKloetze} ist zu gross, um sie auszurechnen!");
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        public void PrintMauer(bool fertig)
        {
            if (fertig)
            {
                PrintWallProperties();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"FERTIGE MAUER FUER N = {AnzahlKloetze}");
                Console.WriteLine();
                Console.ResetColor();
            }
            var mauerBuilder = "";
            for (int y = 0; y < Mauer.Length; y++)
            {
                Reihe curReihe = Mauer[y];
                var reihe = "";
                var mauerBuilderReihe = "[";
                for (int x = 0; x < AnzahlKloetze; x++)
                {
                    var placeholder = "";
                    placeholder = String.Concat(Enumerable.Repeat("0", curReihe.Kloetze[x]));
                    var value = curReihe.Kloetze[x].ToString(placeholder);
                    reihe += $"|{value}";

                    if (x != curReihe.Kloetze.Length - 1)
                        mauerBuilderReihe += $"{curReihe.Kloetze[x]}, ";
                    else
                        mauerBuilderReihe += $"{curReihe.Kloetze[x]}],\n";
                }
                Console.WriteLine($"{reihe}|");
                mauerBuilder += mauerBuilderReihe;
            }
            Console.WriteLine();

            if (fertig && IsDebug)
            {
                Console.WriteLine("String für Mauerersteller Website:");
                Console.WriteLine();
                Console.WriteLine($"[\n{mauerBuilder}]");
            }
        }

        #endregion
    }
}