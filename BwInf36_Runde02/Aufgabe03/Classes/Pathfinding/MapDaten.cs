using System.Diagnostics;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Aufgabe03.Classes.Pathfinding
{
    /// <summary>
    /// SINGLETON | Beinhaltet alle Daten ueber die Map
    /// </summary>
    public class MapDaten
    {
        #region Singleton

        private static MapDaten _instance = null;
        public static MapDaten Instance => _instance ?? (_instance = new MapDaten());
        private MapDaten() { }

        #endregion

        #region Fields

        private WriteableBitmap _map;

        #endregion

        #region Properties

        public WriteableBitmap Map
        {
            get => _map;
            set => SetMap(value);
        }

        #endregion

        #region Methods

        public void SetMap(WriteableBitmap map)
        {
            Debug.WriteLine(map.PixelWidth + "x" + map.PixelHeight);
            _map = map;
        }

        #endregion
    }
}