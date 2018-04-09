using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public class Map : NodeElement
    {
        #region Properties

        /// <summary>
        ///     Die groesst moeglichen Quadrate auf der Map
        /// </summary>
        public List<QuadratNode> StartQuadrate { get; private set; }

        #endregion

        #region Fields

        #endregion

        #region Methods

        public Map()
        {
            GetStartQuadrate();
        }

        public override QuaxInfo SearchQuax(QuaxInfo curStatus)
        {
            QuadratNode quaxStartNode = null;
            for (var i = 0; i < StartQuadrate.Count; i++)
                if (StartQuadrate[i].BeruehrtPoint(curStatus.QuaxPos))
                    quaxStartNode = StartQuadrate[i];

            if (quaxStartNode != null)
            {
                Debug.Write("START QUADRAT:    ");
                var info = quaxStartNode.SearchQuax(curStatus);
                if (info.QuaxNode == null)
                    info.QuaxNode = quaxStartNode;
                return info;
            }

            throw new Exception("Quax ist in keinem der Start Quadrate");
        }

        public override PathInfo SearchPath(PathInfo curStatus)
        {
            // Suche Start QuadratNode
            QuadratNode startQuadrat = null;
            for (var i = 0; i < StartQuadrate.Count; i++)
                if (StartQuadrate[i].BeruehrtQuadratNode(curStatus.LetzterWeg))
                    startQuadrat = StartQuadrate[i];

            var status = curStatus;
            var aktuellesQuadrat = startQuadrat;

            if (startQuadrat != null)
                while (!status.StadtGefunden)
                {
                    Debug.Write("START QUADRAT:    ");

                    status = aktuellesQuadrat.SearchPath(status);

                    if (status.StadtGefunden) // FERTIG
                        return status;

                    // Sonst naechstes Start Quadrat

                    var tempQuadrate01 = new List<QuadratNode>(); // Quadrate die den letzten Weg beruehren
                    for (var i = 0; i < StartQuadrate.Count; i++)
                        if (StartQuadrate[i].BeruehrtQuadratNode(status.LetzterWeg))
                            tempQuadrate01.Add(StartQuadrate[i]);

                    var neuesQuadratGefunden = false;
                    for (var i = 0; i < tempQuadrate01.Count; i++)
                        if (Utilities.EntfernungBerechnen(tempQuadrate01[i].MapQuadrat, status.StadtPos) <
                            Utilities.EntfernungBerechnen(aktuellesQuadrat.MapQuadrat, status.StadtPos))
                        {
                            aktuellesQuadrat = tempQuadrate01[i];
                            neuesQuadratGefunden = true;
                        }

                    if (!neuesQuadratGefunden) return status;
                }

            throw new Exception("Start Node ist in keinem der Start Quadrate");
        }

        /// <summary>
        ///     Fuellt eine rechteckige Map mit gleichen, moeglichst grossen Quadraten
        /// </summary>
        public void GetStartQuadrate()
        {
            StartQuadrate = new List<QuadratNode>();
            var mapBreite = MapDaten.Instance.Map.PixelWidth;
            var mapHoehe = MapDaten.Instance.Map.PixelHeight;
            var anzahlNodes = 0;
            var nodeBreite = 0;

            if (mapBreite > mapHoehe)
            {
                anzahlNodes = (int) Math.Ceiling((double) mapBreite / mapHoehe);
                nodeBreite = mapHoehe;
            }
            else if (mapBreite < mapHoehe)
            {
                //TODO: Map um 90° drehen
                throw new Exception("Map Hoehe ist groesser als Map Breite");
                //anzahlNodes = (int) Math.Ceiling((double) mapHoehe / mapBreite);
                //nodeBreite = mapBreite;
            }
            else
            {
                anzahlNodes = 1;
                nodeBreite = mapBreite;
            }

            var offsetL = 0;
            var offsetR = nodeBreite;
            for (var i = 0; i < anzahlNodes; i++)
                if (i % 2 == 0)
                {
                    if (nodeBreite > 2)
                        StartQuadrate.Add(new Node(new Point(offsetL, 0), nodeBreite));
                    else
                        StartQuadrate.Add(new AbschlussNode(new Point(offsetL, 0), nodeBreite));
                    offsetL += nodeBreite;
                }
                else
                {
                    if (nodeBreite > 2)
                        StartQuadrate.Add(new Node(new Point(mapBreite - offsetR, 0), nodeBreite));
                    else
                        StartQuadrate.Add(new AbschlussNode(new Point(mapBreite - offsetR, 0), nodeBreite));
                    offsetR += nodeBreite;
                }
        }

        #endregion
    }

    //public struct StartQuadrat : IComparable
    //{
    //    /// <summary>
    //    /// Node
    //    /// </summary>
    //    public QuadratNode Node { get; private set; }

    //    public StartQuadrat(QuadratNode node)
    //    {
    //        Node = node;
    //    }

    //    public bool Empty()
    //    {
    //        return Node == null;
    //    }

    //    public int CompareTo(object obj)
    //    {
    //        if (obj == null) return 1;
    //        var otherNode = obj is StartQuadrat ? (StartQuadrat) obj : new StartQuadrat();
    //        if (!otherNode.Empty())
    //        {
    //            // 1. Sortieren nach Beruehrung des Weges
    //            var beruehrtLetztenWeg = Node.BeruehrtQuadratNode(LetzterWeg);
    //            var otherBeruehrtLetztenWeg = otherNode.Node.BeruehrtQuadratNode(LetzterWeg);
    //            if (beruehrtLetztenWeg && !otherBeruehrtLetztenWeg) return -1;
    //            if (!beruehrtLetztenWeg && otherBeruehrtLetztenWeg) return 1;
    //            // 2. Sortieren nach Entfernung zu Ziel
    //            if (EntfernungTarget <= otherNode.EntfernungTarget) return -1;
    //            return 1;

    //        }

    //        throw new ArgumentException("Object ist nicht vom Typ ChildNodeSortiert");
    //    }
    //}
}