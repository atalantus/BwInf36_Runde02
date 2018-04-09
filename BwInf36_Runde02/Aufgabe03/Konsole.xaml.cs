using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Aufgabe03.Classes.GUI;

namespace Aufgabe03
{
    /// <summary>
    /// Interaction logic for Konsole.xaml
    /// </summary>
    public partial class Konsole : Window
    {
        private TextBoxOutputter outputter;

        public Konsole()
        {
            InitializeComponent();

            outputter = new TextBoxOutputter(ConsoleTextBox);
            Console.SetOut(outputter);
        }
    }
}
