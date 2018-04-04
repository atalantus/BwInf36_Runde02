using System;
using System.Numerics;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    /// <summary>
    /// Extra Methoden fuers Pathfinding
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Berechnet die absolute Entfernung zweier Quadrate
        /// Wenn Entfernung verglichen werden soll, brauchen alle Quadrate die gleiche Groesse!
        /// </summary>
        /// <param name="first">Erste Quadrat</param>
        /// <param name="second">Zeite Quadrat</param>
        /// <returns>Die absolute Entfernung im Quadrat (hoch 2)</returns>
        public static double EntfernungBerechnen(Quadrat first, Quadrat second)
        {
            var a = first.Mittelpunkt;
            var b = second.Mittelpunkt;
            return Math.Abs((b - a).LengthSquared);
        }

        /// <summary>
        /// Berechnet die absolute Entfernung zwischen einem Quadrat und einem Punkt
        /// Wenn Entfernung verglichen werden soll, brauchen alle Quadrate die gleiche Groesse!
        /// </summary>
        /// <param name="first">Erste Quadrat</param>
        /// <param name="b">Punkt</param>
        /// <returns>Die absolute Entfernung im Quadrat (hoch 2)</returns>
        public static double EntfernungBerechnen(Quadrat first, Point b)
        {
            var a = first.Mittelpunkt;
            return Math.Abs((b - a).LengthSquared);
        }
    }
}