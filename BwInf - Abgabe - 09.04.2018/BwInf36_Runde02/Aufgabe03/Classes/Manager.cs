using System.Windows.Media.Imaging;
using Aufgabe03.Classes.GUI;
using Aufgabe03.Classes.Pathfinding;

namespace Aufgabe03.Classes
{
    /// <summary>
    /// SINGLETON
    /// </summary>
    public class Manager
    {
        private static Manager _instance = null;

        public static Manager Instance => _instance ?? (_instance = new Manager());

        private Manager() { }

        public MainWindow MainWindow { get; set; }

        public Konsole Konsole { get; set; }

        public PositionTab CurPositionTab
        {
            get => MainWindow.Vm.PositionTabs[MainWindow.QuaxPosIndex];
            set => MainWindow.Vm.PositionTabs[MainWindow.QuaxPosIndex] = value;
        }

        public WriteableBitmap MapOverlay { get; set; }
    }
}