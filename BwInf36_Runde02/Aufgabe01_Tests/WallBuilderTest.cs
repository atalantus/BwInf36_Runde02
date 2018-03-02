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
                AnzahlKloetzchen = 2
            };
            Assert.AreEqual(2, wb.AnzahlKloetzchen);
            Assert.AreEqual(3, wb.MauerBreite);
            Assert.AreEqual(2, wb.MaxMauerHoehe);
            Assert.AreEqual(2, wb.AnzahlFugenStellen);

            wb.AnzahlKloetzchen = 4;
            Assert.AreEqual(4, wb.AnzahlKloetzchen);
            Assert.AreEqual(10, wb.MauerBreite);
            Assert.AreEqual(3, wb.MaxMauerHoehe);
            Assert.AreEqual(9, wb.AnzahlFugenStellen);

            wb.AnzahlKloetzchen = 10;
            Assert.AreEqual(6, wb.MaxMauerHoehe);
        }
    }
}
