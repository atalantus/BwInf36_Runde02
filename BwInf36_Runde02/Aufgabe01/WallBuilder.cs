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

        private int _anzahlKloetze;
        private bool _isDebug;

        #endregion

        #region Properties

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
        /// Alle moeglichen Reihen
        /// </summary>
        public List<Reihe> MoeglicheReihen { get; set; }

        /// <summary>
        /// Liste der richtigen Mauern
        /// </summary>
        public List<Mauer> RichtigeMauern { get; private set; }

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
        public bool IsWorking { get; private set; }

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
            /**
             * Starte Algorithmus
             */
            IsWorking = true;

            /**
             * Aktiviere Main Stopwatch
             */
            var stopwatchComplete = new Stopwatch();
            stopwatchComplete.Start();

            /**
             * Initialisiere Felder
             */
            MoeglicheReihen = new List<Reihe>();
            RichtigeMauern = new List<Mauer>();
            var tempStopwatch = new Stopwatch();
            var moeglicheMauern = new List<Mauer>();

            /**
             * Erstelle Permutations Array
             */
            var values = new List<byte>();
            for (var n = 1; n <= AnzahlKloetze; n++)
            {
                values.Add((byte) n);
            }

            /**
             * Erstelle Permutationen
             */
            try
            {
                tempStopwatch.Start();
                var rows = Utilities.GetPermutations(values).ToList();     
                for (var i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var m = new Mauer(MaxMauerHoehe, MauerBreite);
                    var r = new Reihe(row.ToArray(), MauerBreite, (uint) i);
                    m.AddReihe(r);
                    MoeglicheReihen.Add(r);
                    moeglicheMauern.Add(m);
                }

                tempStopwatch.Stop();
                Debug.WriteLine("Got Permutations!");
                if (IsDebug) Console.WriteLine($"{rows.Count} Permutationen der Reihe in {tempStopwatch.ElapsedMilliseconds}ms gefunden!");
                tempStopwatch.Reset();
            } catch (Exception e)
            {
                tempStopwatch.Stop();
                stopwatchComplete.Stop();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine();
                Console.ResetColor();
            }

            /**
             * Erstelle Reihen Liste und Matrix
             */
            tempStopwatch.Start();
            var moeglicheReihenMatrix = new byte[MoeglicheReihen.Count, MoeglicheReihen.Count]; // 0 = gleiche Reihe / nicht berechnet; 1 = nicht kompatibel; 2 = kompatibel

            for (var n = 0; n < MaxMauerHoehe - 1; n++)
            {
                for (var i = 0; i < moeglicheReihenMatrix.GetLength(0); i++)
                {
                    var checkReihe = MoeglicheReihen[i];
                    for (var e = 0; e < moeglicheReihenMatrix.GetLength(1); e++)
                    {
                        var compareReihe = MoeglicheReihen[e];

                        if (checkReihe != compareReihe && moeglicheReihenMatrix[i, e] == 0)
                        {
                            var kompatibel = true;
                            for (var x = 0; x < checkReihe.FreieFugen.Length - 1; x++)
                            {
                                if (!checkReihe.FreieFugen[x] && !compareReihe.FreieFugen[x])
                                {
                                    kompatibel = false; // Reihen nicht kompatibel
                                }
                            }
                            if (kompatibel) moeglicheReihenMatrix[i, e] = 2;
                            else moeglicheReihenMatrix[i, e] = 1;
                        }
                    }
                }
            }

            Debug.WriteLine("Got start matrix!");
            tempStopwatch.Stop();
            if (IsDebug) Console.WriteLine($"\nGesamt Matrix der Reihen in {tempStopwatch.ElapsedMilliseconds}ms erstellt!");
            tempStopwatch.Reset();

            /**
             * Baue Mauern auf
             */
            RichtigeMauern = FindRichtigeMauernRekursion(moeglicheReihenMatrix, MoeglicheReihen, moeglicheMauern, 1);
            //RichtigeMauern = FindRichtigeMauernSchleife(moeglicheReihenMatrix, MoeglicheReihen, moeglicheMauern);
            stopwatchComplete.Stop();

            /**
             * Ausgabe der Mauern
             */
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine($"{RichtigeMauern.Count} moeglichen Mauer Loesungen:");
            Console.ResetColor();

            for (var i = 0; i < RichtigeMauern.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine($"------- {i + 1} -------");
                Console.ResetColor();
                Console.WriteLine(RichtigeMauern[i].ToString());
            }

            Console.WriteLine();

            /**
             * Ausgabe der Zeit
             */
            var completeTs = stopwatchComplete.Elapsed;
            var elapsedTime =
                $"{completeTs.Hours:00}:{completeTs.Minutes:00}:{completeTs.Seconds:00}.{completeTs.Milliseconds / 10:00}";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Complete Algorithm Time: {elapsedTime} ({stopwatchComplete.ElapsedMilliseconds}ms)");
            Console.WriteLine();
            Console.ResetColor();

            IsWorking = false;
        }

        /// <summary>
        /// Bildet alle moeglichen Mauern in einer Schleifen Struktur
        /// </summary>
        /// <param name="allMoeglicheReihenMatrix">Die Matrix von allen moeglichen Reihen</param>
        /// <param name="allMoeglicheReihen">Die Liste aller moeglicher Reihen</param>
        /// <param name="startMauern">Die Liste der Start Mauern</param>
        /// <returns>Alle moeglichen Mauern in einer Liste</returns>
        private List<Mauer> FindRichtigeMauernSchleife(byte[,] allMoeglicheReihenMatrix, List<Reihe> allMoeglicheReihen, List<Mauer> startMauern)
        {
            var aktuelleMauern = startMauern;
            for (var curMauerHoehe = 1; curMauerHoehe < MaxMauerHoehe; curMauerHoehe++)
            {
                var tempAktuelleMauern = new List<Mauer>();
                for (var i = 0; i < aktuelleMauern.Count; i++)
                {
                    var reihen = aktuelleMauern[i].Reihen.ToList();
                    var dazuMoeglicheReihen = allMoeglicheReihen.Where(r => reihen.All(pr => Utilities.ReihenSindKompatibel(pr, r, allMoeglicheReihenMatrix, allMoeglicheReihen))).ToList();
                    var newAktuelleMauern = dazuMoeglicheReihen.Select((t, e) => aktuelleMauern[i].AddReihe(dazuMoeglicheReihen.ToList()[e]));
                    tempAktuelleMauern.AddRange(newAktuelleMauern);
                }
                aktuelleMauern = tempAktuelleMauern;
                Debug.WriteLine($"Got Mauern with height {curMauerHoehe}");
                if (IsDebug) Console.WriteLine($"\n{aktuelleMauern.Count} Mauern der Hoehe {curMauerHoehe} gebaut!");
            }
            return aktuelleMauern;
        }

        /// <summary>
        /// Bildet alle moeglichen Mauern rekursiv
        /// </summary>
        /// <param name="allMoeglicheReihenMatrix">Die Matrix von allen moeglichen Reihen</param>
        /// <param name="allMoeglicheReihen">Die Liste aller moeglicher Reihen</param>
        /// <param name="aktuelleMauern">Die zuletzt gebildeten Mauern</param>
        /// <param name="curHoehe">Die aktuelle Hohe der bisher gebildeten Mauern</param>
        /// <returns>Alle moeglichen Mauern in einer Liste</returns>
        private List<Mauer> FindRichtigeMauernRekursion(byte[,] allMoeglicheReihenMatrix, List<Reihe> allMoeglicheReihen, List<Mauer> aktuelleMauern, int curHoehe)
        {
            var newAktuelleMauern = new List<Mauer>();

            /**
             * TODO: Optimize!
             */
            for (var i = 0; i < aktuelleMauern.Count; i++)
            {
                var reihen = aktuelleMauern[i].Reihen.ToList();
                var dazuMoeglicheReihen = allMoeglicheReihen.Where(r => reihen.All(pr => Utilities.ReihenSindKompatibel(pr, r, allMoeglicheReihenMatrix, allMoeglicheReihen))).ToList();
                newAktuelleMauern.AddRange(dazuMoeglicheReihen.Select((t, e) => aktuelleMauern[i].AddReihe(dazuMoeglicheReihen.ToList()[e])));
            }
            curHoehe++;

            Debug.WriteLine($"Got Mauern with height {curHoehe}");
            if (IsDebug) Console.WriteLine($"\n{newAktuelleMauern.Count} Mauern der Hoehe {curHoehe} gebaut!");
            
            return curHoehe >= MaxMauerHoehe ? newAktuelleMauern : FindRichtigeMauernRekursion(allMoeglicheReihenMatrix, allMoeglicheReihen, newAktuelleMauern, curHoehe);
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
        }

        //public void PrintMauer(bool fertig)
        //{
        //    if (fertig)
        //    {
        //        PrintWallProperties();
        //        Console.ForegroundColor = ConsoleColor.Green;
        //        Console.WriteLine($"FERTIGE MAUER FUER N = {AnzahlKloetze}");
        //        Console.WriteLine();
        //        Console.ResetColor();
        //    }
        //    var mauerBuilder = new StringBuilder(); // https://blog.goyello.com/2013/01/07/8-most-common-mistakes-c-developers-make/
        //    for (int y = 0; y < AktuelleMauer.Reihen.Length; y++)
        //    {
        //        Reihe curReihe = AktuelleMauer.Reihen[y];
        //        var reihe = new StringBuilder();
        //        var mauerBuilderReihe = new StringBuilder("[");
        //        for (int x = 0; x < AnzahlKloetze; x++)
        //        {
        //            var placeholder = "";
        //            placeholder = String.Concat(Enumerable.Repeat("0", curReihe.Kloetze[x]));
        //            var value = curReihe.Kloetze[x].ToString(placeholder);
        //            reihe.Append($"|{value}");

        //            if (x != curReihe.Kloetze.Length - 1)
        //                mauerBuilderReihe.Append($"{curReihe.Kloetze[x]}, ");
        //            else
        //                mauerBuilderReihe.Append($"{curReihe.Kloetze[x]}],");
        //        }
        //        Console.WriteLine($"{reihe}|");
        //        mauerBuilder.Append(mauerBuilderReihe);
        //    }

        //    mauerBuilder.Length -= 1;
        //    Console.WriteLine();

        //    if (fertig)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Yellow;
        //        Console.WriteLine();
        //        Console.WriteLine("String für Mauerersteller Website:");
        //        Console.WriteLine();
        //        Console.WriteLine($"[{mauerBuilder}]");
        //        Console.WriteLine();
        //        Console.ResetColor();
        //    }
        //}

        #endregion
    }
}