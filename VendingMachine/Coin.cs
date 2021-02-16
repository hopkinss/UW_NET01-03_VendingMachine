using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;


namespace VendingMachine
{
    public enum Denomination { 
        [Description("Slug")]
        SLUG = 0,
        [Description("Nickel")]
        NICKEL = 5,
        [Description("Dime")]
        DIME = 10,
        [Description("Quarter")]
        QUARTER = 25,
        [Description("Half-dollar")]
        HALFDOLLAR = 50 
    }
    public class Coin
    {

        private Denomination denomination;

        public Coin()
        {
            this.denomination = Denomination.SLUG;
        }

        public Coin(Denomination CoinEnumeral)
        {
            if (IsValid(typeof(Denomination), CoinEnumeral))
            {
                this.denomination = CoinEnumeral;
            }
            else
                throw new ArgumentException("Invalid demonination for coin");
        }

        public Coin(string CoinName)
        {
            if (Enum.TryParse<Denomination>(CoinName,true,out Denomination coin))
            {
                this.denomination = coin;
            }
            else
                throw new ArgumentException("Invalid name for coin");
        }

        public Coin(decimal CoinValue)
        {
            if (IsValid(typeof(Denomination),((Denomination)Convert.ToInt32((decimal)CoinValue))))
            {
                this.denomination = ((Denomination)Convert.ToInt32((decimal)CoinValue));
            }
            else
                throw new ArgumentException("Invalid value for coin");
        }

        public decimal ValueOf
        {
            get 
            { 
                return Convert.ToDecimal((int)this.denomination); 
            }
        }

        public Denomination CoinEnumeral
        {
            get => this.denomination;
        }

        public override string ToString()
        {
            return Utility.GetFriendlyName<Denomination>(this.denomination); 
        }

        private bool IsValid(Type t, Enum v)
        {
            if (v != null)
            {
                if (Enum.IsDefined(t, v))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
