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
            wall01.Rows[0].Bricks[0] = false;
            wall01.Rows[0].RowSum = 1;

            var wall02 = wall01.Clone();

            wall01.Rows[0].Bricks[1] = false;
            wall01.Rows[0].RowSum = 3;

            Assert.AreEqual(true, wall02.Rows[0].Bricks[1]);
            Assert.AreEqual(1, wall02.Rows[0].RowSum);
        }
    }
}
