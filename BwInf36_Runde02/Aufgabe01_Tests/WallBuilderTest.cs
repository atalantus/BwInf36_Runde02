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
            WallBuilder wb = new WallBuilder
            {
                AnzahlKloetze = 2
            };
            Assert.AreEqual(2, wb.AnzahlKloetze);
            Assert.AreEqual(3, wb.MauerBreite);
            Assert.AreEqual(2, wb.MaxMauerHoehe);
            Assert.AreEqual(2, wb.AnzahlFugenStellen);
            Assert.AreEqual(2, wb.Mauer.GetLength(0));
            Assert.AreEqual(2, wb.Mauer.GetLength(1));
            Assert.AreEqual(-1, wb.Mauer[0, 1]);

            wb.AnzahlKloetze = 4;
            Assert.AreEqual(4, wb.AnzahlKloetze);
            Assert.AreEqual(10, wb.MauerBreite);
            Assert.AreEqual(3, wb.MaxMauerHoehe);
            Assert.AreEqual(9, wb.AnzahlFugenStellen);
            Assert.AreEqual(3, wb.Mauer.GetLength(0));
            Assert.AreEqual(4, wb.Mauer.GetLength(1));
            Assert.AreEqual(-1, wb.Mauer[2, 3]);

            wb.AnzahlKloetze = 10;
            Assert.AreEqual(6, wb.MaxMauerHoehe);
            Assert.AreEqual(6, wb.Mauer.GetLength(0));
            Assert.AreEqual(10, wb.Mauer.GetLength(1));
            Assert.AreEqual(-1, wb.Mauer[4, 7]);
        }
    }
}
