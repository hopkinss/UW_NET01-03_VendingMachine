using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace VendingMachine
{
    class Program
    {
        // Hopkins, Shawn, hopkinss
        // Exercise 04 - Vending Machine
        // shawn.hopkins1@gmail.com

        static void Main(string[] args)
        {

 


          

            var vendingMachine = new VendingMachine(3, 75.0m);
            vendingMachine.CanRack.FillTheCanRack();

            // parse cmdline arguments 
            CmdArgs cmd = new CmdArgs(vendingMachine.PurchasePrice.Price, args);

            // If valid cmdline arguments are provided, app performs specificed action outside of UI
            if (cmd.IsArgsOk)
            {
                // automate vending without 'UI'
                if (cmd.VendAction ==CmdAction.Vend)
                {
                    // write the json response for transaction to console
                    Console.WriteLine(vendingMachine.AutoVend(cmd));
                    Environment.Exit(0);
                }
            }

            // Otherwise display the UI
            Console.WriteLine($"The price of a soda is {vendingMachine.PurchasePrice.Price} cents. Hit 'Esc' to end at any time\n");

            bool isVending = true;
            while (isVending)
            {
                // Enter coins
                Console.WriteLine("\nHit the key of that corresponds to the coin to enter into the vending machine (enter to submit)\n");
                foreach (var c in Enum.GetValues(typeof(Denomination)))
                {
                    if (c != default)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"\t{c.ToString().Substring(0,1).ToLower()}={c.GetFriendlyName()}\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                var isSubmit = false;
                do
                {
                    var rk = Console.ReadKey();
                    // submit money for soda
                    if (rk.Key == ConsoleKey.Enter)
                    {
                        isSubmit = true;
                    }
                    // leave
                    else if (rk.Key==ConsoleKey.Escape)
                    {
                        LeaveApp(vendingMachine.CoinBox);
                    }
                    // add a coin to the balanace
                    else
                    {
                        if (vendingMachine.CoinBox.ParseCoin(rk))
                        {
                            // While amount of coins are less than price of soda
                            if (!vendingMachine.CoinBox.IsAmountSufficient())
                            {
                                Console.WriteLine($" a {vendingMachine.CoinBox.Coins.LastOrDefault()}, add {vendingMachine.CoinBox.Balance} more cents");
                            }
                            else
                            {
                                Console.WriteLine("  amount sufficient, hit 'Enter' to vend");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"  {rk.Key} isnt a valid coin\n");
                        }                       
                    }

                } while (!isSubmit);

                Console.WriteLine($"--Current inventory--");
                DisplayInventory(vendingMachine.CanRack);

                // Select a flavor
                Console.Write("\nEnter a flavor: ");

                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    LeaveApp(vendingMachine.CoinBox);
                }

                var selection = key.KeyChar.ToString() + Console.ReadLine();
                if (Enum.TryParse<Flavor>(selection, true, out Flavor soda))
                {
                    if (Enum.IsDefined(typeof(Flavor), soda))
                    {
                        // If its not emtpy remove a can
                        if (!vendingMachine.CanRack.IsEmpty(soda))
                        {
                            if (vendingMachine.CoinBox.IsAmountSufficient())
                            {
                                vendingMachine.CanRack.RemoveACanOf(soda);
                                var refund = vendingMachine.CoinBox.ProcessRefund();

                                Console.Write($"Here's your can of {soda} soda");

                                if (refund.Count() > 0)
                                {
                                    Console.WriteLine($" and your refund of {refund.Sum(x => x.ValueOf)} cents");
                                    DisplayRefund(refund.ToList());
                                }
                                Console.WriteLine("\nHit any key to continue");

                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine($"Please deposit {vendingMachine.CoinBox.Balance} more cents");
 
                            }
                        }
                        // Otherwise prompt to add a can
                        else
                        {
                            Console.Write($"\n\nThe {soda} soda is empty. Do you wish to add a can (y/n)? ");

                            if (Console.ReadKey().Key == ConsoleKey.Y)
                            {
                                vendingMachine.CanRack.AddACanOf(soda);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid soda flavor\nhit any key to continue");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid soda flavor\nhit any key to continue");
                    Console.ReadKey();
                }
            }
        }

        private static void DisplayInventory(CanRack rack)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            // display contents of CanRack for each Can using can.tostring override
            foreach (var v in rack.DisplayCanRack().OrderBy(x => (int)x.Can.Flavor))
            {
                var suf = v.Amount > 1 ? "s" : string.Empty;
                Console.WriteLine($"\t{(int)v.Can.Flavor}) There is {v.Amount} can{suf} of {v.Can.Flavor} soda in the rack   ");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void DisplayRefund(List<Coin> refund)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            foreach(var c in refund)
            {               
                Console.WriteLine($"\t{c.ToString()}");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void LeaveApp(CoinBox purse)
        {
            DisplayRefund(purse.Coins);
            Environment.Exit(0);
        }

    }
}
