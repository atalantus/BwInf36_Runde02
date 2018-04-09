using System;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    /// <summary>
    ///     Repraesentiert ein Quadrat
    /// </summary>
    public class Quadrat
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        ///     Eckpunkt links oben des Quadrats
        /// </summary>
        public Point LO_Eckpunkt { get; }

        /// <summary>
        ///     Eckpunkt rechts unten des Quadrats
        /// </summary>
        public Point RU_Eckpunkt { get; }

        /// <summary>
        ///     Die Breite des Quadrats
        /// </summary>
        public int Breite { get; }

        /// <summary>
        ///     Die Hoehe des Quadrats
        /// </summary>
        public int Hoehe => Breite;

        /// <summary>
        ///     Der Mittelpunkt des Quadrats
        /// </summary>
        public Point Mittelpunkt { get; }

        #endregion

        #region Methods

        public Quadrat(Point loEckpunkt, Point ruEckpunkt)
        {
            LO_Eckpunkt = loEckpunkt;
            RU_Eckpunkt = ruEckpunkt;
            Breite = (int) (RU_Eckpunkt.X - LO_Eckpunkt.X);
            Mittelpunkt = new Point(LO_Eckpunkt.X + Breite / 2f, LO_Eckpunkt.Y + Hoehe / 2f);
        }

        public Quadrat(Point loEckpunkt, int breite)
        {
            LO_Eckpunkt = loEckpunkt;
            Breite = breite;
            RU_Eckpunkt = new Point(LO_Eckpunkt.X + Breite, Math.Abs(LO_Eckpunkt.Y + Hoehe));
            Mittelpunkt = new Point(LO_Eckpunkt.X + Breite / 2f, LO_Eckpunkt.Y + Hoehe / 2f);
        }

        /// <summary>
        ///     Ueberprueft ob sich zwei Quadrate beruehren oder ueberschneiden
        /// </summary>
        /// <param name="other">Das zweite Quadrat</param>
        /// <returns>True wenn sich beide Quadrate beruehren bzw. ueberschneiden</returns>
        public bool BeruehrtQuadrat(Quadrat other)
        {
            return !(other.LO_Eckpunkt.X > RU_Eckpunkt.X || other.LO_Eckpunkt.Y > RU_Eckpunkt.Y ||
                     other.RU_Eckpunkt.X < LO_Eckpunkt.X || other.RU_Eckpunkt.Y < LO_Eckpunkt.Y);
        }

        /// <summary>
        ///     Ueberprueft ob sich ein Quadrate und ein Punkt beruehren oder ueberschneiden
        /// </summary>
        /// <param name="other">Der Punkt</param>
        /// <returns>True wenn sich das Quadrat und der Punkt beruehren bzw. ueberschneiden</returns>
        public bool BeruehrtPoint(Point other)
        {
            return !(other.X > RU_Eckpunkt.X || other.Y > RU_Eckpunkt.Y ||
                     other.X < LO_Eckpunkt.X || other.Y < LO_Eckpunkt.Y);
        }

        #endregion
    }
}