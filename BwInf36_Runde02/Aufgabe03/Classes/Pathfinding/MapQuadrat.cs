using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media.Imaging;

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
        /// <param name="loEckpunkt">Eckpunkt links unten des Quadrats</param>
        /// <param name="breite">Breite/Hoehe des Quadrats</param>
        public MapQuadrat(Point loEckpunkt, int breite) : base(loEckpunkt, breite)
        {

        }

        /// <summary>
        /// Findet den Map Typ fuer den Ausschnitt von <see cref="MapQuadrat"/>
        /// TODO: REPRAESENTIERT EINEN DROHNEN FLUG
        /// </summary>
        public void GetMapTyp()
        {
            var mapDaten = MapDaten.Instance;
            var enthaeltWasser = false;
            var enthaeltLand = false;

            for (var i = 0; i < Breite; i++)
            {
                for (var j = 0; j < Hoehe; j++)
                {
                    var wasserPixel = mapDaten.WasserPixel[(int) LO_Eckpunkt.X + i][(int) LO_Eckpunkt.Y + j];

                    if (wasserPixel && !enthaeltWasser)
                        enthaeltWasser = true;
                    else if (!wasserPixel && !enthaeltLand)
                        enthaeltLand = true;
                }
            }

            if (enthaeltWasser && enthaeltLand && Breite > 2)
                MapTyp = MapTypen.Gemischt;
            else if (enthaeltWasser && enthaeltLand && Breite <= 2 || !enthaeltWasser)
                MapTyp = MapTypen.Passierbar;
            else
                MapTyp = MapTypen.Wasser;
        }

        #endregion
    }
}