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
            wb.SetUpWallBuilder(2);
            Assert.AreEqual(2, wb.AnzahlKloetze);
            Assert.AreEqual(3, wb.MauerBreite);
            Assert.AreEqual(2, wb.MaxMauerHoehe);
            Assert.AreEqual(2, wb.AnzahlFugenStellen);

            wb.SetUpWallBuilder(4);
            Assert.AreEqual(4, wb.AnzahlKloetze);
            Assert.AreEqual(10, wb.MauerBreite);
            Assert.AreEqual(3, wb.MaxMauerHoehe);
            Assert.AreEqual(9, wb.AnzahlFugenStellen);

            wb.SetUpWallBuilder(10);
            Assert.AreEqual(10, wb.AnzahlKloetze);
            Assert.AreEqual(55, wb.MauerBreite);
            Assert.AreEqual(6, wb.MaxMauerHoehe);
        }
    }
}
