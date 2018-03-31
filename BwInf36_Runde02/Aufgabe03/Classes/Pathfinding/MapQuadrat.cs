using System;
using System.Numerics;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    /// <inheritdoc />
    /// <summary>
    /// Repraesentiert einen quadratischen Ausschnitt der Map
    /// </summary>
    public class MapQuadrat : Quadrat
    {
        #region Fields

        public enum MapTypen
        {
            Unbekannt,
            Gemischt,
            Passierbar,
            Wasser
        }

        #endregion

        #region Properties

        public MapTypen MapTyp { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Erzeugt ein neues <see cref="MapQuadrat"/> Objekt
        /// </summary>
        /// <param name="luEckpunkt">Eckpunkt links unten des Quadrats</param>
        /// <param name="breite">Breite/Hoehe des Quadrats</param>
        public MapQuadrat(Point luEckpunkt, int breite) : base(luEckpunkt, breite)
        {

        }

        /// <summary>
        /// Findet den Map Typ fuer den Ausschnitt von <see cref="MapQuadrat"/>
        /// HACK: REPRAESENTIERT EINEN DROHNEN FLUG
        /// TODO: REPRAESENTIERT EINEN DROHNEN FLUG
        /// </summary>
        public void GetMapTyp()
        {
            var mapDaten = MapDaten.Instance;

            //var pixel = mapDaten.Map.CopyPixels(new Int32Rect(LU_Eckpunkt.X, LU_Eckpunkt.Y + Hoehe, Breite, Hoehe), pixel, )
        }

        #endregion
    }
}