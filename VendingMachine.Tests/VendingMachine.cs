using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace VendingMachine.Tests
{
    [TestClass]
    public class VendingMachine
    {
        [TestMethod]
        public void PurchasePriceCtorTest()
        {
            var pp0 = new PurchasePrice();
            var ppDec = new PurchasePrice(75.0m);
            var ppInt = new PurchasePrice(75);

            Assert.AreEqual(pp0.Price, 0);
            Assert.AreEqual(ppDec.Price, 75);
            Assert.AreEqual(ppInt.Price, 75);
            Assert.AreEqual(pp0.PriceDecimal, 0m);
            Assert.AreEqual(ppDec.PriceDecimal, 75m);
            Assert.AreEqual(ppInt.PriceDecimal, 75m);
        }
        [TestMethod]
        public void CanCtorTest()
        {
            foreach (Flavor f in Enum.GetValues(typeof(Flavor)))
            {
                var c = new Can(f);
                Assert.AreEqual(c.Flavor, f);
            }
        }

        [TestMethod]
        public void CanRackCtorTest()
        {
            var cr = new CanRack(5);
            Assert.AreEqual(cr.MaxInventory, 5);
        }
        [TestMethod]
        public void CanRackFillCanRackTest()
        {
            var cr = new CanRack(3);
            cr.FillTheCanRack();
            foreach (Flavor f in Enum.GetValues(typeof(Flavor)))
            {
                // Check the filled canRack by flavor
                Assert.AreEqual(cr.Contents(f).Amount, 3);
            }
        }
        [TestMethod]
        public void CanRackEmptyRackOfTest()
        {
            var cr = new CanRack(3);
            cr.FillTheCanRack();

            // Empty each flavor and confirm amount is zero
            foreach (Flavor f in Enum.GetValues(typeof(Flavor)))
            {
                cr.EmptyCanRackOf(f);
                Assert.AreEqual(cr.Contents(f).Amount, 0);
            }
        }

        [TestMethod]
        public void CanRackAddACanOfTest()
        {
            int maxInventory = 6;
            var cr = new CanRack(maxInventory);

            // Attempt to add 3 more cans than the maximum allowed
            foreach (Flavor f in Enum.GetValues(typeof(Flavor)))
            {
                for (int i = 1; i <= maxInventory + 3; i++)
                {
                    cr.AddACanOf(f);
                    // Check the cans are incrementing up to the max inventory, but not over
                    if (i <= maxInventory)
                        Assert.AreEqual(cr.Contents(f).Amount, i);
                    else
                        Assert.AreNotEqual(cr.Contents(f).Amount, i);
                }
            }
        }

        [TestMethod]
        public void CanRackRemoveACanOfTest()
        {
            int maxInventory = 3;
            var cr = new CanRack(maxInventory);
            cr.FillTheCanRack();
            foreach (Flavor f in Enum.GetValues(typeof(Flavor)))
            {
                for (int i = maxInventory; i >= -1; i--)
                {
                    // Check inventory is decremented by 1 for each iteration but never less than 0
                    cr.RemoveACanOf(f);
                    Assert.AreEqual(cr.Contents(f).Amount, Math.Max(0, i - 1));
                }
            }
        }
        [TestMethod]
        public void CanRackIsFullTest()
        {
            int maxInventory = 5;
            var cr = new CanRack(maxInventory);

            foreach (Flavor f in Enum.GetValues(typeof(Flavor)))
            {
                for (int i = 1; i <= maxInventory + 2; i++)
                {
                    cr.AddACanOf(f);
                    // While amount < maxInventory rack is not full
                    if (i < maxInventory)
                        Assert.IsFalse(cr.IsFull(f));
                    else
                        Assert.IsTrue(cr.IsFull(f));
                }
            }
        }

        [TestMethod]
        public void CanRackIsEmptyTest()
        {

            int maxInventory = 5;
            var cr = new CanRack(maxInventory);
            cr.FillTheCanRack();

            foreach (Flavor f in Enum.GetValues(typeof(Flavor)))
            {
                for (int i = maxInventory; i >= -1; i--)
                {
                    // While amount is at least 1 soda, rack is not empty                    
                    if (i >= 1)
                        Assert.IsFalse(cr.IsEmpty(f));
                    else
                        Assert.IsTrue(cr.IsEmpty(f));

                    cr.RemoveACanOf(f);
                }
            }
        }
    }
}
