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
            var q01 = new Quadrat(new Point(0, 3), new Point(3, 0));
            var q02 = new Quadrat(new Point(2, 8), 6);
            Assert.AreEqual(3, q01.Breite);
            Assert.AreEqual(3, q01.Hoehe);
            Assert.AreEqual(new Point(8, 14), q02.RU_Eckpunkt);
        }

        [TestMethod]
        public void BeruehrtQuadrat_Test()
        {
            var q01 = new Quadrat(new Point(0, 3), new Point(3, 6));
            var q02 = new Quadrat(new Point(1, 7), new Point(7, 13));
            var q03 = new Quadrat(new Point(2, 4), 2);
            var q04 = new Quadrat(new Point(15, 35), 20);
            var q05 = new Quadrat(new Point(3, 3), 3);
            Assert.AreEqual(false, q01.BeruehrtQuadrat(q02));
            Assert.AreEqual(new Point(4, 6), q03.RU_Eckpunkt);
            Assert.AreEqual(true, q01.BeruehrtQuadrat(q05));
            Assert.AreEqual(true, q01.BeruehrtQuadrat(q03));
            Assert.AreEqual(true, q03.BeruehrtQuadrat(q01));
            Assert.AreEqual(false, q02.BeruehrtQuadrat(q04));
            Assert.AreEqual(false, q04.BeruehrtQuadrat(q03));
        }
    }
}
