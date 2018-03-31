using System;
using System.Numerics;
using System.Windows;
using Aufgabe03.Classes.Pathfinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aufgabe03_Tests
{
    [TestClass]
    public class QuadratTest
    {
        [TestMethod]
        public void Properites_Test()
        {
            var q01 = new Quadrat(new Point(0, 0), new Point(3, 3));
            var q02 = new Quadrat(new Point(2, 2), 6);
            Assert.AreEqual(3, q01.Breite);
            Assert.AreEqual(3, q01.Hoehe);
            Assert.AreEqual(q02.RO_Eckpunkt, new Point(8, 8));
        }

        [TestMethod]
        public void BeruehrtQuadrat_Test()
        {
            var q01 = new Quadrat(new Point(0, 0), new Point(3, 3));
            var q02 = new Quadrat(new Point(1, 1), new Point(7, 7));
            var q03 = new Quadrat(new Point(2, 2), 2);
            var q04 = new Quadrat(new Point(15, 15), 20);
            var q05 = new Quadrat(new Point(3, 0), 3);
            Assert.AreEqual(true, q01.BeruehrtQuadrat(q02));
            Assert.AreEqual(new Point(4, 4), q03.RO_Eckpunkt);
            Assert.AreEqual(true, q01.BeruehrtQuadrat(q05));
            Assert.AreEqual(true, q01.BeruehrtQuadrat(q03));
            Assert.AreEqual(true, q03.BeruehrtQuadrat(q01));
            Assert.AreEqual(false, q02.BeruehrtQuadrat(q04));
            Assert.AreEqual(false, q04.BeruehrtQuadrat(q03));
        }
    }
}
