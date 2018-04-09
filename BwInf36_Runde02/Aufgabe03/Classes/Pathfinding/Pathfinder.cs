using System;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public class Pathfinder
    {
        #region Properties

        /// <summary>
        ///     Die Start "Node" / Map
        /// </summary>
        public Map Map { get; }

        /// <summary>
        ///     Die aktuelle Quax Position
        /// </summary>
        public Point QuaxPos { get; }

        /// <summary>
        ///     Die aktuelle Stadt Position
        /// </summary>
        public Point StadtPos { get; }

        #endregion

        #region Methods

        public Pathfinder(int quaxIndex)
        {
            Map = new Map();
            QuaxPos = MapDaten.Instance.QuaxPositionen[quaxIndex];
            StadtPos = MapDaten.Instance.StadtPosition;
        }

        /// <summary>
        ///     Sucht einen Weg von <see cref="QuaxPos" /> zu <see cref="StadtPos" />
        /// </summary>
        public PathInfo FindPath()
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine("Quax Search");
            Console.WriteLine("-------------------------");
            var quaxInfo = Map.SearchQuax(new QuaxInfo(QuaxPos));
            Console.WriteLine("-------------------------");
            Console.WriteLine("Path Search");
            Console.WriteLine("-------------------------");
            var pathInfo = Map.SearchPath(new PathInfo(StadtPos, quaxInfo.QuaxNode));
            return pathInfo;
        }

        #endregion
    }
}