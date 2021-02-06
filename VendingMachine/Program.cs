using System;
using System.Collections.Generic;
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
            var vendingMachine = new VendingMachine(2, 75.0m);

            // display the PurchasePrice.Price property
            Console.WriteLine($"The price of a soda is {vendingMachine.PurchasePrice.Price} cents\n");

            // Calls the CanRack.FillTheCanRack method to set the inventory of each flavor to maxinventory
            vendingMachine.CanRack.FillTheCanRack();

            bool isVending = true;
            while (isVending)
            {
                Console.WriteLine($"Enter a flavor from the menu:\n\n{string.Join('\n',vendingMachine.AvailableFlavors())} \n\n--Current inventory--");
                Console.ForegroundColor = ConsoleColor.Blue;

                // display contents of CanRack for each Can using can.tostring override
                foreach (Content v in vendingMachine.CanRack.Contents())
                {
                    var suf = v.Amount > 1 ? "s" : string.Empty;
                    Console.WriteLine($"{v.Amount} can{suf} of {v.Flavor} soda");
                }
                Console.ForegroundColor = ConsoleColor.White;

                // Parse user response into enum
                Console.WriteLine("\n");
                if (Enum.TryParse<Flavor>(Console.ReadLine(), true, out Flavor soda))
                {
                    // If its not emtpy remove a can
                    if (!vendingMachine.CanRack.IsEmpty(soda))
                    {
                        vendingMachine.CanRack.RemoveACanOf(soda);
                        Console.WriteLine($"Here's your can of {soda} soda");
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
                    Console.WriteLine("Invalid soda flavor\n");
                }

                // leave or repeat
                Console.WriteLine("\n\nHit 'q' to quit, or any other key to continue ");
                var resp = Console.ReadKey();
                if (resp.Key == ConsoleKey.Q)
                {
                    Environment.Exit(0);
                }
                Console.Clear();
            }
        }
    }
}
