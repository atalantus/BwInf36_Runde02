using System;
using Aufgabe01_LR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aufgabe01_LR_Tests
{
    [TestClass]
    public class RowTest
    {
        [TestMethod]
        public void PlaceShortestBrick_Test()
        {
            var row01 = new Row(8);
            
            Assert.AreEqual(0, row01.RowSum);
            Assert.AreEqual(1, row01.NextLowestRowSum);

            row01.PlaceShortestBrick();

            Assert.AreEqual(false, row01.Bricks[0]);
            Assert.AreEqual(1, row01.RowSum);
            Assert.AreEqual(3, row01.NextLowestRowSum);

            row01.PlaceShortestBrick();

            Assert.AreEqual(false, row01.Bricks[1]);
            Assert.AreEqual(3, row01.RowSum);
            Assert.AreEqual(6, row01.NextLowestRowSum);
        }
    }
}
