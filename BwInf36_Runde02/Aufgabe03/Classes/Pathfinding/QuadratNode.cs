using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public abstract class QuadratNode : NodeElement
    {
        #region Properties

        /// <summary>
        ///     Der Ausschnitt der Map, den die Node einschliesst
        /// </summary>
        public MapQuadrat MapQuadrat { get; set; }

        /// <summary>
        ///     Die ID der node
        /// </summary>
        public int NodeID { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Ueberprueft, ob eine <see cref="QuadratNode" /> eine andere <see cref="QuadratNode" /> beruehrt
        /// </summary>
        /// <param name="other">Die andere Node</param>
        /// <returns>True wenn sich beide Nodes beruehren</returns>
        public bool BeruehrtQuadratNode(QuadratNode other)
        {
            return MapQuadrat.BeruehrtQuadrat(other.MapQuadrat);
        }

        /// <summary>
        ///     Ueberprueft, ob eine <see cref="QuadratNode" /> einen <see cref="Point" /> beruehrt
        /// </summary>
        /// <param name="other">Der Punkt</param>
        /// <returns>True wenn sich beide beruehren</returns>
        public bool BeruehrtPoint(Point other)
        {
            return MapQuadrat.BeruehrtPoint(other);
        }

        #endregion
    }
}