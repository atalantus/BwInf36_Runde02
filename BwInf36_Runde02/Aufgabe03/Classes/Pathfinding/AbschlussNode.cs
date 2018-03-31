using System;
using System.Windows;

namespace Aufgabe03.Classes.Pathfinding
{
    public class AbschlussNode : QuadratNode, ISearchPath
    {
        #region Methods

        public AbschlussNode(Point luEckpunkt, int breite) : base(luEckpunkt, breite)
        {

        }

        public new SearchInformation SearchPath(SearchInformation curStatus)
        {
            if (MapQuadrat.MapTyp == MapQuadrat.MapTypen.Unbekannt) MapQuadrat.GetMapTyp();

            switch (MapQuadrat.MapTyp)
            {
                case MapQuadrat.MapTypen.Wasser:
                    // Node ist nicht passierbar
                    return curStatus;
                case MapQuadrat.MapTypen.Passierbar:
                    if (!curStatus.QuaxGefunden)
                    {
                        if (BeruehrtPoint(curStatus.QuaxPos))
                        {
                            // Quax ist in diesem Abschnitt
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
                            curStatus.StadtGefunden = true;
                            curStatus.Weg.Add(this);
                        }
                        else
                        {
                            // Weg fuehrt durch diese Node
                            curStatus.Weg.Add(this);
                        }
                    }

                    return curStatus;
                case MapQuadrat.MapTypen.Unbekannt:
                    throw new Exception("Unerwarteter Map Typ");
                case MapQuadrat.MapTypen.Gemischt:
                    throw new Exception("Abschluss Node ist Gemischt");
                default:
                    throw new ArgumentOutOfRangeException(nameof(curStatus));
            }

            throw new Exception($"Map Typ {MapQuadrat.MapTyp} wurde nicht implementiert");
        }

        #endregion
    }
}