using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VendingMachine
{
    public class CoinBox
    {
        private List<Coin> coins;
        private int price;
        private int balance;

        public CoinBox(int price)
        {
            this.coins = new List<Coin>();
            this.price = price;
            SetBalance();
        }

        public List<Coin> Coins
        {
            get { return this.coins; }        
        }


        public int Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        public void AddCoin(Coin coin)
        {
            this.coins.Add(coin);
            SetBalance();
        }
        public void RemoveCoin(Coin coin)
        {
            var coinToRemove = this.coins.Where(x => x.CoinEnumeral == coin.CoinEnumeral).FirstOrDefault();
            if (coinToRemove != null)
            {
                this.coins.Remove(coinToRemove);
                SetBalance();
            }
        }

        public bool IsAmountSufficient()
        {
            return this.coins.Sum(x => x.ValueOf) >= this.price;
        }

        private void SetBalance()
        {
            this.balance = price - (int)this.coins.Sum(x => x.ValueOf);
        }

        public List<Coin> MakePurchase()
        {
            var refund = ProcessRefund().ToList();
            this.coins.Clear();
            SetBalance();
            return refund;
        }

        public IEnumerable<Coin> ProcessRefund()
        {
            var remainder = Math.Abs(this.balance);
            var returnedCoins = new List<Coin>();
            while (remainder > 0)
            {
                foreach (var d in Enum.GetValues(typeof(Denomination)).Cast<Denomination>().Where(x=>(int)x>0).Reverse())
                {
                    while ((int)d<=remainder)
                    {                        
                        remainder -= (int)d;
                        yield return new Coin(d);
                    }             
                }
            }
        }

        public bool ParseCoin(ConsoleKeyInfo key)
        {
            var parseDict = new Dictionary<ConsoleKey, Denomination> {
                { ConsoleKey.S,Denomination.SLUG },
                { ConsoleKey.N,Denomination.NICKEL },
                { ConsoleKey.D,Denomination.DIME },
                { ConsoleKey.Q,Denomination.QUARTER },
                { ConsoleKey.H,Denomination.HALFDOLLAR },
            };

            if (parseDict.TryGetValue(key.Key,out Denomination coin))
            {
                AddCoin(new Coin(coin));
                return true;
            }
            else
                return false;
        }
    }
}
