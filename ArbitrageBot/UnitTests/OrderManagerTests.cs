using System;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Poloniex;
using ArbitrageBot.APIs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace ArbitrageBot.UnitTests
{
    /// <summary>
    /// testing the order manager component directly
    /// </summary>
    [TestClass]
    public class OrderManagerTests : TestBase
    {
        const string TEST_CURRENCY = "LTC";

        const string BITFINEX_TEST_ID = "000bfx";
        const string BITTREX_TEST_ID = "000btx";
        const string POLONIEX_TEST_ID = "000plx";


        [TestMethod]
        public void Test_BittrexAddRetrieveOrder()
        {
            BittrexOrder btxOrder = new BittrexOrder(BITTREX_TEST_ID, TEST_CURRENCY, OrderType.BUY, 1);
            OrderManager.AddOrder(btxOrder);
            Assert.IsTrue(OrderManager.HasOrder(btxOrder.Id));
            Assert.IsTrue(OrderManager.GetOrders().Count == 1);
            Assert.IsTrue(OrderManager.BittrexOrders.Count == 1);
            Assert.AreEqual(OrderManager.GetOrder(btxOrder.Id), btxOrder);
        }

        [TestMethod]
        public void Test_BitfinexAddRetrieveOrder()
        {
            BitfinexOrder bfxOrder = new BitfinexOrder(BITFINEX_TEST_ID, TEST_CURRENCY, OrderType.BUY, 1);
            OrderManager.AddOrder(bfxOrder);
            Assert.IsTrue(OrderManager.HasOrder(bfxOrder.Id));
            Assert.IsTrue(OrderManager.GetOrders().Count == 1);
            Assert.IsTrue(OrderManager.BitfinexOrders.Count == 1);
            Assert.AreEqual(OrderManager.GetOrder(bfxOrder.Id), bfxOrder);
        }

        [TestMethod]
        public void Test_PoloniexAddRetrieveOrder()
        {
            PoloniexOrder plxOrder = new PoloniexOrder(POLONIEX_TEST_ID, TEST_CURRENCY, OrderType.BUY, 1);
            OrderManager.AddOrder(plxOrder);
            Assert.IsTrue(OrderManager.HasOrder(plxOrder.Id));
            Assert.IsTrue(OrderManager.GetOrders().Count == 1);
            Assert.IsTrue(OrderManager.PoloniexOrders.Count == 1);
            Assert.AreEqual(OrderManager.GetOrder(plxOrder.Id), plxOrder);
        }

        [TestMethod]
        public void Test_UpdateRemovesUnopenOrders()
        {
            BittrexOrder btxOrder = new BittrexOrder(BITTREX_TEST_ID, TEST_CURRENCY, OrderType.BUY, 1);
            BitfinexOrder bfxOrder = new BitfinexOrder(BITFINEX_TEST_ID, TEST_CURRENCY, OrderType.BUY, 1);
            PoloniexOrder plxOrder = new PoloniexOrder(POLONIEX_TEST_ID, TEST_CURRENCY, OrderType.BUY, 1);

            OrderManager.AddOrder(btxOrder);
            OrderManager.AddOrder(bfxOrder);
            OrderManager.AddOrder(plxOrder);


            Assert.IsTrue(OrderManager.GetOrder(btxOrder.Id).IsOpen);
            Assert.IsTrue(OrderManager.GetOrder(bfxOrder.Id).IsOpen);
            Assert.IsTrue(OrderManager.GetOrder(plxOrder.Id).IsOpen);
            ///Updates will close the orders just added
            ///because they are not actually on the exchange
            OrderManager.CheckOrders();

            Assert.IsFalse(OrderManager.GetOrder(btxOrder.Id).IsOpen);
            Assert.IsFalse(OrderManager.GetOrder(bfxOrder.Id).IsOpen);
            Assert.IsFalse(OrderManager.GetOrder(plxOrder.Id).IsOpen);
        }
    }
}
