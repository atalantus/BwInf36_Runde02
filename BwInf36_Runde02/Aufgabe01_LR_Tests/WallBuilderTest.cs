﻿using System;
using Aufgabe01_LR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aufgabe01_LR_Tests
{
    [TestClass]
    public class WallBuilderTest
    {
        [TestMethod]
        public void CalculateWallProperties_Test()
        {
            var wb = new WallBuilder();

            wb.BuildWall(2);
            Assert.AreEqual(2, wb.BricksPerRow);
            Assert.AreEqual(3, wb.WallLength);
            Assert.AreEqual(2, wb.WallHeight);
            Assert.AreEqual(2, wb.GapCount);
            Assert.AreEqual(2, wb.UsedGapCount);
            Assert.AreEqual(0, wb.FreeGaps);

            wb.BuildWall(3);
            Assert.AreEqual(3, wb.BricksPerRow);
            Assert.AreEqual(6, wb.WallLength);
            Assert.AreEqual(2, wb.WallHeight);
            Assert.AreEqual(5, wb.GapCount);
            Assert.AreEqual(4, wb.UsedGapCount);
            Assert.AreEqual(1, wb.FreeGaps);

            wb.BuildWall(4);
            Assert.AreEqual(4, wb.BricksPerRow);
            Assert.AreEqual(10, wb.WallLength);
            Assert.AreEqual(3, wb.WallHeight);
            Assert.AreEqual(9, wb.GapCount);
            Assert.AreEqual(9, wb.UsedGapCount);

            wb.BuildWall(10);
            Assert.AreEqual(10, wb.BricksPerRow);
            Assert.AreEqual(55, wb.WallLength);
            Assert.AreEqual(6, wb.WallHeight);
        }
    }
}
