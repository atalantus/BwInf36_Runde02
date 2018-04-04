using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Aufgabe03.Classes.GUI
{
    public class PositionTab
    {
        /// <summary>
        /// Der Index der Quax Position
        /// </summary>
        public int QuaxPosIndex { get; private set; }

        /// <summary>
        /// Der 'Start Algorithmus' Button
        /// </summary>
        public Button StartAlgorithmBtn { get; }

        /// <summary>
        /// Die Drohnen Fluege
        /// </summary>
        public List<DrohnenFlug> DrohnenFluege { get; set; }

        public PositionTab(int quaxIndex)
        {
            QuaxPosIndex = quaxIndex;
            DrohnenFluege = new List<DrohnenFlug>();
        }
    }
}