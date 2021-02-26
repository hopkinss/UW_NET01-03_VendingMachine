using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VendingMachine.Tests
{
    [TestClass]
    public class Transaction
    {
        protected static CoinBox trx;


        [TestMethod]
        public void CoinBoxBalanceTest()
        {
            var trx = new global::VendingMachine.Transaction(70);
            Assert.AreEqual(trx.Balance, 70);
        }

        [TestMethod]
        public void CoinBoxAddCoinTest()
        {
            var trx = new global::VendingMachine.Transaction(50);
            trx.AddCoin(new Coin(Denomination.DIME));
            trx.AddCoin(new Coin(Denomination.DIME));

            Assert.AreEqual(trx.Balance, 30);
        }
        [TestMethod]
        public void CoinBoxRemoveCoinTest()
        {
            var trx = new global::VendingMachine.Transaction(50);
            trx.AddCoin(new Coin(Denomination.QUARTER));
            trx.AddCoin(new Coin(Denomination.DIME));
            trx.RemoveCoin(new Coin(Denomination.QUARTER));

            Assert.AreEqual(trx.Balance, 40);
        }

        [TestMethod]
        public void CoinBoxIsAmountSufficientTest()
        {
            var trx = new global::VendingMachine.Transaction(55);
            trx.AddCoin(new Coin(Denomination.QUARTER));
            trx.AddCoin(new Coin(Denomination.QUARTER));

            Assert.IsFalse(trx.IsAmountSufficient());

            trx.AddCoin(new Coin(Denomination.NICKEL));
            Assert.IsTrue(trx.IsAmountSufficient());
        }
        [TestMethod]
        public void CoinBoxProcessPaymentNoRefundTest()
        {
            var trx = new global::VendingMachine.Transaction(50);
            trx.AddCoin(new Coin(Denomination.QUARTER));
            trx.AddCoin(new Coin(Denomination.DIME));
            trx.AddCoin(new Coin(Denomination.DIME));
            trx.AddCoin(new Coin(Denomination.NICKEL));
            
            var refund = trx.MakePurchase();

            Assert.AreEqual(refund.Count, 0);
            Assert.AreEqual(trx.Balance, 50);
        }

        [TestMethod]
        public void CoinBoxProcessPaymentRefundTest()
        {
            var trx = new global::VendingMachine.Transaction(35);
            trx.AddCoin(new Coin(Denomination.QUARTER));
            trx.AddCoin(new Coin(Denomination.DIME));
            trx.AddCoin(new Coin(Denomination.DIME));
            trx.AddCoin(new Coin(Denomination.NICKEL));

            var refund = trx.MakePurchase();

            Assert.AreEqual(refund.Sum(x=>x.ValueOf), 15);
            Assert.AreEqual(trx.Balance, 35);
            Assert.IsTrue(trx.Coins.Count == 0);
        }

        [TestMethod]
        public void CoinBoxParseCoinTest()
        {
            var trx = new global::VendingMachine.Transaction(35);

            Denomination[] coins = { Denomination.SLUG, Denomination.NICKEL, Denomination.DIME,
                Denomination.QUARTER, Denomination.HALFDOLLAR };

            ConsoleKeyInfo[] keys = { new ConsoleKeyInfo('S', ConsoleKey.S, false, false, false),
                                        new ConsoleKeyInfo('N', ConsoleKey.N, false, false, false),
                                        new ConsoleKeyInfo('D', ConsoleKey.D, false, false, false),
                                        new ConsoleKeyInfo('Q', ConsoleKey.Q, false, false, false),
                                        new ConsoleKeyInfo('H', ConsoleKey.H, false, false, false) };

            for(int i = 0; i < coins.Length; i++)
            {
                trx.ParseCoin(keys[i]);
                Assert.AreEqual(trx.Coins.LastOrDefault().CoinEnumeral, coins[i]);
            }
        }
    }
}