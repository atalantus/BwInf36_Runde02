using System;
using Aufgabe01;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aufgabe01_Tests
{
    [TestClass]
    public class WallBuilderTest
    {
        [TestMethod]
        public void CalculateWallProperties_Test()
        {
            WallBuilder wb = new WallBuilder();
            wb.SetUpWallBuilder(2, false);
            Assert.AreEqual(2, wb.AnzahlKloetze);
            Assert.AreEqual(3, wb.MauerBreite);
            Assert.AreEqual(2, wb.MaxMauerHoehe);
            Assert.AreEqual(2, wb.AnzahlFugenStellen);
            Assert.AreEqual(2, wb.Mauer.Length);
            Assert.AreEqual(2, wb.Mauer[0].Kloetze.Length);
            Assert.AreEqual(0, wb.Mauer[0].Kloetze[1]);

            wb.SetUpWallBuilder(4, false);
            Assert.AreEqual(4, wb.AnzahlKloetze);
            Assert.AreEqual(10, wb.MauerBreite);
            Assert.AreEqual(3, wb.MaxMauerHoehe);
            Assert.AreEqual(9, wb.AnzahlFugenStellen);
            Assert.AreEqual(3, wb.Mauer.Length);
            Assert.AreEqual(4, wb.Mauer[0].Kloetze.Length);
            Assert.AreEqual(0, wb.Mauer[2].Kloetze[3]);

            wb.SetUpWallBuilder(10, false);
            Assert.AreEqual(10, wb.AnzahlKloetze);
            Assert.AreEqual(55, wb.MauerBreite);
            Assert.AreEqual(6, wb.MaxMauerHoehe);
            Assert.AreEqual(6, wb.Mauer.Length);
            Assert.AreEqual(10, wb.Mauer[0].Kloetze.Length);
            Assert.AreEqual(0, wb.Mauer[4].Kloetze[7]);
        }
    }
}
