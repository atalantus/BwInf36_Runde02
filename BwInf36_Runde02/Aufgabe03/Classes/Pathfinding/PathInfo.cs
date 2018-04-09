using System.Collections.Generic;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public struct PathInfo
    {
        #region Properties

        /// <summary>
        ///     Stadt Position
        /// </summary>
        public Point StadtPos { get; }

        /// <summary>
        ///     Wurde Stadt bereits gefunden
        /// </summary>
        public bool StadtGefunden { get; set; }

        /// <summary>
        ///     Der zuletzt hinzugefuegte Weg
        /// </summary>
        public QuadratNode LetzterWeg => Weg[Weg.Count - 1];

        /// <summary>
        ///     Die Start node des Pathfinding
        /// </summary>
        public QuadratNode StartNode { get; }

        /// <summary>
        ///     Der Weg von Quax zur Stadt
        /// </summary>
        public List<QuadratNode> Weg { get; set; }

        #endregion

        #region Methods

        public PathInfo(Point stadtPos, QuadratNode startNode)
        {
            StadtPos = stadtPos;
            StadtGefunden = false;
            Weg = new List<QuadratNode> {startNode};
            StartNode = startNode;
        }

        #endregion
    }
}