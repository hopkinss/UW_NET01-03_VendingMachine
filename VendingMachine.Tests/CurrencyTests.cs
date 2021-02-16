using Currency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;


namespace VendingMachine.Tests
{
 
    [TestClass]
    public class CurrencyTests
    {
        [TestMethod]
        public void CoinCtorTest()
        {
            var coin = new Coin();
            Assert.IsInstanceOfType(coin, typeof(Coin));
            Assert.AreEqual(coin.CoinEnumeral, Denomination.SLUG);
            Assert.AreEqual(coin.ValueOf, 0M);
        }

        [TestMethod]
        public void CoinCtorEnumTest()
        {
            var coin = new Coin(Denomination.NICKEL);
            Assert.AreEqual(coin.CoinEnumeral, Denomination.NICKEL);
            Assert.AreEqual(coin.ValueOf, 5M);
            Assert.ThrowsException<ArgumentException>(Ctor(() => new Coin((Denomination)4)));
        }

        [TestMethod]
        public void CoinCtorNameTest()
        {
            var coin = new Coin("Dime");
            Assert.AreEqual(coin.CoinEnumeral, Denomination.DIME);
            Assert.AreEqual(coin.ValueOf, 10M);
            Assert.ThrowsException<ArgumentException>(Ctor(() => new Coin("PENNY")));
        }

        [TestMethod]
        public void CoinCtorValueTest()
        {
            var coin = new Coin(50M);
            Assert.AreEqual(coin.CoinEnumeral,Denomination.HALFDOLLAR);
            Assert.AreEqual(coin.ValueOf, 50M);
            Assert.ThrowsException<ArgumentException>(Ctor(() => new Coin(30M)));
        }


        static Action Ctor<T>(Func<T> func)
        {
            return () => func();
        }
    }
}
