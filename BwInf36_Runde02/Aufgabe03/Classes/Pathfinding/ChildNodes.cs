using System;
using System.Linq;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    /// <summary>
    /// Struktur zur speicherung der 4 child nodes einer Node
    /// </summary>
    public class ChildNodes
    {
        #region Fields

        private readonly QuadratNode[] _nodes;

        #endregion

        #region Properties

        /// <summary>
        /// Die child node oben rechts
        /// </summary>
        public QuadratNode NO_Node => _nodes[0];
        /// <summary>
        /// Die child node unten rechts
        /// </summary>
        public QuadratNode SO_Node => _nodes[1];
        /// <summary>
        /// Die child node unten links
        /// </summary>
        public QuadratNode SW_Node => _nodes[2];
        /// <summary>
        /// Die child node oben links
        /// </summary>
        public QuadratNode NW_Node => _nodes[3];

        /// <summary>
        /// Enthaelt die child nodes sortiert nach Angrenzung an den letzten Weg
        /// </summary>
        public ChildNodeSortiert[] ChildNodesSortiert { get; private set; }

        /// <summary>
        /// Sind die child nodes fuer eine Suche nach Quax sortiert
        /// </summary>
        public bool SortiertFuerQuax { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Erstellt eine neue <see cref="ChildNodes"/> Struktur
        /// </summary>
        /// <param name="childNodes">Die child nodes im Uhrzeigersinn. Beginnend oben rechts</param>
        /// <param name="letzterWeg">Die zuletzt gesetzte Weg Node</param>
        /// <param name="target">Das aktuelle Such Target</param>
        public ChildNodes(QuadratNode[] childNodes, QuadratNode letzterWeg, Point target)
        {
            _nodes = childNodes;
            SortierChildNodes(letzterWeg, target);
        }

        /// <summary>
        /// Sortiert die child nodes nach Prioritaet
        /// </summary>
        /// <param name="letzterWeg">Die zuletzt gesetzte Weg Node</param>
        /// <param name="target">Das aktuelle Such Target</param>
        private void SortierChildNodes(QuadratNode letzterWeg, Point target)
        {
            ChildNodesSortiert = new ChildNodeSortiert[_nodes.Length];        
            for (var i = 0; i < _nodes.Length; i++)
            {
                ChildNodesSortiert[i] = new ChildNodeSortiert(_nodes[i], letzterWeg, target);
            }

            Array.Sort(ChildNodesSortiert);

            var kuerzesteEntfernungIndex = 0;
            for (var i = 0; i < ChildNodesSortiert.Length; i++)
            {
                if (ChildNodesSortiert[i].EntfernungTarget <
                    ChildNodesSortiert[kuerzesteEntfernungIndex].EntfernungTarget)
                    kuerzesteEntfernungIndex = i;
            }

            ChildNodesSortiert[kuerzesteEntfernungIndex].KuerzesteTargetEntfernung = true;
        }

        #endregion
    }

    /// <summary>
    /// Eine child node mit ihrer Entfernung zum Target im Verleich zu den anderen child nodes
    /// </summary>
    public struct ChildNodeSortiert : IComparable
    {
        /// <summary>
        /// Node der child node
        /// </summary>
        public QuadratNode Node { get; private set; }

        /// <summary>
        /// Entfernung zum Ziel
        /// </summary>
        public double EntfernungTarget { get; private set; }

        /// <summary>
        /// Geringste Entfernung zum Ziel im Vergleich mit anderen child nodes
        /// </summary>
        public bool KuerzesteTargetEntfernung { get; set; }

        /// <summary>
        /// Die zuletzt gesetzte Node fuer den Weg
        /// </summary>
        public QuadratNode LetzterWeg { get; set; }

        /// <summary>
        /// Das aktuelle Ziel
        /// </summary>
        public Point Target { get; set; }

        public ChildNodeSortiert(QuadratNode node, QuadratNode letzterWeg, Point target)
        {
            Node = node;
            LetzterWeg = letzterWeg;
            Target = target;
            EntfernungTarget = Utilities.EntfernungBerechnen(Node.MapQuadrat, Target);
            KuerzesteTargetEntfernung = false;
        }

        /// <summary>
        /// Wurde diesem Struct schon Werte zugewiesen
        /// </summary>
        /// <returns>True wenn diesem Struct noch keine Werte zugewiesen wurde</returns>
        public bool Empty()
        {
            return Node == null && EntfernungTarget == 0d && LetzterWeg == null && Target == null && !KuerzesteTargetEntfernung;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var otherNode = obj is ChildNodeSortiert ? (ChildNodeSortiert) obj : new ChildNodeSortiert();
            if (!otherNode.Empty())
            {
                // 1. Sortieren nach Beruehrung des Weges
                var beruehrtLetztenWeg = Node.BeruehrtQuadratNode(LetzterWeg);
                var otherBeruehrtLetztenWeg = otherNode.Node.BeruehrtQuadratNode(LetzterWeg);
                if (beruehrtLetztenWeg && !otherBeruehrtLetztenWeg) return -1;
                if (!beruehrtLetztenWeg && otherBeruehrtLetztenWeg) return 1;
                // 2. Sortieren nach Entfernung zu Ziel
                if (EntfernungTarget <= otherNode.EntfernungTarget) return -1;
                return 1;

            }

            throw new ArgumentException("Object ist nicht vom Typ ChildNodeSortiert");
        }
    }
}