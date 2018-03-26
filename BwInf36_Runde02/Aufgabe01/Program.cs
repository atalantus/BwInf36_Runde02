using System;
using System.Collections.Generic;
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
            var wallBuilder = new WallBuilder();



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
            var isDebug = false;
            while (!started)
            {
                Console.WriteLine("Starte den Algorithmus im Debug oder Normalem Modus: (D/N)");
                var input = Console.ReadLine()?.ToLower();
                if (input == "d" || input == "debug")
                {
                    started = true;
                    isDebug = true;
                }
                else if (input == "n" || input == "normal")
                {
                    started = true;
                    isDebug = false;
                }
            }



            /**
             * Algorithmus Typ Abfrage
             */
            var gotAlgorithm = false;
            var isRekursiv = true;
            while (!gotAlgorithm)
            {
                Console.WriteLine("Fuehre den Algorithmus Rekursiv oder in einer Schleife aus: (R/S)");
                var input = Console.ReadLine()?.ToLower();
                if (input == "r" || input == "rekursiv")
                {
                    gotAlgorithm = true;
                    isRekursiv = true;
                }
                else if (input == "s" || input == "schleife")
                {
                    gotAlgorithm = true;
                    isRekursiv = false;
                }
            }


            /**
             * Starte Algorithmus
             */
            wallBuilder.SetUpWallBuilder(anzahlKloetze, isDebug, isRekursiv);
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
                    if (wallBuilder.IsDebug) wallBuilder.PrintWallProperties();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("ERROR:");
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                    Console.ResetColor();

                    if (!wallBuilder.IsDebug)
                    {
                        Console.WriteLine("Schreibe [Y] um den Algorithmus nochmal im Debug Modus zu starten");
                        var input = Console.ReadLine()?.ToUpper();
                        if (input == "Y")
                        {
                            wallBuilder.SetUpWallBuilder(anzahlKloetze, true, isRekursiv);
                        }
                        else end = true;
                    }
                    else
                    {
                        end = true;
                    }

                    Console.WriteLine();
                }
                catch (KeinMoeglicherKlotzException e)
                {
                    if (wallBuilder.IsDebug) wallBuilder.PrintWallProperties();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("ERROR:");
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                    Console.ResetColor();

                    if (!wallBuilder.IsDebug)
                    {
                        Console.WriteLine("Schreibe [Y] um den Algorithmus nochmal im Debug Modus zu starten");
                        var input = Console.ReadLine()?.ToUpper();
                        if (input == "Y")
                        {
                            wallBuilder.SetUpWallBuilder(anzahlKloetze, true, isRekursiv);
                        }
                        else end = true;
                    }
                    else
                    {
                        end = true;
                    }

                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    if (wallBuilder.IsDebug) wallBuilder.PrintWallProperties();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
            Console.WriteLine("Druecke ENTER um das Programm zu beenden");
            Console.ReadLine();
        }
    }
}
