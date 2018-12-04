using System;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public class Node : QuadratNode
    {
        #region Properties

        /// <summary>
        ///     Die Child Nodes dieser Node
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
            if (MapQuadrat.MapTyp == MapQuadrat.MapTypen.Unbekannt)
                if (NodeID == 0)
                    NodeID = MapQuadrat.GetMapTyp();
                else
                    MapQuadrat.GetMapTyp();

            Console.Write(
                $"[{NodeID}] Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - ");

            switch (MapQuadrat.MapTyp)
            {
                case MapQuadrat.MapTypen.Wasser:
                    Console.WriteLine("Wasser");
                    throw new Exception("Quax ist in Wasser Node");

                case MapQuadrat.MapTypen.Passierbar:
                    Console.WriteLine("Passierbar");
                    Console.WriteLine(
                        $"Quax gefunden: ({MapQuadrat.LO_Eckpunkt.X} | {MapQuadrat.LO_Eckpunkt.Y}) {MapQuadrat.Breite}");
                    curStatus.QuaxNode = this;
                    return curStatus;

                case MapQuadrat.MapTypen.Gemischt:
                    Console.WriteLine("Gemischt");

                    QuadratNode nodeMitQuax = null;

                    for (var i = 0; i < ChildNodes.Nodes.Length; i++)
                        if (ChildNodes.Nodes[i].BeruehrtPoint(curStatus.QuaxPos))
                            nodeMitQuax = ChildNodes.Nodes[i];

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
            if (MapQuadrat.MapTyp == MapQuadrat.MapTypen.Unbekannt)
                if (NodeID == 0)
                    NodeID = MapQuadrat.GetMapTyp();
                else
                    MapQuadrat.GetMapTyp();

            Console.Write(
                $"[{NodeID}] Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - ");

            switch (MapQuadrat.MapTyp)
            {
                case MapQuadrat.MapTypen.Wasser:
                    Console.WriteLine("Wasser");
                    return curStatus;

                case MapQuadrat.MapTypen.Passierbar:
                    Console.WriteLine("Passierbar");

                    if (BeruehrtPoint(curStatus.StadtPos))
                    {
                        curStatus.StadtGefunden = true;
                        Console.Write("STADT GEFUNDEN - ");
                    }

                    if (curStatus.LetzterWeg != this)
                    {
                        Console.WriteLine("Weg hinzugefuegt!");
                        curStatus.Weg.Add(this);
                    }

                    return curStatus;

                case MapQuadrat.MapTypen.Gemischt:
                    Console.WriteLine("Gemischt");

                    ChildNodes.SortierChildNodes(curStatus.LetzterWeg, curStatus.StadtPos);

                    var gefundeneQuadrate = 0;
                    var isStartNodeParent = false;
                    for (var i = 0; i < ChildNodes.Nodes.Length; i++)
                        if (ChildNodes.Nodes[i] == curStatus.StartNode)
                            isStartNodeParent = true;
                    if (curStatus.Weg.Count == 1 && isStartNodeParent) gefundeneQuadrate = 1;

                    while (gefundeneQuadrate < 2)
                        for (var i = 0; i < ChildNodes.ChildNodesSortiert.Length; i++)
                        {
                            var aktuelleNode = ChildNodes.ChildNodesSortiert[i].Node;
                            var aktuelleWegLaenge = curStatus.Weg.Count;

                            if (!aktuelleNode.BeruehrtQuadratNode(curStatus.LetzterWeg)) // Beruehrt nicht den Weg
                                return
                                    curStatus; // Weil es keinen Weg gibt (Zur Sicherheit andere nodes auch noch checken)

                            //Console.WriteLine(
                            //    $"[{NodeID}] Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - CHILD NODE UEBERPRUEFEN");
                            var info = aktuelleNode.SearchPath(curStatus);

                            if (info.StadtGefunden) // FERTIG
                                return info;

                            if (info.Weg.Count > aktuelleWegLaenge
                            ) // child node ist passierbar und wurde zu Weg hinzugefuegt
                            {
                                if (ChildNodes.ChildNodesSortiert[i]
                                        .KuerzesteTargetEntfernung /* || info.Weg.Count > vorherigeWegLaenge + 1*/
                                ) // Durchquerbare child node gefunden --> node ist fertig
                                    return info;

                                // Weg hinzugefuegt
                                ChildNodes.SortierChildNodes(info.LetzterWeg, info.StadtPos);
                                gefundeneQuadrate++;

                                // Nochmal neu
                                break;
                            }

                            if (i == ChildNodes.ChildNodesSortiert.Length - 1)
                            {
                                // Dieses Node und all ihre child nodes helfen nicht weiter
                                Console.WriteLine(
                                    $"[{NodeID}] Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - HILFT NICHT WEITER");
                                return info;
                            }

                            curStatus = info;
                        }

                    return curStatus;

                case MapQuadrat.MapTypen.Unbekannt:
                    throw new Exception("Unbekannter Map Typ");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Berechnet die child nodes dieser Node
        /// </summary>
        private void CalculateChildNodes()
        {
            var neueBreite = (int) Math.Ceiling(MapQuadrat.Breite / 2d);
            QuadratNode[] nodes;

            if (neueBreite > 2)
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
            else
                nodes = new QuadratNode[]
                {
                    new AbschlussNode(
                        new Point(MapQuadrat.RU_Eckpunkt.X - neueBreite, MapQuadrat.LO_Eckpunkt.Y),
                        neueBreite),
                    new AbschlussNode(
                        new Point(MapQuadrat.RU_Eckpunkt.X - neueBreite, MapQuadrat.RU_Eckpunkt.Y - neueBreite),
                        neueBreite),
                    new AbschlussNode(new Point(MapQuadrat.LO_Eckpunkt.X, MapQuadrat.RU_Eckpunkt.Y - neueBreite),
                        neueBreite),
                    new AbschlussNode(new Point(MapQuadrat.LO_Eckpunkt.X, MapQuadrat.LO_Eckpunkt.Y),
                        neueBreite)
                };

            ChildNodes = new ChildNodes(nodes);
        }

        #endregion
    }
}