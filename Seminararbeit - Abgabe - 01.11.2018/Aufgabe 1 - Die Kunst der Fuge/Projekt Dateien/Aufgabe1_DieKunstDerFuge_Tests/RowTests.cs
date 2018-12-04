using System;
using System.Linq;
using Aufgabe1_DieKunstDerFuge;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aufgabe1_DieKunstDerFuge_Tests
{
    [TestClass]
    public class RowTests
    {
        [TestMethod]
        public void PlaceShortestBrick_Test()
        {
            var row01 = new Row(8);

            Assert.AreEqual(0, row01.RowSum);
            Assert.AreEqual(8, row01.NextPossibleRowSums.Count);
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 1));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 2));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 3));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 4));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 5));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 6));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 7));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 8));

            row01.NextBrickToPlace = 0;
            row01.PlaceNextBrick();

            Assert.AreEqual(false, row01.Bricks[0]);
            Assert.AreEqual(1, row01.RowSum);
            Assert.AreEqual(7, row01.NextPossibleRowSums.Count);
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 3));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 4));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 5));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 6));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 7));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 8));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 9));

            row01.NextBrickToPlace = 7;
            row01.PlaceNextBrick();

            Assert.AreEqual(false, row01.Bricks[7]);
            Assert.AreEqual(9, row01.RowSum);
            Assert.AreEqual(6, row01.NextPossibleRowSums.Count);
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 11));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 12));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 13));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 14));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 15));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 16));

            row01.NextBrickToPlace = 3;
            row01.PlaceNextBrick();

            Assert.AreEqual(false, row01.Bricks[3]);
            Assert.AreEqual(13, row01.RowSum);
            Assert.AreEqual(5, row01.NextPossibleRowSums.Count);
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 15));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 16));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 18));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 19));
            Assert.AreEqual(true, row01.NextPossibleRowSums.Any(nprs => nprs.PossibleRowSum == 20));
        }
    }
}