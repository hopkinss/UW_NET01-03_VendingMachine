using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VendingMachine.Tests
{
    [TestClass]
    public class CoinBoxTests
    {
        protected static CoinBox cb;


        [TestMethod]
        public void CoinBoxBalanceTest()
        {
            var cb = new CoinBox(70);
            Assert.AreEqual(cb.Balance, 70);
        }

        [TestMethod]
        public void CoinBoxAddCoinTest()
        {
            var cb = new CoinBox(50);
            cb.AddCoin(new Coin(Denomination.DIME));
            cb.AddCoin(new Coin(Denomination.DIME));

            Assert.AreEqual(cb.Balance, 30);
        }
        [TestMethod]
        public void CoinBoxRemoveCoinTest()
        {
            var cb = new CoinBox(50);
            cb.AddCoin(new Coin(Denomination.QUARTER));
            cb.AddCoin(new Coin(Denomination.DIME));
            cb.RemoveCoin(new Coin(Denomination.QUARTER));

            Assert.AreEqual(cb.Balance, 40);
        }

        [TestMethod]
        public void CoinBoxIsAmountSufficientTest()
        {
            var cb = new CoinBox(55);
            cb.AddCoin(new Coin(Denomination.QUARTER));
            cb.AddCoin(new Coin(Denomination.QUARTER));

            Assert.IsFalse(cb.IsAmountSufficient());

            cb.AddCoin(new Coin(Denomination.NICKEL));
            Assert.IsTrue(cb.IsAmountSufficient());
        }
        [TestMethod]
        public void CoinBoxProcessPaymentNoRefundTest()
        {
            var cb = new CoinBox(50);
            cb.AddCoin(new Coin(Denomination.QUARTER));
            cb.AddCoin(new Coin(Denomination.DIME));
            cb.AddCoin(new Coin(Denomination.DIME));
            cb.AddCoin(new Coin(Denomination.NICKEL));
            
            var refund = cb.MakePurchase();

            Assert.AreEqual(refund.Count, 0);
            Assert.AreEqual(cb.Balance, 50);
        }

        [TestMethod]
        public void CoinBoxProcessPaymentRefundTest()
        {
            var cb = new CoinBox(35);
            cb.AddCoin(new Coin(Denomination.QUARTER));
            cb.AddCoin(new Coin(Denomination.DIME));
            cb.AddCoin(new Coin(Denomination.DIME));
            cb.AddCoin(new Coin(Denomination.NICKEL));

            var refund = cb.MakePurchase();

            Assert.AreEqual(refund.Sum(x=>x.ValueOf), 15);
            Assert.AreEqual(cb.Balance, 35);
            Assert.IsTrue(cb.Coins.Count == 0);
        }

        [TestMethod]
        public void CoinBoxParseCoinTest()
        {
            var cb = new CoinBox(35);

            Denomination[] coins = { Denomination.SLUG, Denomination.NICKEL, Denomination.DIME,
                Denomination.QUARTER, Denomination.HALFDOLLAR };

            ConsoleKeyInfo[] keys = { new ConsoleKeyInfo('S', ConsoleKey.S, false, false, false),
                                        new ConsoleKeyInfo('N', ConsoleKey.N, false, false, false),
                                        new ConsoleKeyInfo('D', ConsoleKey.D, false, false, false),
                                        new ConsoleKeyInfo('Q', ConsoleKey.Q, false, false, false),
                                        new ConsoleKeyInfo('H', ConsoleKey.H, false, false, false) };

            for(int i = 0; i < coins.Length; i++)
            {
                cb.ParseCoin(keys[i]);
                Assert.AreEqual(cb.Coins.LastOrDefault().CoinEnumeral, coins[i]);
            }
        }
    }
}