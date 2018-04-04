using System.Windows.Media.Imaging;

namespace Aufgabe03.Classes
{
    public class Utilities
    {
        public static WriteableBitmap ResizeMap(WriteableBitmap map)
        {
            var w = map.PixelWidth;
            var h = map.PixelHeight;

            if (!IsEven(w)) w++;
            if (!IsEven(h)) h++;

            if (w > h) h = w;
            else if (h > w) w = h;

            return map.Resize(w, h, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
        }

        public static bool IsEven(int n)
        {
            return n % 2 == 0;
        }
    }
}