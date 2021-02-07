using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace VendingMachine
{
    class Program
    {
        // Hopkins, Shawn, hopkinss
        // Exercise 03 - Vending Machine
        // shawn.hopkins1@gmail.com

        static void Main(string[] args)
        {
            // 1) Creates and instance of can rack and set maxInventory 
            // 2)  Creates an instance of PurchasePrice and sets price
            var vendingMachine = new VendingMachine(1, 75.0m);

            // display the PurchasePrice.Price property
            Console.WriteLine($"The price of a soda is {vendingMachine.PurchasePrice.Price} cents\n");

            // Calls the CanRack.FillTheCanRack method to set the inventory of each flavor to maxinventory
            vendingMachine.CanRack.FillTheCanRack();
            vendingMachine.CanRack.DebugWriteCanRackContents(); // Write canrack inventory to debug window

            bool isVending = true;
            while (isVending)
            {
                Console.WriteLine($"--Current inventory--");
                Console.ForegroundColor = ConsoleColor.Blue;

                // display contents of CanRack for each Can using can.tostring override
                foreach (Content v in vendingMachine.CanRack.Contents().OrderBy(x => (int)x.Flavor))
                {
                    var suf = v.Amount > 1 ? "s" : string.Empty;
                    Console.WriteLine($"{(int)v.Flavor}) There is {v.Amount} can{suf} of {v.Flavor} soda in the rack   ");

                }
                Console.ForegroundColor = ConsoleColor.White;

                // Parse user response into enum
                Console.Write("\nEnter a flavor (hit q to quit): ");

                var exitKey = Console.ReadKey();
                if (exitKey.Key == ConsoleKey.Q)
                {
                    Environment.Exit(0);
                }

                var selection = exitKey.KeyChar.ToString() + Console.ReadLine();
                if (Enum.TryParse<Flavor>(selection, true, out Flavor soda))
                {
                    if (Enum.IsDefined(typeof(Flavor), soda))
                    {
                        // If its not emtpy remove a can
                        if (!vendingMachine.CanRack.IsEmpty(soda))
                        {
                            vendingMachine.CanRack.RemoveACanOf(soda);
                            Console.WriteLine($"Here's your can of {soda} soda\nhit any key to continue");
                            Console.ReadKey();
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
                Console.Clear();
            }
        }
    }
}
