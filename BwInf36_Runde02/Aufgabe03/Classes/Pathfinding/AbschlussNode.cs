using System;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public class AbschlussNode : QuadratNode
    {
        #region Methods

        public AbschlussNode(Point luEckpunkt, int breite)
        {
            MapQuadrat = new MapQuadrat(luEckpunkt, breite);
        }

        public override QuaxInfo SearchQuax(QuaxInfo curStatus)
        {
            if (MapQuadrat.MapTyp == MapQuadrat.MapTypen.Unbekannt)
                if (NodeID == 0)
                    NodeID = MapQuadrat.GetMapTyp();
                else
                    MapQuadrat.GetMapTyp();

            Console.Write(
                $"[{NodeID}] Abschluss Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - ");

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
                    throw new Exception("Abschluss Node ist gemischt");

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
                $"[{NodeID}] Abschluss Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - ");

            switch (MapQuadrat.MapTyp)
            {
                case MapQuadrat.MapTypen.Wasser:
                    Console.WriteLine("Wasser");
                    return curStatus;

                case MapQuadrat.MapTypen.Passierbar:
                    Console.WriteLine("Passierbar");
                    if (curStatus.LetzterWeg != this)
                    {
                        if (BeruehrtPoint(curStatus.StadtPos))
                        {
                            curStatus.StadtGefunden = true;
                            Console.Write("STADT GEFUNDEN - ");
                        }

                        Console.WriteLine("Weg hinzugefuegt!");
                        curStatus.Weg.Add(this);
                    }

                    return curStatus;

                case MapQuadrat.MapTypen.Gemischt:
                    Console.WriteLine("Gemischt");
                    throw new Exception("Abschluss Node ist gemischt");

                case MapQuadrat.MapTypen.Unbekannt:
                    throw new Exception("Unbekannter Map Typ");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}