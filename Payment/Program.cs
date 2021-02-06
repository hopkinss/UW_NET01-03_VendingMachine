using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Currency.Coin;

namespace Currency
{
    class Program
    {
        static void Main(string[] args)
        {
            var coin = new Coin(10.0m);

            var list = new List<Coin>();
            list.Add(new Coin(Denomination.DIME));
            list.Add(new Coin(Denomination.DIME));
            list.Add(new Coin(Denomination.DIME));

            list.RemoveAll(x => x.CoinEnumeral == Denomination.DIME);
        }
      //  cans.FirstOrDefault(x => x.Flavor == FlavorOfCanToBeRemoved)


    }
}
