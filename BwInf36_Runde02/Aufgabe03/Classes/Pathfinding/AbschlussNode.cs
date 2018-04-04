using System;
using System.Diagnostics;
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
            Debug.Write($"Abschluss Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - ");

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
                    throw new Exception("Abschluss Node ist gemischt");

                case MapQuadrat.MapTypen.Unbekannt:
                    throw new Exception("Unbekannter Map Typ");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override PathInfo SearchPath(PathInfo curStatus)
        {
            Debug.Write($"Abschluss Node: ({MapQuadrat.LO_Eckpunkt.X}|{MapQuadrat.LO_Eckpunkt.Y}) -> ({MapQuadrat.RU_Eckpunkt.X}|{MapQuadrat.RU_Eckpunkt.Y}) - ");

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