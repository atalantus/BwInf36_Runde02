using System.Collections.Generic;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public struct SearchInformation
    {
        #region Properties
        /// <summary>
        /// Quax Position
        /// </summary>
        public Point QuaxPos { get; private set; }
        /// <summary>
        /// Wurde Quax bereits gefunden
        /// </summary>
        public bool QuaxGefunden { get; set; }
        /// <summary>
        /// Stadt Position
        /// </summary>
        public Point StadtPos { get; private set; }
        /// <summary>
        /// Wurde Stadt bereits gefunden
        /// </summary>
        public bool StadtGefunden { get; set; }
        /// <summary>
        /// Der Weg von Quax zur Stadt
        /// </summary>
        public List<QuadratNode> Weg { get; set; }

        #endregion

        #region Methods

        public SearchInformation(Point quaxPos, Point stadtPos)
        {
            QuaxPos = quaxPos;
            QuaxGefunden = false;
            StadtPos = stadtPos;
            StadtGefunden = false;
            Weg = new List<QuadratNode>();
        }

        #endregion
    }
}