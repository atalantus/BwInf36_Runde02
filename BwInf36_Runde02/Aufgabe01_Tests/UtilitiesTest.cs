using System;
using Aufgabe01;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aufgabe01_Tests
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void FakultaetBerechnen_Test()
        {
            Assert.AreEqual(1, Utilities.FakultaetBerechnen(0));
            Assert.AreEqual(1, Utilities.FakultaetBerechnen(1));
            Assert.AreEqual(2, Utilities.FakultaetBerechnen(2));
            Assert.AreEqual(6, Utilities.FakultaetBerechnen(3));
            Assert.AreEqual(24, Utilities.FakultaetBerechnen(4));
            Assert.AreEqual(3628800, Utilities.FakultaetBerechnen(10));
        }
    }
}
