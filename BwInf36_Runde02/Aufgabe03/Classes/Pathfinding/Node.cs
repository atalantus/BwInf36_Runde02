using System;
using System.Diagnostics;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public class Node : QuadratNode, ISearchPath
    {
        #region Properties

        /// <summary>
        /// Die Child Nodes dieser Node
        /// </summary>
        private ChildNodes ChildNodes { get; set; }

        #endregion

        #region Methods

        public Node(Point luEckpunkt, int breite) : base(luEckpunkt, breite) { }

        public new SearchInformation SearchPath(SearchInformation curStatus)
        {
            if (MapQuadrat.MapTyp == MapQuadrat.MapTypen.Unbekannt) MapQuadrat.GetMapTyp();

            switch (MapQuadrat.MapTyp)
            {
                case MapQuadrat.MapTypen.Wasser:
                    Debug.WriteLine("Wasser");
                    return curStatus;
                case MapQuadrat.MapTypen.Passierbar:
                    Debug.Write("Passierbar | ");
                    if (!curStatus.QuaxGefunden)
                    {
                        if (BeruehrtPoint(curStatus.QuaxPos))
                        {
                            // Quax ist in diesem Abschnitt
                            Debug.WriteLine("QUAX GEFUNDEN");
                            curStatus.QuaxGefunden = true;
                            curStatus.Weg.Add(this);
                        }
                        else
                        {
                            throw new Exception("Quax ist nicht in erwarteter Node");
                        }
                    }
                    else
                    {
                        if (BeruehrtPoint(curStatus.StadtPos))
                        {
                            // Stadt ist in diesem Abschnitt
                            Debug.WriteLine("STADT GEFUNDEN");
                            curStatus.StadtGefunden = true;
                            curStatus.Weg.Add(this);
                        }
                        else
                        {
                            // Weg fuehrt durch diese Node
                            Debug.WriteLine("Weg hinzugefuegt");
                            curStatus.Weg.Add(this);
                        }
                    }

                    return curStatus;
                case MapQuadrat.MapTypen.Gemischt:
                    Debug.WriteLine("Gemischt");
                    // Aktuelles Such Ziel
                    var target = !curStatus.QuaxGefunden ? curStatus.QuaxPos : curStatus.StadtPos;
                    var letzterWeg = curStatus.Weg[curStatus.Weg.Count - 1];

                    if (ChildNodes == null || (ChildNodes.SortiertFuerQuax && curStatus.QuaxGefunden))
                        CalculateChildNodes(curStatus.Weg[curStatus.Weg.Count - 1], target);

                    var aktuelleWegLaenge = curStatus.Weg.Count;

                    for (var i = 0; i < ChildNodes.ChildNodesSortiert.Length; i++)
                    {
                        var aktuelleNode = ChildNodes.ChildNodesSortiert[i].Node;

                        if (!aktuelleNode.BeruehrtQuadratNode(letzterWeg)) continue; // Node beruehrt nicht den Weg

                        var info = aktuelleNode.SearchPath(curStatus);

                        if (info.StadtGefunden) // Stadt gefunden => Fertig
                            return info;
                        if (info.QuaxGefunden && !curStatus.QuaxGefunden) // Quax gefunden
                            return SearchPath(info); // Starte Suche nach Stadt
                        if (info.Weg.Count > aktuelleWegLaenge)
                        {
                            // Weg hinzugefuegt
                            if (ChildNodes.ChildNodesSortiert[i].KuerzesteTargetEntfernung)
                            {
                                // Beste child node ist passierbar
                            }
                        }
                    }
                    throw new Exception("Sollte nicht passieren");
                case MapQuadrat.MapTypen.Unbekannt:
                    throw new Exception("Unerwarteter Map Typ");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Berechnet die child nodes dieser Node
        /// </summary>
        /// <param name="letzterWeg">Die zuletzt zum Weg hinzugefuegte <see cref="QuadratNode"/></param>
        /// <param name="target">Das aktuelle Ziel der Suche</param>
        private void CalculateChildNodes(QuadratNode letzterWeg, Point target)
        {
            var neueBreite = (int)Math.Ceiling(MapQuadrat.Breite / 2d);

            var nodes = new[]
            {
                new QuadratNode(new Point(MapQuadrat.RO_Eckpunkt.X - neueBreite, MapQuadrat.RO_Eckpunkt.Y - neueBreite), neueBreite),
                new QuadratNode(new Point(MapQuadrat.LU_Eckpunkt.X + neueBreite, MapQuadrat.LU_Eckpunkt.Y), neueBreite),
                new QuadratNode(new Point(MapQuadrat.LU_Eckpunkt.X, MapQuadrat.LU_Eckpunkt.Y), neueBreite),
                new QuadratNode(new Point(MapQuadrat.LU_Eckpunkt.X, MapQuadrat.LU_Eckpunkt.Y + neueBreite), neueBreite)
            };

            ChildNodes = new ChildNodes(nodes, letzterWeg, target);
        }

        #endregion
    }
}