using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aufgabe03.Classes.Pathfinding
{
    /// <summary>
    ///     SINGLETON | Beinhaltet alle Daten ueber die Map
    /// </summary>
    public class MapDaten
    {
        #region Fields

        private static MapDaten _instance;

        #endregion

        #region Properties

        public static MapDaten Instance => _instance ?? (_instance = new MapDaten());

        /// <summary>
        ///     Die Map
        /// </summary>
        public WriteableBitmap Map { get; set; }

        /// <summary>
        ///     Enthaelt alle Pixel der Map.
        ///     True wenn ein Pixel Wasser ist.
        ///     Beginnend oben links
        /// </summary>
        public bool[][] WasserPixel { get; private set; }

        /// <summary>
        ///     Die Quax Positionen
        /// </summary>
        public List<Point> QuaxPositionen { get; set; }

        /// <summary>
        ///     Die Stadt Position
        /// </summary>
        public Point StadtPosition { get; set; }

        #endregion

        #region Methods

        private MapDaten()
        {
        }

        public void LoadMapData(BitmapSource image)
        {
            Map = null;
            QuaxPositionen = new List<Point>();
            WasserPixel = new bool[image.PixelWidth][];

            Map = new WriteableBitmap(image);
            var curReihe = -1;
            var height = Map.PixelHeight;

            Map.ForEach((x, y, color) =>
            {
                if (x > curReihe)
                {
                    curReihe++;
                    WasserPixel[x] = new bool[height];
                }

                if (color.Equals(Colors.White))
                    WasserPixel[x][y] = true;
                else if (color.Equals(Colors.Red))
                    QuaxPositionen.Add(new Point(x, y));
                else if (color.Equals(Colors.Lime))
                    StadtPosition = new Point(x, y);

                return color;
            });
        }

        #endregion
    }
}