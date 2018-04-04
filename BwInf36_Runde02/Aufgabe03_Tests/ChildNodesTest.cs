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
            var node = new Node(new Point(2, 6), 6);
            node.ChildNodes.SortierChildNodes(
                new Node(new Point(0, 11), 2),                      // Letzter Weg
                new Point(11, 11)                                   // Ziel
            );

            Assert.AreEqual(new Point(8, 12), node.MapQuadrat.RU_Eckpunkt);

            Assert.AreEqual(new Point(8, 9), node.ChildNodes.NO_Node.MapQuadrat.RU_Eckpunkt);
            Assert.AreEqual(new Point(5, 9), node.ChildNodes.SO_Node.MapQuadrat.LO_Eckpunkt);
            Assert.AreEqual(new Point(8, 12), node.ChildNodes.SO_Node.MapQuadrat.RU_Eckpunkt);
            Assert.AreEqual(new Point(5, 12), node.ChildNodes.SW_Node.MapQuadrat.RU_Eckpunkt);
            Assert.AreEqual(new Point(5, 9), node.ChildNodes.NW_Node.MapQuadrat.RU_Eckpunkt);

            // Erste Node weil einzigste die Weg beruehrt
            Assert.AreEqual(node.ChildNodes.SW_Node, node.ChildNodes.ChildNodesSortiert[0].Node);
            // Zweite Node weil am nächsten zum Ziel
            Assert.AreEqual(node.ChildNodes.SO_Node, node.ChildNodes.ChildNodesSortiert[1].Node);
            // Kuerzeste Entfernung
            Assert.AreEqual(true, node.ChildNodes.ChildNodesSortiert[1].KuerzesteTargetEntfernung);
        }
    }
}
