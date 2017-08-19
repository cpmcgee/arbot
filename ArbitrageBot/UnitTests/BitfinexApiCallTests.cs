using System;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Poloniex;
using ArbitrageBot.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitrageBot.Strategies;
using System.Net;

namespace ArbitrageBot.UnitTests
{
    [Ignore]
    [TestClass]
    public class BitfinexApiCallTests : TestBase
    {
        const string TEST_WITHDRAW_ADDRESS = "";
        const string BITFINEX_TEST_SYMBOL = "btcusd";
        const string BITFINEX_TEST_CURRENCY = "ltc";
        const string BITFINEX_TEST_WALLET = Bitfinex.WalletType.EXCHANGE;
        static int BITFINEX_ORDER_ID = 0;

        //---Bitfinex API call tests---//

        [TestMethod]
        public void Test_BitfinexGetTicker() => new BitfinexRequest().GetTicker(BITFINEX_TEST_SYMBOL);

        [TestMethod]
        public void Test_BitfinexGetStats() => new BitfinexRequest().GetStats(BITFINEX_TEST_SYMBOL);

        [TestMethod]
        public void Test_BitfinexGetSymbols() => new BitfinexRequest().GetSymbols();

        [TestMethod]
        public void Test_BitfinexGetSymbolDetails() => new BitfinexRequest().GetSymbolDetails();

        [TestMethod]
        public void Test_BitfinexGetOrderBook() => new BitfinexRequest().GetOrderBook(BITFINEX_TEST_SYMBOL);

        [TestMethod]
        public void Test_BitfinexGetTrades() => new BitfinexRequest().GetTrades(BITFINEX_TEST_SYMBOL);

        [TestMethod]
        public void Test_BitfinexAccountInfo() => new BitfinexRequest().AccountInfo();

        [TestMethod]
        [ExpectedException(typeof(WebException),
        "{\"message\":\"Unknown wallet name\"}")]
        public void Test_BitfinexDeposit()
        {
            new BitfinexRequest().Deposit(BITFINEX_TEST_WALLET, BITFINEX_TEST_CURRENCY);
        }

        [TestMethod]
        public void Test_BitfinexWalletBalances() => new BitfinexRequest().WalletBalances();

        [TestMethod]
        public void Test_BitfinexTransfer() => new BitfinexRequest().Transfer("0", BITFINEX_TEST_CURRENCY, BITFINEX_TEST_WALLET, "deposit");

        [TestMethod]
        public void Test_BitfinexWithdraw() => new BitfinexRequest().Withdraw(BITFINEX_TEST_CURRENCY, BITFINEX_TEST_WALLET, "0", TEST_WITHDRAW_ADDRESS);

        [TestMethod]
        [ExpectedException(typeof(WebException),
        "{\"message\":\"Order price must be positive.\"}")]
        public void Test_BitfinexNewOrder() => new BitfinexRequest().NewOrder(BITFINEX_TEST_SYMBOL, 0, 0, "buy", "market");

        [TestMethod]
        [ExpectedException(typeof(WebException),
        "{\"message\":\"Order could not be cancelled.\"}")]
        public void Test_BitfinexCancelOrder() => new BitfinexRequest().CancelOrder(BITFINEX_ORDER_ID);

        [TestMethod]
        public void Test_BitfinexCancelOrders() => new BitfinexRequest().CancelOrders(new int[] { BITFINEX_ORDER_ID });

        [TestMethod]
        public void Test_BitfinexCancelAllOrders() => new BitfinexRequest().CancelAllOrders();

        [TestMethod]
        [ExpectedException(typeof(WebException),
        "{\"message\":\"No such order found.\"}")]
        public void Test_BitfinexOrderStatus() => new BitfinexRequest().OrderStatus(BITFINEX_ORDER_ID);

        [TestMethod]
        public void Test_BitfinexActiveOrders() => new BitfinexRequest().ActiveOrders();

        [TestMethod]
        public void Test_BitfinexPastTrades() => new BitfinexRequest().PastTrades(BITFINEX_TEST_SYMBOL, DateTime.Now);
    }
}
