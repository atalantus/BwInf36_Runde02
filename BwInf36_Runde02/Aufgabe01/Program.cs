using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
            int.TryParse(Console.ReadLine(), out var eingabe);

            Console.ForegroundColor = ConsoleColor.Red;
            while (eingabe <= 1)
            {
                Console.WriteLine();
                Console.WriteLine(eingabe == 1
                    ? "Fuer die maximale Anzahl von 1 Klotz, kann eine unendlich hohe Mauer gebaut werden!"
                    : "Die von Ihnen angegebene Anzahl Kloetzchen kann nicht verarbeitet werden!");
                Console.Write("Bitte waehlen Sie eine andere Anzahl von Kloetzchen in einer Reihe: ");
                int.TryParse(Console.ReadLine(), out eingabe);
            }

            wallBuilder.AnzahlKloetze = eingabe;

            Console.ResetColor();

            Console.WriteLine();

            /**
             * Ausgabe der Werte
             */

            Console.WriteLine($"Anzahl Kloetzchen in einer Reihe: {wallBuilder.AnzahlKloetze}");
            Console.WriteLine($"Breite der Mauer: {wallBuilder.MauerBreite}");
            Console.WriteLine($"Maximale Hoehe der Mauer: {wallBuilder.MaxMauerHoehe}");
            Console.WriteLine($"Anzahl verfuegbarer Stellen fuer Fugen: {wallBuilder.AnzahlFugenStellen}");
            Console.WriteLine($"Anzahl benoetigter Fugen fuer Mauer der maximalen Hoehe: {wallBuilder.MaxFugenBenutzt}");

            BigInteger varianten = BigInteger.Pow(Utilities.FakultaetBerechnen(wallBuilder.AnzahlKloetze), 2);
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
            Console.WriteLine("Druecke ENTER, um den Algorithmus zur Suche der Mauer zu starten.");
            Console.WriteLine();

            /**
             * Starte Algorithmus
             */
            wallBuilder.StartBruteForce();

            Console.ReadLine();
        }
    }
}
