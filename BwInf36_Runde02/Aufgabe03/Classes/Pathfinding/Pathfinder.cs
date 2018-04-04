using System.Diagnostics;
using System.Windows;
using Aufgabe03.Classes.GUI;

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
        public Map Map { get; private set; }

        /// <summary>
        /// Der aktuelle GUI Tab
        /// </summary>
        public PositionTab AktuellerTab { get; set; }

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

        public Pathfinder(int quaxIndex)
        {
            Map = new Map();
            QuaxPos = MapDaten.Instance.QuaxPositionen[quaxIndex];
            StadtPos = MapDaten.Instance.StadtPosition;
        }

        /// <summary>
        /// Sucht einen Weg von <see cref="QuaxPos"/> zu <see cref="StadtPos"/>
        /// </summary>
        public PathInfo FindPath()
        {
            IsSearching = true;
            Debug.WriteLine("-------------------------");
            Debug.WriteLine("Quax Search");
            Debug.WriteLine("-------------------------");
            var quaxInfo = Map.SearchQuax(new QuaxInfo(QuaxPos));
            Debug.WriteLine("-------------------------");
            Debug.WriteLine("Path Search");
            Debug.WriteLine("-------------------------");
            var pathInfo = Map.SearchPath(new PathInfo(StadtPos, quaxInfo.QuaxNode));
            IsSearching = false;
            return pathInfo;
        }

        #endregion
    }
}