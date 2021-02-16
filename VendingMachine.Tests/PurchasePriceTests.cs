﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace VendingMachine.Tests
{
    [TestClass]
    class PurchasePriceTests
    {
        [TestMethod]
        public void PurchasePriceCtorTest()
        {
            var pp0 = new PurchasePrice();
            var ppDec = new PurchasePrice(75.0m);
            var ppInt = new PurchasePrice(75);

            Assert.AreEqual(pp0.Price, 0);
            Assert.AreEqual(ppDec.Price, 75);
            Assert.AreEqual(ppInt.Price, 75);
            Assert.AreEqual(pp0.PriceDecimal, 0m);
            Assert.AreEqual(ppDec.PriceDecimal, 75m);
            Assert.AreEqual(ppInt.PriceDecimal, 75m);
        }
    }
}