using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Aufgabe03.Classes
{
    /// <summary>
    /// Die verschiedenen Zustaende der Auschnitte der Map
    /// </summary>
    public enum MapTypen
    {
        UNBEKANNT,
        LAND,
        WASSER,
        GEMISCHT
    }

    /// <summary>
    /// Enthaelt Informationen ueber einen quadratischen Ausschnitt der Map
    /// </summary>
    public class Quadrat
    {
        /// <summary>
        /// Der Map Typ des Quadrats
        /// </summary>
        public MapTypen Typ { get; private set; }

        /// <summary>
        /// Die Eckpunkte des Quadrats im Uhrzeigersinn, beginnend oben rechts
        /// </summary>
        public Vector2[] Eckpunkte { get; private set; }

        /// <summary>
        /// Die Breite des Quadrats
        /// </summary>
        public int Breite { get; set; }
    }
}
