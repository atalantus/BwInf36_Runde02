using System;
using System.Numerics;

namespace Aufgabe01
{
    class Program
    {
        static void Main(string[] args)
        {
            /**
             * Variablen
             */
            WallBuilder wallBuilder = new WallBuilder();

            /**
             * Header
             */
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("BwInf36 | Runde 2 | Aufgabe 1 (Die Kunst der Fuge)");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.ResetColor();

            /**
             * Eingabe: Anzahl Kloetzchen
             */
            Console.Write("Anzahl der Kloetzchen in einer Reihe: ");
            int.TryParse(Console.ReadLine(), out var anzahlKloetze);

            Console.ForegroundColor = ConsoleColor.Red;
            while (anzahlKloetze <= 1)
            {
                Console.WriteLine();
                Console.WriteLine(anzahlKloetze == 1
                    ? "Fuer die maximale Anzahl von 1 Klotz, kann eine unendlich hohe Mauer gebaut werden!"
                    : "Die von Ihnen angegebene Anzahl Kloetzchen kann nicht verarbeitet werden!");
                Console.Write("Bitte waehlen Sie eine andere Anzahl von Kloetzchen in einer Reihe: ");
                int.TryParse(Console.ReadLine(), out anzahlKloetze);
            }

            Console.ResetColor();

            Console.WriteLine();

            /**
             * Debug Modus Abfrage und Ausgabe der Werte
             */
            var started = false;
            while (!started)
            {
                Console.WriteLine("Starte den Algorithmus im Debug oder Normalem Modus: (D/N)");
                var input = Console.ReadLine()?.ToUpper();
                if (input == "D")
                {
                    started = true;
                    wallBuilder.SetUpWallBuilder(anzahlKloetze, true);
                }
                else if (input == "N")
                {
                    started = true;
                    wallBuilder.SetUpWallBuilder(anzahlKloetze, false);
                }
            }
            Console.WriteLine();

            Console.WriteLine($"Anzahl Kloetzchen in einer Reihe: {wallBuilder.AnzahlKloetze}");
            Console.WriteLine($"Breite der Mauer: {wallBuilder.MauerBreite}");
            Console.WriteLine($"Maximale Hoehe der Mauer: {wallBuilder.MaxMauerHoehe}");
            Console.WriteLine($"Anzahl verfuegbarer Stellen fuer Fugen: {wallBuilder.AnzahlFugenStellen}");
            Console.WriteLine($"Anzahl benoetigter Fugen fuer Mauer der maximalen Hoehe: {wallBuilder.MaxFugenBenutzt}");

            var varianten = BigInteger.Pow(Utilities.FakultaetBerechnen(wallBuilder.AnzahlKloetze), 2);
            try
            {
                Console.WriteLine($"Anzahl an Mauer Varianten: {varianten.ToString()}");
            }
            catch (OutOfMemoryException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ACHTUNG!");
                Console.WriteLine($"Die Anzahl an moeglichen Varianten für eine Mauer mit einer Kloetzchen Anzahl von {wallBuilder.AnzahlKloetze} ist zu gross, um sie auszurechnen!");
                Console.ResetColor();
            }

            Console.WriteLine();
            

            /**
             * Starte Algorithmus
             */
            var end = false;
            while (!end)
            {
                try
                {
                    wallBuilder.StartAlgorithmus();
                    end = true;
                }
                catch (FugenUeberlappungException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("ERROR:");
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                    Console.ResetColor();

                    if (!wallBuilder.IsDebug)
                    {
                        Console.WriteLine("Algorithmus nochmal im Debug Modus starten? (Y/N)");
                        var input = Console.ReadLine()?.ToUpper();
                        if (input == "Y")
                        {
                            wallBuilder.SetUpWallBuilder(anzahlKloetze, true);
                        }
                    }
                    else
                    {
                        end = true;
                    }

                    Console.WriteLine();
                }
                catch (KeinMoeglicherKlotzException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("ERROR:");
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                    Console.ResetColor();

                    if (!wallBuilder.IsDebug)
                    {
                        Console.WriteLine("Algorithmus nochmal im Debug Modus starten? (Y/N)");
                        var input = Console.ReadLine()?.ToUpper();
                        if (input == "Y")
                        {
                            wallBuilder.SetUpWallBuilder(anzahlKloetze, true);
                        }
                    }
                    else
                    {
                        end = true;
                    }

                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
            Console.ReadLine();
        }
    }
}
