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
        // Exercise 05 - Vending Machine
        // shawn.hopkins1@gmail.com

        static void Main(string[] args)
        {           
            var vendingMachine = new VendingMachine(3,50);
            vendingMachine.CanRack.FillTheCanRack();

            // If Command-line args are provided in the debugger the app will not dispaly the UI
            CmdArgs cmd = new CmdArgs(vendingMachine.PurchasePrice.Price, args);

            // commandline args pass validation
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
            bool isVending = true;
            while (isVending)
            {
                Console.WriteLine($"\nThe price of a soda is {vendingMachine.PurchasePrice.Price} cents. Hit 'Esc' to end at any time\n");

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
                        LeaveApp(vendingMachine.Transaction);
                    }
                    // add a coin to the balanace
                    else
                    {
                        if (vendingMachine.Transaction.ParseCoin(rk))
                        {
                            // While amount of coins are less than price of soda
                            if (!vendingMachine.Transaction.IsAmountSufficient())
                            {
                                Console.WriteLine($" a {vendingMachine.Transaction.Coins.LastOrDefault()}, add {vendingMachine.Transaction.Balance} more cents");
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
                    LeaveApp(vendingMachine.Transaction);
                }

                var selection = key.KeyChar.ToString() + Console.ReadLine();
                var soda = FlavorOps.ToFlavor(selection);
                if (soda !=default)
                {
                    // If its not emtpy remove a can
                    if (!vendingMachine.CanRack.IsEmpty(soda))
                    {
                        if (vendingMachine.Transaction.IsAmountSufficient())
                        {
                            vendingMachine.CanRack.RemoveACanOf(soda);
 
                            // Make the purchase and deposit the money into the coinbox 


                            foreach (var coin in vendingMachine.Transaction.MakePurchase())
                            {
                                vendingMachine.Box.Deposit(coin);
                            }

                            var refund = vendingMachine.Transaction.GetRefund();

                            Console.Write($"Here's your can of {soda} soda");

                            if (refund.Count() > 0)
                            {
                                DisplayRefund(refund.ToList());
                            }
                            Console.WriteLine("\nHit any key to continue");

                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine($"Please deposit {vendingMachine.Transaction.Balance} more cents");
 
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
            Console.WriteLine($"\n  _Refunding {refund.Sum(x => x.ValueOf)} cents...");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach(var c in refund)
            {               
                Console.WriteLine($"\t{c.ToString()}");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void LeaveApp(Transaction purse)
        {
            DisplayRefund(purse.Coins);
            Environment.Exit(0);
        }
    }
}
