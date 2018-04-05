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
            byte.TryParse(Console.ReadLine(), out var anzahlKloetze);
            Console.ForegroundColor = ConsoleColor.Red;
            while (anzahlKloetze <= 1 || anzahlKloetze > 22)
            {
                Console.WriteLine();
                Console.WriteLine("Die Anzahl der Kloetze muss zwischen 2 (eingeschlossen) und 22 (eingeschlossen) liegen!");
                Console.Write("Bitte waehlen Sie eine andere Anzahl von Kloetzchen in einer Reihe: ");
                byte.TryParse(Console.ReadLine(), out anzahlKloetze);
            }
            Console.ResetColor();
            Console.WriteLine();



            /**
             * Starte Algorithmus
             */
            wallBuilder.SetUpWallBuilder(anzahlKloetze);
            var end = false;
            while (!end)
            {
                try
                {
                    wallBuilder.StartAlgorithmus();
                    end = true;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("ERROR:");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine();
                    Console.ResetColor();

                    end = true;
                }
            }
            Console.WriteLine("Druecke ENTER um das Programm zu beenden");
            Console.ReadLine();
        }
    }
}
