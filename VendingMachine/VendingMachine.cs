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
        private Transaction transaction;
        private CoinBox box;




        public VendingMachine(int inventory,dynamic price)
        {

            Type t = ((object)price).GetType();
            if (t == typeof(int)  || t == typeof(decimal))
            {
                this.canRack = new CanRack(inventory);
                this.purchasePrice = new PurchasePrice(price);
                this.transaction = new Transaction(purchasePrice.Price);
                //this.box = new CoinBox(CoinBox.GenerateRandomSeed(50).ToList());
                this.box = new CoinBox();
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

        public Transaction Transaction
        {
            get { return transaction; }
            set { transaction = value; }

        }
        public CoinBox Box
        {
            get { return box; }
            set { box = value; }
        }

        public string AutoVend(CmdArgs cmd)
        {
            StatusJson json = new StatusJson();
            if (cmd.IsArgsOk)
            {
                foreach (var c in cmd.Coins)
                    this.transaction.AddCoin(c);

                // if there is inventory
                if (!this.canRack.IsEmpty(cmd.Flavor))
                {
                    // and amount of money is sufficent
                    if (this.transaction.IsAmountSufficient())
                    {
                        this.canRack.RemoveACanOf(cmd.Flavor);
                        json.Msg = $"can of {cmd.Flavor} dispensed";
                        json.IsSuccess = true;

                        // use coin.tostring override to display enum descripition
                        //json.Refund = this.transaction.ProcessPayment().Select(x => x.ToString()).ToList();
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
