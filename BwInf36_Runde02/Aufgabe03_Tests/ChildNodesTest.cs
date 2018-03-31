using System;
using System.Diagnostics;
using System.Windows;
using Aufgabe03.Classes.Pathfinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aufgabe03_Tests
{
    [TestClass]
    public class ChildNodesTest
    {
        [TestMethod]
        public void Properties_Test()
        {
            var childNodes = new ChildNodes(
                new[]
                {
                    new QuadratNode(new Point(2, 2), 2),    // Nord Ost 
                    new QuadratNode(new Point(2, 0), 2),    // Sued Ost
                    new QuadratNode(new Point(0, 0), 2),    // Sued West
                    new QuadratNode(new Point(0, 2), 2)     // Nord West
                },
                new QuadratNode(new Point(4, 3), 3),        // Letzter Weg
                new Point(0, 0)                             // Ziel
            );

            Assert.AreEqual(new Point(1, 1), childNodes.SW_Node.MapQuadrat.Mittelpunkt);
            Assert.AreEqual(new Point(3, 1), childNodes.SO_Node.MapQuadrat.Mittelpunkt);
            Assert.AreEqual(new Point(1, 3), childNodes.NW_Node.MapQuadrat.Mittelpunkt);
            Assert.AreEqual(new Point(3, 3), childNodes.NO_Node.MapQuadrat.Mittelpunkt);
            
            // Erste Node weil einzigste die Weg beruehrt
            Assert.AreEqual(childNodes.NO_Node, childNodes.ChildNodesSortiert[0].Node);
            // Zweite Node weil am nächsten zum Ziel
            Assert.AreEqual(childNodes.SW_Node, childNodes.ChildNodesSortiert[1].Node);
            // Kuerzeste Entfernung
            Assert.AreEqual(true, childNodes.ChildNodesSortiert[1].KuerzesteTargetEntfernung);



            var childNodes02 = new ChildNodes(
                new[]
                {
                    new QuadratNode(new Point(2, 2), 2),    // Nord Ost 
                    new QuadratNode(new Point(2, 0), 2),    // Sued Ost
                    new QuadratNode(new Point(0, 0), 2),    // Sued West
                    new QuadratNode(new Point(0, 2), 2)     // Nord West
                },
                new QuadratNode(new Point(4, 3), 3),        // Letzter Weg
                new Point(3, 0)                             // Ziel
            );

            // Erste Node weil einzigste die Weg beruehrt
            Assert.AreEqual(childNodes02.NO_Node, childNodes02.ChildNodesSortiert[0].Node);
            // Zweite Node weil am nächsten zum Ziel
            Assert.AreEqual(childNodes02.SO_Node, childNodes02.ChildNodesSortiert[1].Node);
            // Kuerzeste Entfernung
            Assert.AreEqual(true, childNodes02.ChildNodesSortiert[1].KuerzesteTargetEntfernung);


            var childNodes03 = new ChildNodes(
                new[]
                {
                    new QuadratNode(new Point(2, 2), 2),    // Nord Ost 
                    new QuadratNode(new Point(2, 0), 2),    // Sued Ost
                    new QuadratNode(new Point(0, 0), 2),    // Sued West
                    new QuadratNode(new Point(0, 2), 2)     // Nord West
                },
                new QuadratNode(new Point(-4, 0), 4),       // Letzter Weg
                new Point(7, 5)                             // Ziel
            );

            // Erste Node weil Weg beruehrt und am kuerzesten zu Ziel
            Assert.AreEqual(childNodes03.NW_Node, childNodes03.ChildNodesSortiert[0].Node);
            // Zweite Node weil Weg beruehrt
            Assert.AreEqual(childNodes03.SW_Node, childNodes03.ChildNodesSortiert[1].Node);
            // Dritte Node weil am nächsten zum Ziel
            Assert.AreEqual(childNodes03.NO_Node, childNodes03.ChildNodesSortiert[2].Node);
            // NICHT Kuerzeste Entfernung
            Assert.AreEqual(false, childNodes03.ChildNodesSortiert[1].KuerzesteTargetEntfernung);
            // Kuerzeste Entfernung
            Assert.AreEqual(true, childNodes03.ChildNodesSortiert[2].KuerzesteTargetEntfernung);
        }
    }
}
