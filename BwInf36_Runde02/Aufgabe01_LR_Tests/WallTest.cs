using System;
using Aufgabe01_LR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aufgabe01_LR_Tests
{
    [TestClass]
    public class WallTest
    {
        [TestMethod]
        public void Clone_Test()
        {
            var wall01 = new Wall(2, 3); 
            wall01.Rows[0].PlaceNextBrick();

            var wall02 = wall01.Clone();

            wall01.Rows[0].NextBrickToPlace = 2;
            wall01.Rows[0].PlaceNextBrick();

            Assert.AreEqual(1, wall01.Rows[0].NextPossibleRowSums.Count);
            Assert.AreEqual(2, wall02.Rows[0].NextPossibleRowSums.Count);
            Assert.AreEqual(true, wall02.Rows[0].Bricks[2]);
            Assert.AreEqual(1, wall02.Rows[0].RowSum);
        }
    }
}
