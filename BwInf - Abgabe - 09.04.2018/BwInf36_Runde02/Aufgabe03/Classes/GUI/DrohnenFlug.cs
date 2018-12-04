using System;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Aufgabe03.Classes.Pathfinding;

namespace Aufgabe03.Classes.GUI
{
    public class DrohnenFlug
    {
        /// <summary>
        /// Der wievielte Drohnen Flug ist das
        /// </summary>
        public int FlugId { get; }

        public MapQuadrat MapQuadrat { get; }

        public string MapTyp => MapQuadrat.MapTyp.ToString();

        public DrohnenFlug(int flugId, MapQuadrat mapQuadrat)
        {
            FlugId = flugId;
            MapQuadrat = mapQuadrat;

            DrawMapSquare();
        }

        public void DrawMapSquare()
        {
            var mapOverlay = Manager.Instance.MapOverlay;
            var OverlayZoomLevel = Manager.Instance.MainWindow.OverlayZoomLevel;
            Color color;
            switch (MapQuadrat.MapTyp)
            {
                case MapQuadrat.MapTypen.Unbekannt:
                    throw new Exception("Unbekannter Map Typ");
                case MapQuadrat.MapTypen.Gemischt:
                    color = MapColors.GemischtPassiv;
                    break;
                case MapQuadrat.MapTypen.Passierbar:
                    color = MapColors.PassierbarPassiv;
                    break;
                case MapQuadrat.MapTypen.Wasser:
                    color = MapColors.WasserPassiv;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var loX = Convert.ToInt32(MapQuadrat.LO_Eckpunkt.X * OverlayZoomLevel);
            var loY = Convert.ToInt32(MapQuadrat.LO_Eckpunkt.Y * OverlayZoomLevel);
            var ruX = Convert.ToInt32(MapQuadrat.RU_Eckpunkt.X * OverlayZoomLevel);
            var ruY = Convert.ToInt32(MapQuadrat.RU_Eckpunkt.Y * OverlayZoomLevel);

            mapOverlay.FillRectangle(loX, loY, ruX, ruY, color);
            mapOverlay.DrawRectangle(loX, loY, ruX, ruY, Colors.Yellow);
        }

        public void HighlightMapSquare()
        {
            var mapOverlay = Manager.Instance.MapOverlay;
            var OverlayZoomLevel = Manager.Instance.MainWindow.OverlayZoomLevel;
            Color color;
            switch (MapQuadrat.MapTyp)
            {
                case MapQuadrat.MapTypen.Unbekannt:
                    throw new Exception("Unbekannter Map Typ");
                case MapQuadrat.MapTypen.Gemischt:
                    color = MapColors.GemischtAktiv;
                    break;
                case MapQuadrat.MapTypen.Passierbar:
                    color = MapColors.PassierbarAktiv;
                    break;
                case MapQuadrat.MapTypen.Wasser:
                    color = MapColors.WasserAktiv;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var loX = Convert.ToInt32(MapQuadrat.LO_Eckpunkt.X * OverlayZoomLevel);
            var loY = Convert.ToInt32(MapQuadrat.LO_Eckpunkt.Y * OverlayZoomLevel);
            var ruX = Convert.ToInt32(MapQuadrat.RU_Eckpunkt.X * OverlayZoomLevel);
            var ruY = Convert.ToInt32(MapQuadrat.RU_Eckpunkt.Y * OverlayZoomLevel);

            mapOverlay.FillRectangle(loX, loY, ruX, ruY, color);
            mapOverlay.DrawRectangle(loX, loY, ruX, ruY, Colors.DeepPink);
        }
    }
}