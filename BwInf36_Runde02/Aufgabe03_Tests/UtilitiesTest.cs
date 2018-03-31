using System;
using System.Windows;
using Aufgabe03.Classes.Pathfinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aufgabe03_Tests
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void EntfernungBerechnen_Test()
        {
            var a = new Quadrat(new Point(0, 0), 2);
            var b = new Quadrat(new Point(4, 4), 2);
            var c = new Quadrat(new Point(5, 5), 2);
            var d = new Point(3, 3);
            var ab = Utilities.EntfernungBerechnen(a, b);
            var ac = Utilities.EntfernungBerechnen(a, c);
            var bc = Utilities.EntfernungBerechnen(b, c);
            var bd = Utilities.EntfernungBerechnen(b, d);
            var cd = Utilities.EntfernungBerechnen(c, d);
            Assert.AreEqual(true, ab < ac);
            Assert.AreEqual(true, bc < ac && bc < ab);
            Assert.AreEqual(false, cd <= bd);
        }
    }
}
