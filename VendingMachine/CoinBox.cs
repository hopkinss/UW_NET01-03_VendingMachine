using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VendingMachine
{
    public class CoinBox
    {
        private List<Coin> box;

        public CoinBox()
        {
            this.box = new List<Coin>();
        }
        public CoinBox(List<Coin> SeedMoney)
        {
            this.box = new List<Coin>();
            box.AddRange(SeedMoney);
        }

        public void Deposit(Coin Acoin)
        {
            box.Add(Acoin);
        }

        public bool Withdraw(Denomination ACoinDenomination)
        {
            var coinToRemove = this.box.FirstOrDefault(x => x.CoinEnumeral == ACoinDenomination);
            if (coinToRemove != null)
            {
                this.box.Remove(coinToRemove);
                return true;
            }
            return false;
        }

        public int HalfDollarCount
        {
            get { return CoinCount(Denomination.HALFDOLLAR); }
        }
        public int QuarterCount
        {
            get { return CoinCount(Denomination.QUARTER); }
        }
        public int DimeCount
        {
            get { return CoinCount(Denomination.DIME); }
        }
        public int NickelCount
        {
            get { return CoinCount(Denomination.NICKEL); }
        }
        public int SlugCount
        {
            get { return CoinCount(Denomination.SLUG); }
        }

        public int CoinCount(Denomination denomination)
        {
           return this.box.Where(x => x.CoinEnumeral == denomination).Count();
        }

        public decimal ValueOf
        {
            get { return this.box.Sum(x => x.ValueOf); }
        }

 
        public static IEnumerable<Coin> GenerateRandomSeed(int coinCount)
        {
            var coins = Enum.GetValues(typeof(Denomination)).Cast<Denomination>().ToArray();
            var random = new Random();
            for(int i = 0; i < coinCount; i++)
            {
                yield return new Coin(coins[random.Next(0, 4)]);
            }
        }

    }
}
