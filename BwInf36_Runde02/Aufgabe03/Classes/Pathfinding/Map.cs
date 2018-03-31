using System;
using System.Collections.Generic;

namespace Aufgabe03.Classes.Pathfinding
{
    public class Map : NodeElement, ISearchPath
    {
        #region Fields



        #endregion

        #region Properties

        /// <summary>
        /// Die groesst moeglichen Quadrate auf der Map
        /// </summary>
        public List<QuadratNode> StartQuadrate { get; private set; }

        #endregion

        #region Methods

        public Map()
        {
            GetStartQuadrate();
        }

        public SearchInformation SearchPath(SearchInformation curStatus)
        {
            throw new NotImplementedException();
        }

        public void GetStartQuadrate()
        {
            StartQuadrate = new List<QuadratNode>();
            throw new NotImplementedException();
        }

        #endregion
    }
}