using System.Windows.Media;

namespace Aufgabe03.Classes
{
    public static class MapColors
    {
        public static readonly Color PassierbarAktiv = Color.FromArgb(255, 0, 200, 0);
        public static readonly Color WasserAktiv = Color.FromArgb(255, 200, 0, 0);
        public static readonly Color GemischtAktiv = Color.FromArgb(255, 200, 200, 50);
        public static readonly Color PassierbarPassiv = Color.FromArgb(100, 50, 255, 50);
        public static readonly Color WasserPassiv = Color.FromArgb(125, 255, 50, 50);
        public static readonly Color GemischtPassiv = Color.FromArgb(150, 200, 200, 50);
    }
}