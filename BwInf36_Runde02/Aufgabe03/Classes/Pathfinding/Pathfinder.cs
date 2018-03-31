using System.Diagnostics;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public class Pathfinder
    {
        #region Fields

        private bool _isSearching;

        #endregion

        #region Properties

        /// <summary>
        /// Die Start "Node" / Map
        /// </summary>
        public Map StartNode { get; private set; }

        /// <summary>
        /// Die aktuelle Quax Position
        /// </summary>
        public Point QuaxPos { get; private set; }

        /// <summary>
        /// Die aktuelle Stadt Position
        /// </summary>
        public Point StadtPos { get; private set; }

        /// <summary>
        /// Wurde ein Weg zur Stadt gefunden
        /// </summary>
        public bool WegGefunden { get; private set; }

        /// <summary>
        /// Wird ein Weg gerade gesucht
        /// </summary>
        public bool IsSearching { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sucht einen Weg von <see cref="QuaxPos"/> zu <see cref="StadtPos"/>
        /// </summary>
        public void FindPath()
        {
            var info = new SearchInformation(QuaxPos, StadtPos);
            var endInfo = StartNode.SearchPath(info);
            Debug.WriteLine($"Quax gefunden: {endInfo.QuaxGefunden}");
            Debug.WriteLine($"Stadt gefunden: {endInfo.StadtGefunden}");
            Debug.WriteLine($"Weg Laenge: {endInfo.Weg.Count}");
        }

        #endregion
    }
}