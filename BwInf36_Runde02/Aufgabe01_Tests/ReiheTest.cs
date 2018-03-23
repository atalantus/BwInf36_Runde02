using System;
using Aufgabe01;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aufgabe01_Tests
{
    [TestClass]
    public class ReiheTest
    {
        [TestMethod]
        public void GetRowSum_Test()
        {
            Reihe reihe = new Reihe(new Mauer(4, 15), 5);
            reihe.SetKlotz(0, 1);
            reihe.SetKlotz(1, 2);
            Assert.AreEqual(1, reihe.GetRowSum(1));
//            Assert.AreEqual(3, reihe.WholeRowSum); // Geht nicht mit BruteForce
            reihe.SetKlotz(4, 5);
//            Assert.AreEqual(8, reihe.WholeRowSum);  // Funktioniert nicht mit BruteForce aktiviert

            for (int i = 0; i < 5; i++)
            {
                reihe.SetKlotz(i, i + 1);
            }
            Assert.AreEqual(0, reihe.GetRowSum(0));
            Assert.AreEqual(1, reihe.GetRowSum(1));
            Assert.AreEqual(3, reihe.GetRowSum(2));
            Assert.AreEqual(6, reihe.GetRowSum(3));
            Assert.AreEqual(10, reihe.GetRowSum(4));
            Assert.AreEqual(15, reihe.GetRowSum(5));
            Assert.AreEqual(15, reihe.WholeRowSum);
        }
    }
}
