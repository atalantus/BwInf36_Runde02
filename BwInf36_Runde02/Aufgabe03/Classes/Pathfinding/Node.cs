using System;
using System.Diagnostics;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public class Node : QuadratNode
    {
        #region Properties

        /// <summary>
        /// Die Child Nodes dieser Node
        /// </summary>
        public ChildNodes ChildNodes { get; set; }

        #endregion

        #region Methods

        public Node(Point loEckpunkt, int breite)
        {
            MapQuadrat = new MapQuadrat(loEckpunkt, breite);
            CalculateChildNodes();
        }

        public override QuaxInfo SearchQuax(QuaxInfo curStatus)
        {
            Debug.Write($"Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - ");

            if (MapQuadrat.MapTyp == MapQuadrat.MapTypen.Unbekannt) MapQuadrat.GetMapTyp();

            switch (MapQuadrat.MapTyp)
            {
                case MapQuadrat.MapTypen.Wasser:
                    Debug.WriteLine("Wasser");
                    throw new Exception("Quax ist in Wasser Node");

                case MapQuadrat.MapTypen.Passierbar:
                    Debug.WriteLine("Passierbar");
                    Debug.WriteLine($"Quax gefunden: ({MapQuadrat.LO_Eckpunkt.X} | {MapQuadrat.LO_Eckpunkt.Y}) {MapQuadrat.Breite}");
                    curStatus.QuaxNode = this;
                    return curStatus;

                case MapQuadrat.MapTypen.Gemischt:
                    Debug.WriteLine("Gemischt");

                    QuadratNode nodeMitQuax = null;

                    for (var i = 0; i < ChildNodes.Nodes.Length; i++)
                    {
                        if (ChildNodes.Nodes[i].BeruehrtPoint(curStatus.QuaxPos))
                            nodeMitQuax = ChildNodes.Nodes[i];
                    }

                    if (nodeMitQuax == null)
                        throw new Exception("Quax ist in keinem inneren Quadrat");

                    return nodeMitQuax.SearchQuax(curStatus);

                case MapQuadrat.MapTypen.Unbekannt:
                    throw new Exception("Unbekannter Map Typ");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override PathInfo SearchPath(PathInfo curStatus)
        {
            Debug.Write($"Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - ");

            if (MapQuadrat.MapTyp == MapQuadrat.MapTypen.Unbekannt) MapQuadrat.GetMapTyp();

            switch (MapQuadrat.MapTyp)
            {
                case MapQuadrat.MapTypen.Wasser:
                    Debug.WriteLine("Wasser");
                    return curStatus;

                case MapQuadrat.MapTypen.Passierbar:
                    Debug.WriteLine("Passierbar");
                    if (BeruehrtPoint(curStatus.StadtPos))
                    {
                        curStatus.StadtGefunden = true;
                        Debug.Write("STADT GEFUNDEN - ");
                    }
                    Debug.WriteLine("Weg hinzugefuegt!");
                    curStatus.Weg.Add(this);
                    return curStatus;

                case MapQuadrat.MapTypen.Gemischt:
                    Debug.WriteLine("Gemischt");

                    ChildNodes.SortierChildNodes(curStatus.LetzterWeg, curStatus.StadtPos);

                    var fertig = false;
                    var counter = 0;
                    var isStartNodeParent = false;
                    for (int i = 0; i < ChildNodes.Nodes.Length; i++)
                    {
                        if (ChildNodes.Nodes[i] == curStatus.StartNode)
                            isStartNodeParent = true;
                    }
                    if (curStatus.Weg.Count == 1 && isStartNodeParent) counter = 1;
                    //var vorherigeWegLaenge = curStatus.Weg.Count;

                    while (counter < 2)
                    {
                        for (var i = 0; i < ChildNodes.ChildNodesSortiert.Length; i++)
                        {
                            var aktuelleNode = ChildNodes.ChildNodesSortiert[i].Node;
                            var aktuelleWegLaenge = curStatus.Weg.Count;

                            if (!aktuelleNode.BeruehrtQuadratNode(curStatus.LetzterWeg))
                                throw new Exception("Ueberpruefte node beruehrt nicht den Weg");

                            Debug.WriteLine($"Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - CHILD NODE UEBERPRUEFEN");
                            var info = aktuelleNode.SearchPath(curStatus);

                            if (info.StadtGefunden) // FERTIG
                                return info;

                            if (info.Weg.Count > aktuelleWegLaenge) // child node ist passierbar und wurde zu Weg hinzugefuegt
                            {
                                if (ChildNodes.ChildNodesSortiert[i].KuerzesteTargetEntfernung/* || info.Weg.Count > vorherigeWegLaenge + 1*/) // Durchquerbare child node gefunden --> node ist fertig
                                    return info;

                                // Weg hinzugefuegt
                                ChildNodes.SortierChildNodes(info.LetzterWeg, info.StadtPos);
                                counter++;

                                // Nochmal neu
                                break;
                            }

                            curStatus = info;
                        }
                    }

                    return curStatus;

                case MapQuadrat.MapTypen.Unbekannt:
                    throw new Exception("Unbekannter Map Typ");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Berechnet die child nodes dieser Node
        /// </summary>
        private void CalculateChildNodes()
        {
            var neueBreite = (int)Math.Ceiling(MapQuadrat.Breite / 2d);
            QuadratNode[] nodes;

            if (neueBreite > 2)
            {
                nodes = new QuadratNode[]
                {
                    new Node(
                        new Point(MapQuadrat.RU_Eckpunkt.X - neueBreite, MapQuadrat.LO_Eckpunkt.Y),
                        neueBreite),
                    new Node(new Point(MapQuadrat.RU_Eckpunkt.X - neueBreite, MapQuadrat.RU_Eckpunkt.Y - neueBreite),
                        neueBreite),
                    new Node(new Point(MapQuadrat.LO_Eckpunkt.X, MapQuadrat.RU_Eckpunkt.Y - neueBreite), neueBreite),
                    new Node(new Point(MapQuadrat.LO_Eckpunkt.X, MapQuadrat.LO_Eckpunkt.Y),
                        neueBreite)
                };
            }
            else
            {
                nodes = new QuadratNode[]
                {
                    new AbschlussNode(
                        new Point(MapQuadrat.RU_Eckpunkt.X - neueBreite, MapQuadrat.LO_Eckpunkt.Y),
                        neueBreite), 
                    new AbschlussNode(new Point(MapQuadrat.RU_Eckpunkt.X - neueBreite, MapQuadrat.RU_Eckpunkt.Y - neueBreite),
                        neueBreite),
                    new AbschlussNode(new Point(MapQuadrat.LO_Eckpunkt.X, MapQuadrat.RU_Eckpunkt.Y - neueBreite), neueBreite),
                    new AbschlussNode(new Point(MapQuadrat.LO_Eckpunkt.X, MapQuadrat.LO_Eckpunkt.Y),
                        neueBreite)
                };
            }

            ChildNodes = new ChildNodes(nodes);
        }

        #endregion
    }
}