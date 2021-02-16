using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;

namespace VendingMachine
{
    
    public class VendingMachine
    {

        private PurchasePrice purchasePrice;
        private CanRack canRack;
        private CoinBox coinBox;

        public VendingMachine(int inventory,dynamic price)
        {

            Type t = ((object)price).GetType();
            if (t == typeof(int)  || t == typeof(decimal))
            {
                this.canRack = new CanRack(inventory);
                this.purchasePrice = new PurchasePrice(price);
                this.coinBox = new CoinBox(purchasePrice.Price);
            }
            else
                throw new ArgumentException($"Value must be an integer or decimal type");
        }

        public CanRack CanRack
        {           
            get { return canRack; }
            set { canRack = value; }
        }

        public PurchasePrice PurchasePrice
        {
            get { return purchasePrice; }
            set { purchasePrice = value; }
        }

        public CoinBox CoinBox
        {
            get { return coinBox; }
            set { coinBox = value; }

        }

        public string AutoVend(CmdArgs cmd)
        {
            StatusJson json = new StatusJson();
            if (cmd.IsArgsOk)
            {
                foreach (var c in cmd.Coins)
                    this.coinBox.AddCoin(c);

                // if there is inventory
                if (!this.canRack.IsEmpty(cmd.Flavor))
                {
                    // and amount of money is sufficent
                    if (this.coinBox.IsAmountSufficient())
                    {
                        this.canRack.RemoveACanOf(cmd.Flavor);
                        json.Msg = $"can of {cmd.Flavor} dispensed";
                        json.IsSuccess = true;

                        // use coin.tostring override to display enum descripition
                        json.Refund = this.CoinBox.ProcessRefund().Select(x => x.ToString()).ToList();
                    }
                    else
                    {
                        json.IsSuccess = false;
                        json.Msg = "insufficent funds";
                    }
                }
                else
                {
                    json.IsSuccess = false;
                    json.Msg = $"{cmd.Flavor} is not in stock";
                }
            }
            else
            {
                json.IsSuccess = false;
                json.Msg = "arguments not in expected format";
            }

            return json.WriteJson();
        }
    }
}
