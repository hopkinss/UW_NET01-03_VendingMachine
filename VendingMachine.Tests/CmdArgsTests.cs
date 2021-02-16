using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VendingMachine.Tests
{
    [TestClass]
    public class CmdArgsTests
    {

        [TestMethod]
        public void CmdArgsVendTest()
        {
            var vm = new VendingMachine(3, 75);
            vm.CanRack.FillTheCanRack();
            
            string[] args = { "Vend", "Quarter", "Nickel", "Dime", "Halfdollar", "Lemon" };
            var cmd = new CmdArgs(75, args);

            Assert.IsTrue(cmd.IsArgsOk);

            var json = (StatusJson)JsonConvert.DeserializeObject(vm.AutoVend(cmd), typeof(StatusJson));

            Assert.IsTrue(json.IsSuccess);
            int refundTotal = 0;
            foreach(var v in json.Refund)
            {
                refundTotal += (int)new Coin(v).ValueOf;
            }

            Assert.AreEqual(refundTotal, 15);
        }
        [TestMethod]
        public void CmdArgsInsufficientFundsTest()
        {
            var vm = new VendingMachine(3, 75);
            vm.CanRack.FillTheCanRack();

            string[] args = { "Vend", "Quarter", "Nickel", "Lemon" };
            var cmd = new CmdArgs(75, args);

            Assert.IsTrue(cmd.IsArgsOk);

            var json = (StatusJson)JsonConvert.DeserializeObject(vm.AutoVend(cmd), typeof(StatusJson));

            Assert.IsFalse(json.IsSuccess);
            Assert.AreEqual(json.Msg, "insufficent funds");
        }

        [TestMethod]
        public void CmdArgsInsufficientInventoryTest()
        {
            var vm = new VendingMachine(3, 30);

            string[] args = { "Vend", "Quarter", "Nickel", "Lemon" };
            var cmd = new CmdArgs(75, args);

            Assert.IsTrue(cmd.IsArgsOk);

            var json = (StatusJson)JsonConvert.DeserializeObject(vm.AutoVend(cmd), typeof(StatusJson));

            Assert.IsFalse(json.IsSuccess);
            Assert.AreEqual(json.Msg, "Lemon is not in stock");
        }

        [TestMethod]
        public void CmdArgsInvalidArgsTest()
        {
            var vm = new VendingMachine(3, 30);

            string[] args = { "Vend", "Quarter", "Nickel", "Chive" };
            var cmd = new CmdArgs(75, args);

            Assert.IsFalse(cmd.IsArgsOk);
        }
    }
}