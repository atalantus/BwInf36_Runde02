using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
        /// HACK: Alle moeglichen Reihen
        /// </summary>
        public List<Reihe> MoeglicheReihen { get; set; }

        /// <summary>
        /// HACK: Liste der richtigen Mauern fuer BruteForce
        /// </summary>
        public List<Mauer> RichtigeMauern { get; private set; }

        /// <summary>
        /// Die aktuelle Mauer
        /// </summary>
        public Mauer AktuelleMauer { get; set; }

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

            /**
             * Ohne Algorithmus
             * */

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

            /**
             * Algorithmus 01
             * */

            //Counter = 0;
            //for (int n = 0; n < AnzahlKloetze; n++)
            //{
            //    //Reihe[] sortedMauer = Mauer;
            //    for (int m = 0; m < Mauer.Length; m++)
            //    {
            //        Counter++;
            //        Array.Sort(Mauer);
            //        FindNextKlotz(Mauer[0]);
            //    }
            //}

            /**
             * BruteForce
             * */
            MoeglicheReihen = new List<Reihe>();
            RichtigeMauern = new List<Mauer>();

            //var stelle = 0;
            //var done = false;

            //for (int i = 0; i < Math.Pow(AnzahlKloetze, AnzahlKloetze) - 1; i++)
            //{
            //    stelle = 0;
            //    done = false;
            //    while (stelle < AnzahlKloetze)
            //    {
            //        if (Mauer[0].Kloetze[stelle] < AnzahlKloetze && !done)
            //        {
            //            Mauer[0].SetKlotz(stelle, Mauer[0].Kloetze[stelle] + 1);
            //            done = true;
            //            stelle++;

            //            string reihe = "";
            //            foreach (var item in Mauer[0].Kloetze)
            //            {
            //                reihe += item.ToString();
            //            }
            //            Debug.WriteLine(reihe);

            //            // Filter
            //            bool[] nummerBenutzt = new bool[AnzahlKloetze];
            //            bool richtigeMauer = true;
            //            foreach (var klotz in Mauer[0].Kloetze)
            //            {
            //                if (nummerBenutzt[klotz - 1]) richtigeMauer = false;
            //                else nummerBenutzt[klotz - 1] = true;
            //            }
            //            if (richtigeMauer)
            //            {
            //                MoeglicheReihen.Add(new Reihe(Mauer[0].Kloetze, MauerBreite));
            //                //Debug.WriteLine($"====================={reihe}===================");
            //            }
            //        }
            //        else if (done)
            //        {
            //            stelle = AnzahlKloetze;
            //        } else
            //        {
            //            Mauer[0].SetKlotz(stelle, 1);
            //            stelle++;
            //        }
            //    }
            //}

            List<int> values = new List<int>();
            for (int n = 1; n <= AnzahlKloetze; n++)
            {
                values.Add(n);
            }
            IEnumerable<int> rowValues = values;

            var passendeReihen = new List<Reihe>();
            var passendeMauern = new List<Mauer>();
            
            try
            {
                var rows = Utilities.GetPermutations(rowValues);
                Debug.WriteLine("Got Permutations!");
                foreach (var row in rows)
                {
                    Mauer m = new Mauer(MaxMauerHoehe, MauerBreite);
                    Reihe r = new Reihe(m, row.ToArray(), MauerBreite);
                    m.Reihen[0] = r;
                    passendeReihen.Add(r);
                    passendeMauern.Add(m);
                }
            } catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine();
                Console.ResetColor();
            }

            var passendeReihenMatrix = new int[passendeReihen.Count, passendeReihen.Count]; // 0 = gleiche Reihe / nicht berechnet; -1 = nicht kompatibel; 1 = kompatibel

            for (int n = 0; n < MaxMauerHoehe - 1; n++)
            {
                for (int i = 0; i < passendeReihenMatrix.GetLength(0); i++)
                {
                    Reihe checkReihe = passendeReihen[i];
                    for (int e = 0; e < passendeReihenMatrix.GetLength(1); e++)
                    {
                        Reihe compareReihe = passendeReihen[e];

                        if (checkReihe != compareReihe && passendeReihenMatrix[i, e] == 0)
                        {
                            var kompatibel = true;
                            for (int x = 0; x < checkReihe.Mauer.FreieFugen.Length - 1; x++)
                            {
                                if (!checkReihe.Mauer.FreieFugen[x] && !compareReihe.Mauer.FreieFugen[x])
                                {
                                    // Reihen nicht kompatibel
                                    kompatibel = false;
                                }
                            }
                            if (kompatibel) passendeReihenMatrix[i, e] = 1;
                            else passendeReihenMatrix[i, e] = -1;
                        }
                    }
                }
            }


            //for (int i = 0; i < passendeReihenMatrix.GetLength(0); i++)
            //{
            //    for (int e = 0; e < passendeReihenMatrix.GetLength(1); e++)
            //    {
            //        Debug.WriteLine("----------");
            //        Debug.WriteLine($"({i} | {e}): {passendeReihenMatrix[i, e]}");
            //        Debug.WriteLine(passendeReihen[i].ToString());
            //        Debug.WriteLine(passendeReihen[e].ToString());
            //    }
            //}
            Debug.WriteLine("Got start matrix!");

            //RichtigeMauern = FindRichtigeMauernRekursion(passendeReihenMatrix, passendeReihen, passendeMauern, 1);
            //RichtigeMauern = FindRichtigeMauernSchleife(passendeReihenMatrix, passendeReihen, passendeMauern);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Alle moeglichen Mauer Loesungen:");
            Console.ResetColor();

            for (int i = 0; i < RichtigeMauern.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine($"------- {i + 1} -------");
                Console.ResetColor();
                Console.WriteLine(RichtigeMauern[i].ToString());
            }

            //PrintMauer(true); // HACK: Nicht bei BruteForce
            IsWorking = false;
        }

        //private List<Mauer> FindRichtigeMauernSchleife(int[,] allPassendeReihenMatrix, List<Reihe> allPassendeReihen, List<Mauer> startMauern)
        //{
        //    List<Mauer> aktuelleMauern = startMauern;
        //    for (int curMauerHoehe = 1; curMauerHoehe < MaxMauerHoehe; curMauerHoehe++)
        //    {
        //        var aktuelleMauernKombiniertMatrix = new bool[allPassendeReihen.Count, allPassendeReihen.Count];
        //        List<Mauer> tempAktuelleMauern = new List<Mauer>();
        //        for (int i = 0; i < aktuelleMauern.Count; i++)
        //        {
        //            List<Mauer> newAktuelleMauern = new List<Mauer>();
        //            List<Reihe> reihen = aktuelleMauern[i].Reihen.ToList();
        //            var dazupassendeReihen = allPassendeReihen.Where(r => reihen.All(pr => Utilities.ReihenSindKompatibel(pr, r, allPassendeReihenMatrix, allPassendeReihen))).ToList();
        //            for (int e = 0; e < dazupassendeReihen.Count; e++)
        //            {
        //                //Debug.WriteLine($"aktuelleMauer {i + 1}: dazupassende Reihe {e + 1}");
        //                newAktuelleMauern.Add(Utilities.MergeMauer(aktuelleMauern[i], dazupassendeReihen.ToList()[e].Mauer));
        //            }
        //            tempAktuelleMauern.AddRange(newAktuelleMauern);
        //        }
        //        aktuelleMauern = tempAktuelleMauern;
        //    }
        //    return aktuelleMauern;
        //}

        //private List<Mauer> FindRichtigeMauernRekursion(int[,] allPassendeReihenMatrix, List<Reihe> allPassendeReihen, List<Mauer> aktuelleMauern, int curHoehe)
        //{
        //    List<Mauer> newAktuelleMauern = new List<Mauer>();

        //    for (int i = 0; i < aktuelleMauern.Count; i++)
        //    {
        //        List<Reihe> reihen = aktuelleMauern[i].Reihen.ToList();
        //        var dazupassendeReihen = allPassendeReihen.Where(r => reihen.All(pr => Utilities.ReihenSindKompatibel(pr, r, allPassendeReihenMatrix, allPassendeReihen))).ToList();
        //        for (int e = 0; e < dazupassendeReihen.Count; e++)
        //        {
        //            Debug.WriteLine($"aktuelleMauer {i + 1}: dazupassende Reihe {e + 1}");
        //            newAktuelleMauern.Add(Utilities.MergeMauer(aktuelleMauern[i], dazupassendeReihen.ToList()[e].Mauer));
        //        }
        //    }
        //    Debug.WriteLine($"Got Mauern with height {curHoehe}");

        //    curHoehe++;
        //    if (curHoehe >= MaxMauerHoehe) return newAktuelleMauern;
        //    else return FindRichtigeMauernRekursion(allPassendeReihenMatrix, allPassendeReihen, newAktuelleMauern, curHoehe);
        //}

        /// <summary>
        /// Berechnet alle von <see cref="AnzahlKloetze"/> abhaengigen Eigenschaften der Mauer
        /// </summary>
        private void SetUpWallProperties()
        {
            MauerBreite = (int)(Math.Pow(AnzahlKloetze, 2) + AnzahlKloetze) / 2; // Gausssche Summenformel
            AnzahlFugenStellen = MauerBreite - 1;
            MaxMauerHoehe = AnzahlFugenStellen / (AnzahlKloetze - 1);
            MaxFugenBenutzt = (AnzahlKloetze - 1) * MaxMauerHoehe;
            AktuelleMauer = new Mauer(MaxMauerHoehe, MauerBreite);

            for (var i = 0; i < AktuelleMauer.Reihen.Length; i++)
            {
                AktuelleMauer.Reihen[i] = new Reihe(AktuelleMauer, AnzahlKloetze);
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
        /// HACK: Fuegt einen Klotz in eine Reihe, ohne ihn vorher zu ueberpruefen
        /// </summary>
        /// <param name="reihe"></param>
        /// <param name="x"></param>
        /// <param name="value"></param>
        private void AddKlotzBruteForce(Reihe reihe, int x, int value)
        {
            reihe.SetKlotz(x, value);
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

            var varianten = Utilities.FakultaetBerechnen(AnzahlKloetze) * MaxMauerHoehe;
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
            var mauerBuilder = new StringBuilder(); // https://blog.goyello.com/2013/01/07/8-most-common-mistakes-c-developers-make/
            for (int y = 0; y < AktuelleMauer.Reihen.Length; y++)
            {
                Reihe curReihe = AktuelleMauer.Reihen[y];
                var reihe = new StringBuilder();
                var mauerBuilderReihe = new StringBuilder("[");
                for (int x = 0; x < AnzahlKloetze; x++)
                {
                    var placeholder = "";
                    placeholder = String.Concat(Enumerable.Repeat("0", curReihe.Kloetze[x]));
                    var value = curReihe.Kloetze[x].ToString(placeholder);
                    reihe.Append($"|{value}");

                    if (x != curReihe.Kloetze.Length - 1)
                        mauerBuilderReihe.Append($"{curReihe.Kloetze[x]}, ");
                    else
                        mauerBuilderReihe.Append($"{curReihe.Kloetze[x]}],");
                }
                Console.WriteLine($"{reihe}|");
                mauerBuilder.Append(mauerBuilderReihe);
            }

            mauerBuilder.Length -= 1;
            Console.WriteLine();

            if (fertig)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine("String für Mauerersteller Website:");
                Console.WriteLine();
                Console.WriteLine($"[{mauerBuilder}]");
                Console.WriteLine();
                Console.ResetColor();
            }
        }

        #endregion
    }
}