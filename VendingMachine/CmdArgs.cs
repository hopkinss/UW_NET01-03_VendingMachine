using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine
{
    public enum CmdAction { Vend=1} // extensible for other cmdline actions. Vend sells a single can of soda per assignment instructions
    public class CmdArgs
    {
        private int price;
        private bool isArgsOk;
        private CmdAction vendAction;
        private List<Coin> coins;
        private Flavor flavor;

        public CmdArgs(int initialPrice, string[] args)
        {
            this.price = initialPrice;
            this.Coins = new List<Coin>();
            this.IsArgsOk = true;
            ParseVendAction(args.ToList().FirstOrDefault());
            ParseCoins(args);
            ParseFlavor(args.ToList().LastOrDefault());
        }

        public bool IsArgsOk { get => isArgsOk; set => isArgsOk = value; }
        public CmdAction VendAction { get => vendAction; set => vendAction = value; }
        public List<Coin> Coins { get => coins; set => coins = value; }
        public Flavor Flavor { get => flavor; set => flavor = value; }

        private void ParseVendAction(string arg)
        {
            if (Enum.TryParse(typeof(CmdAction), arg, true, out object result))
            {
                this.VendAction = (CmdAction)result;
            }
            else
            {
                this.IsArgsOk = false;
            }
        }
        private void ParseCoins(string[] args)
        {
            var rawCoins = args.ToList().Skip(1).Take(args.Length - 2);
            foreach (var c in rawCoins)
            {
                if (Enum.TryParse(typeof(Denomination), c, true, out Object result))
                {
                    if (Enum.IsDefined(typeof(Denomination), (Denomination)result))
                    {
                        this.coins.Add(new Coin((Denomination)result));
                    }
                }
                else
                {
                    this.IsArgsOk = false;
                }
            }
        }

        private void ParseFlavor(string arg)
        {
            var result = FlavorOps.ToFlavor(arg);
            if (result!=default)
            {
                this.Flavor = (Flavor)result;
            }
            else
            {
                this.IsArgsOk = false;
            }
        }
    }
}