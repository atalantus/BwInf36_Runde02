using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public struct QuaxInfo
    {
        #region Properties

        /// <summary>
        /// Quax Position
        /// </summary>
        public Point QuaxPos { get; private set; }
        
        /// <summary>
        /// Die node die Quax enthaelt
        /// </summary>
        public QuadratNode QuaxNode { get; set; }

        #endregion

        #region Methods

        public QuaxInfo(Point quaxPos)
        {
            QuaxPos = quaxPos;
            QuaxNode = null;
        }

        #endregion
    }
}