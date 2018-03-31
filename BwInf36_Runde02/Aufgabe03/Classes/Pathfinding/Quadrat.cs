using System.Numerics;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    /// <summary>
    /// Repraesentiert ein Quadrat
    /// </summary>
    public class Quadrat
    {
        #region Fields

        private Point _luEckpunkt;
        private Point _roEckpunkt;

        #endregion

        #region Properties

        /// <summary>
        /// Eckpunkt links unten des Quadrats
        /// </summary>
        public Point LU_Eckpunkt => _luEckpunkt;

        /// <summary>
        /// Eckpunkt rechts oben des Quadrats
        /// </summary>
        public Point RO_Eckpunkt => _roEckpunkt;

        /// <summary>
        /// Die Breite des Quadrats
        /// </summary>
        public int Breite { get; private set; }

        /// <summary>
        /// Die Hoehe des Quadrats
        /// </summary>
        public int Hoehe => Breite;

        /// <summary>
        /// Der Mittelpunkt des Quadrats
        /// </summary>
        public Point Mittelpunkt { get; private set; }

        #endregion

        #region Methods

        public Quadrat(Point luEckpunkt, Point roEckpunkt)
        {
            _luEckpunkt = luEckpunkt;
            _roEckpunkt = roEckpunkt;
            Breite = (int) (_roEckpunkt.X - _luEckpunkt.X);
            Mittelpunkt = new Point(LU_Eckpunkt.X + Breite / 2f, LU_Eckpunkt.Y + Hoehe / 2f);
        }

        public Quadrat(Point luEckpunkt, int breite)
        {
            _luEckpunkt = luEckpunkt;
            Breite = breite;
            _roEckpunkt = new Point(_luEckpunkt.X + Breite, _luEckpunkt.Y + Hoehe);
            Mittelpunkt = new Point(LU_Eckpunkt.X + Breite / 2f, LU_Eckpunkt.Y + Hoehe / 2f);
        }

        /// <summary>
        /// Ueberprueft ob sich zwei Quadrate beruehren oder ueberschneiden
        /// </summary>
        /// <param name="other">Das zweite Quadrat</param>
        /// <returns>True wenn sich beide Quadrate beruehren bzw. ueberschneiden</returns>
        public bool BeruehrtQuadrat(Quadrat other)
        {
            return !(other.LU_Eckpunkt.X > RO_Eckpunkt.X || other.LU_Eckpunkt.Y > RO_Eckpunkt.Y ||
                     other.RO_Eckpunkt.X < LU_Eckpunkt.X || other.RO_Eckpunkt.Y < LU_Eckpunkt.Y);
        }

        /// <summary>
        /// Ueberprueft ob sich ein Quadrate und ein Punkt beruehren oder ueberschneiden
        /// </summary>
        /// <param name="other">Der Punkt</param>
        /// <returns>True wenn sich das Quadrat und der Punkt beruehren bzw. ueberschneiden</returns>
        public bool BeruehrtPoint(Point other)
        {
            return !(other.X > RO_Eckpunkt.X || other.Y > RO_Eckpunkt.Y ||
                     other.X < LU_Eckpunkt.X || other.Y < LU_Eckpunkt.Y);
        }

        #endregion
    }
}