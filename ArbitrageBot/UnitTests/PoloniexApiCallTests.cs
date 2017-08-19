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
    public class PoloniexApiCallTests : TestBase
    {
        const string TEST_WITHDRAW_ADDRESS = "";
        const string POLONIEX_TEST_PAIR = "BTC_LTC";
        const string POLONIEX_TEST_CURRENCY = "LTC";
        static int POLONIEX_ORDER_ID = 0;

        //---Poloniex API call tests---//

        //public endpoints
        [TestMethod]
        public void Test_PoloniexReturnTicker() => new PoloniexRequest().Public().ReturnTicker();

        [TestMethod]
        public void Test_PoloniexReturnOrderBook() => new PoloniexRequest().Public().ReturnOrderBook();

        [TestMethod]
        public void Test_PoloniexReturnTradeHistory() => new PoloniexRequest().Public().ReturnTradeHistory(POLONIEX_TEST_PAIR, DateTime.Now, DateTime.Now);

        [TestMethod]
        public void Test_PoloniexReturnCurrencies() => new PoloniexRequest().Public().ReturnCurrencies();

        //trading endpoints
        [TestMethod]
        public void Test_PoloniexReturnBalances() => new PoloniexRequest().Trading().ReturnBalances();

        [TestMethod]
        public void Test_PoloniexReturnCompleteBalances() => new PoloniexRequest().Trading().ReturnCompleteBalances();

        [TestMethod]
        public void Test_PoloniexReturnDepositAddresses() => new PoloniexRequest().Trading().ReturnDepositAddresses();

        [TestMethod]
        public void Test_PoloniexReturnDepositsWithdrawals() => new PoloniexRequest().Trading().ReturnDepositsWithdrawals(DateTime.Now, DateTime.Now);

        [TestMethod]
        public void Test_PoloniexGenerateNewAddress() => new PoloniexRequest().Trading().GenerateNewAddress(POLONIEX_TEST_CURRENCY);

        [TestMethod]
        public void Test_PoloniexReturnOpenOrders() => new PoloniexRequest().Trading().ReturnOpenOrders(POLONIEX_TEST_PAIR);

        [TestMethod]
        public void Test_PoloniexReturnOrderTrades() => new PoloniexRequest().Trading().ReturnOrderTrades(POLONIEX_ORDER_ID);

        [TestMethod]
        [ExpectedException(typeof(WebException),
        "{\"error\":\"Total must be at least 0.0001\"}")]
        public void Test_PoloniexBuy() => new PoloniexRequest().Trading().Buy(POLONIEX_TEST_PAIR, 0, 0);

        [TestMethod]
        [ExpectedException(typeof(WebException),
        "{\"error\":\"Total must be at least 0.0001\"}")]
        public void Test_PoloniexSell() => new PoloniexRequest().Trading().Sell(POLONIEX_TEST_PAIR, 0, 0);

        [TestMethod]
        public void Test_PoloniexCancelOrder() => new PoloniexRequest().Trading().CancelOrder(POLONIEX_ORDER_ID);

        [TestMethod]
        public void Test_PoloniexMoveOrder() => new PoloniexRequest().Trading().MoveOrder(POLONIEX_ORDER_ID, 0);

        [TestMethod]
        [ExpectedException(typeof(WebException),
        "{\"error\":\"Invalid address parameter.\"}")]
        public void Test_PoloniexWithdraw() => new PoloniexRequest().Trading().Withdraw(POLONIEX_TEST_CURRENCY, 0, TEST_WITHDRAW_ADDRESS);

        [TestMethod]
        public void Test_PoloniexReturnAvailableBalances() => new PoloniexRequest().Trading().ReturnAvailableAccountBalances();

        [TestMethod]
        public void Test_PoloniexReturnTradeableBalances() => new PoloniexRequest().Trading().ReturnTradeableBalances();
    }
}
