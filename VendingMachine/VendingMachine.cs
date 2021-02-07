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

        public VendingMachine(int inventory,dynamic price)
        {

            Type t = ((object)price).GetType();
            if (t == typeof(int)  || t == typeof(decimal))
            {
                this.canRack = new CanRack(inventory);
                this.purchasePrice = new PurchasePrice(price);
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
    }
}
