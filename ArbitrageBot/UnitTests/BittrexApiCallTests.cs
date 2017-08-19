using System;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Poloniex;
using ArbitrageBot.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitrageBot.Strategies;


namespace ArbitrageBot.UnitTests
{
    [Ignore]
    [TestClass]
    public class BittrexApiCallTests : TestBase
    {
        const string TEST_WITHDRAW_ADDRESS = "";
        const string BITTREX_TEST_MARKET = "BTC-LTC";
        const string BITTREX_TEST_CURRENCY = "LTC";
        static string BITTREX_ORDER_ID = "0";
        
        //---Bittrex API call tests---//

        //public endpoints
        [TestMethod]
        public void Test_BittrexGetMarkets() => new BittrexRequest().Public().GetMarkets();

        [TestMethod]
        public void Test_BittrexGetCurrencies() => new BittrexRequest().Public().GetCurrencies();

        [TestMethod]
        public void Test_BittrexGetTicker() => new BittrexRequest().Public().GetTicker(BITTREX_TEST_MARKET);

        [TestMethod]
        public void Test_BittrexGetMarketSummaries() => new BittrexRequest().Public().GetMarketSummaries();

        [TestMethod]
        public void Test_BittrexGetMarketSummary() => new BittrexRequest().Public().GetMarketSummary(BITTREX_TEST_MARKET);

        [TestMethod]
        public void Test_BittrexGetMarketHistory() => new BittrexRequest().Public().GetMarketHistory(BITTREX_TEST_MARKET);

        [TestMethod]
        public void Test_BittrexGetOrderBook() => new BittrexRequest().Public().GetOrderBook(BITTREX_TEST_MARKET, "both");
        //market endpoints

        [TestMethod]
        public void Test_BittrexBuyLimit() => new BittrexRequest().Market().BuyLimit(BITTREX_TEST_MARKET, 0, 0);

        [TestMethod]
        public void Test_BittrexSellLimit() => new BittrexRequest().Market().SellLimit(BITTREX_TEST_MARKET, 0, 0);

        [TestMethod]
        public void Test_BittrexCancel() => new BittrexRequest().Market().Cancel(BITTREX_ORDER_ID);

        [TestMethod]
        public void Test_BittrexGetOpenOrders() => new BittrexRequest().Market().GetOpenOrders();
        //account endpoints

        [TestMethod]
        public void Test_BittrexGetDepositHistory() => new BittrexRequest().Account().GetDepositHistory();

        [TestMethod]
        public void Test_BittrexGetWithdrawalHistory() => new BittrexRequest().Account().GetWithdrawalHistory();

        [TestMethod]
        public void Test_BittrexGetOrderHistory() => new BittrexRequest().Account().GetOrderHistory();

        [TestMethod]
        public void Test_BittrexGetOrder() => new BittrexRequest().Account().GetOrder(BITTREX_ORDER_ID);

        [TestMethod]
        public void Test_BittrexGetBalances() => new BittrexRequest().Account().GetBalances();

        [TestMethod]
        public void Test_BittrexGetBalance() => new BittrexRequest().Account().GetBalance(BITTREX_TEST_CURRENCY);

        [TestMethod]
        public void Test_BittrexGetDepositAddress() => new BittrexRequest().Account().GetDepositAddress(BITTREX_TEST_CURRENCY);
    }
}
