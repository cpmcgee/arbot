using System;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Poloniex;
using ArbitrageBot.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitrageBot.Strategies
{
    [TestClass]
    public class TestStrategy : IStrategy
    {
        Bittrex bittrex = new Bittrex();
        Bitfinex bitfinex = new Bitfinex();
        Poloniex poloniex = new Poloniex();

        const string TEST_WITHDRAW_ADDRESS = "";

        const string BITTREX_TEST_MARKET = "BTC-LTC";
        const string BITTREX_TEST_CURRENCY = "LTC";
        static string BITTREX_ORDER_ID = "";

        const string BITFINEX_TEST_SYMBOL = "btcusd";
        const string BITFINEX_TEST_CURRENCY = "litecoin";
        const string BITFINEX_TEST_WALLET = "exchange";
        static string BITFINEX_ORDER_ID = "";

        const string POLONIEX_TEST_PAIR = "BTC_LTC";
        const string POLONIEX_TEST_CURRENCY = "LTC";
        static string POLONIEX_ORDER_ID = "";

        public void Run()
        {
            var list = this.GetType().GetMethods();
            foreach (var method in this.GetType().GetMethods())
            {
                var name = method.Name;
                if (method.Name.Contains("Test"))
                    method.Invoke(this, null);
            }
            Console.ReadLine();
        }

        //---Bittrex API call tests---//

        //public endpoints
        [TestMethod]
        public void Test_BittrexGetMarkets() => 
            AssertMethod("Bittrex GetMarkets", 
            () => new BittrexRequest().Public().GetMarkets());

        [TestMethod]
        public void Test_BittrexGetCurrencies() => 
            AssertMethod("Bittrex GetCurrencies", 
            () => new BittrexRequest().Public().GetCurrencies());

        [TestMethod]
        public void Test_BittrexGetTicker() => AssertMethod("Bittrex GetTicker", 
            () => new BittrexRequest().Public().GetTicker(BITTREX_TEST_MARKET));

        [TestMethod]
        public void Test_BittrexGetMarketSummaries() => 
            AssertMethod("Bittrex GetMarketSummaries", 
                () => new BittrexRequest().Public().GetMarketSummaries());

        [TestMethod]
        public void Test_BittrexGetMarketSummary() => 
            AssertMethod("Bittrex GetMarketSummary", 
                () => new BittrexRequest().Public().GetMarketSummary(BITTREX_TEST_MARKET));

        [TestMethod]
        public void Test_BittrexGetMarketHistory() => 
            AssertMethod("Bittrex GetMarketHistory", 
                () => new BittrexRequest().Public().GetMarketHistory(BITTREX_TEST_MARKET));

        [TestMethod]
        public void Test_BittrexGetOrderBook() => 
            AssertMethod("Bittrex GetOrderBook", 
                () => new BittrexRequest().Public().GetOrderBook(BITTREX_TEST_MARKET, "both"));
        //market endpoints

        [TestMethod]
        public void Test_BittrexBuyLimit() => 
            AssertMethod("Bittrex BuyLimit", 
                () => new BittrexRequest().Market().BuyLimit(BITTREX_TEST_MARKET, 0, 0));

        [TestMethod]
        public void Test_BittrexSellLimit() => 
            AssertMethod("Bittrex SellLimit", 
                () => new BittrexRequest().Market().SellLimit(BITTREX_TEST_MARKET, 0, 0));

        [TestMethod]
        public void Test_BittrexCancel() => 
            AssertMethod("Bittrex Cancel", 
                () => new BittrexRequest().Market().Cancel(BITTREX_ORDER_ID));

        [TestMethod]
        public void Test_BittrexGetOpenOrders() => 
            AssertMethod("Bittrex GetOpenOrders", 
                () => new BittrexRequest().Market().GetOpenOrders());
        //account endpoints

        [TestMethod]
        public void Test_BittrexGetDepositHistory() => 
            AssertMethod("Bittrex GetDepositHistory", 
                () => new BittrexRequest().Account().GetDepositHistory());

        [TestMethod]
        public void Test_BittrexGetWithdrawalHistory() => 
            AssertMethod("Bittrex GetWithdrawalHistory", 
                () => new BittrexRequest().Account().GetWithdrawalHistory());

        [TestMethod]
        public void Test_BittrexGetOrderHistory() => 
            AssertMethod("Bittrex GetOrderHistory", 
                () => new BittrexRequest().Account().GetOrderHistory());

        [TestMethod]
        public void Test_BittrexGetOrder() => 
            AssertMethod("Bittrex GetOrder", 
                () => new BittrexRequest().Account().GetOrder(BITTREX_ORDER_ID));

        [TestMethod]
        public void Test_BittrexGetBalances() => 
            AssertMethod("Bittrex GetBalances", 
                () => new BittrexRequest().Account().GetBalances());

        [TestMethod]
        public void Test_BittrexGetBalance() => 
            AssertMethod("Bittrex GetBalance", 
                () => new BittrexRequest().Account().GetBalance(BITTREX_TEST_CURRENCY));

        [TestMethod]
        public void Test_BittrexGetDepositAddress() => 
            AssertMethod("Bittrex GetDepositAddress", 
                () => new BittrexRequest().Account().GetDepositAddress(BITTREX_TEST_CURRENCY));



        //---Bitfinex API call tests---//

        [TestMethod]
        public void Test_BitfinexGetTicker() => 
            AssertMethod("Bitfinex GetTicker", 
                () => new BitfinexRequest().GetTicker(BITFINEX_TEST_SYMBOL));

        [TestMethod]
        public void Test_BitfinexGetStats() => 
            AssertMethod("Bitfinex GetStats", 
                () => new BitfinexRequest().GetStats(BITFINEX_TEST_SYMBOL));

        [TestMethod]
        public void Test_BitfinexGetSymbols() => 
            AssertMethod("Bitfinex GetSymbols", 
                () => new BitfinexRequest().GetSymbols());

        [TestMethod]
        public void Test_BitfinexGetSymbolDetails() => 
            AssertMethod("Bitfinex GetSymbolDetails", 
                () => new BitfinexRequest().GetSymbolDetails());

        [TestMethod]
        public void Test_BitfinexGetOrderBook() => 
            AssertMethod("Bitfinex GetOrderBook", 
                () => new BitfinexRequest().GetOrderBook(BITFINEX_TEST_SYMBOL));

        [TestMethod]
        public void Test_BitfinexGetTrades() => 
            AssertMethod("Bitfinex GetTrades", 
                () => new BitfinexRequest().GetTrades(BITFINEX_TEST_SYMBOL));

        [TestMethod]
        public void Test_BitfinexAccountInfo() => 
            AssertMethod("Bitfinex AccountInfo", 
                () => new BitfinexRequest().AccountInfo());

        [TestMethod]
        public void Test_BitfinexDeposit() => 
            AssertMethod("Bitfinex Deposit", 
                () => new BitfinexRequest().Deposit(BITFINEX_TEST_WALLET, BITFINEX_TEST_CURRENCY));

        [TestMethod]
        public void Test_BitfinexWalletBalances() => 
            AssertMethod("Bitfinex WalletBalances", 
                () => new BitfinexRequest().WalletBalances());

        [TestMethod]
        public void Test_BitfinexTransfer() => 
            AssertMethod("Bitfinex Transfer", 
                () => new BitfinexRequest().Transfer(0, BITFINEX_TEST_CURRENCY, BITFINEX_TEST_WALLET, "deposit"));

        [TestMethod]
        public void Test_BitfinexWithdraw() =>
            AssertMethod("Bitfinex Transfer",
                () => new BitfinexRequest().Withdraw(BITFINEX_TEST_CURRENCY, BITFINEX_TEST_WALLET, 0, TEST_WITHDRAW_ADDRESS));

        [TestMethod]
        public void Test_BitfinexNewOrder() => 
            AssertMethod("Bitfinex NewOrder", 
                () => new BitfinexRequest().NewOrder(BITFINEX_TEST_SYMBOL, 0, 0, "buy", "market", false, 0, 0));

        [TestMethod]
        public void Test_BitfinexCancelOrder() => 
            AssertMethod("Bitfinex CancelOrder", 
                () => new BitfinexRequest().CancelOrder(BITFINEX_ORDER_ID));

        [TestMethod]
        public void Test_BitfinexCancelOrders() => 
            AssertMethod("Bitfinex CancelOrders", 
                () => new BitfinexRequest().CancelOrders(new string[] { BITFINEX_ORDER_ID }));

        [TestMethod]
        public void Test_BitfinexCancelAllOrders() => 
            AssertMethod("Bitfinex CancelAllOrders", 
                () => new BitfinexRequest().CancelAllOrders());

        [TestMethod]
        public void Test_BitfinexOrderStatus() => 
            AssertMethod("Bitfinex OrderStatus", 
                () => new BitfinexRequest().OrderStatus(BITFINEX_ORDER_ID));

        [TestMethod]
        public void Test_BitfinexActiveOrders() => 
            AssertMethod("Bitfinex ActiveOrders", 
                () => new BitfinexRequest().ActiveOrders());

        [TestMethod]
        public void Test_BitfinexPastTrades() => 
            AssertMethod("Bitfinex PastTrades", 
                () => new BitfinexRequest().PastTrades(BITFINEX_TEST_SYMBOL, DateTime.Now));



        //---Poloniex API call tests---//

        //public endpoints
        [TestMethod]
        public void Test_PoloniexReturnTicker() => 
            AssertMethod("Poloniex ReturnTicker", 
                () => new PoloniexRequest().Public().ReturnTicker());

        [TestMethod]
        public void Test_PoloniexReturnOrderBook() => 
            AssertMethod("Poloniex ReturnOrderBook", 
                () => new PoloniexRequest().Public().ReturnOrderBook());

        [TestMethod]
        public void Test_PoloniexReturnTradeHistory() => 
            AssertMethod("Poloniex ReturnTradeHistory", 
                () => new PoloniexRequest().Public().ReturnTradeHistory(POLONIEX_TEST_PAIR, DateTime.Now, DateTime.Now));

        [TestMethod]
        public void Test_PoloniexReturnCurrencies() => 
            AssertMethod("Poloniex ReturnCurrencies", 
                () => new PoloniexRequest().Public().ReturnCurrencies());
        
        //trading endpoints
        [TestMethod]
        public void Test_PoloniexReturnBalances() => 
            AssertMethod("Poloniex ReturnBalances", 
                () => new PoloniexRequest().Trading().ReturnBalances());

        [TestMethod]
        public void Test_PoloniexReturnCompleteBalances() => 
            AssertMethod("Poloniex ReturnCompleteBalances", 
                () => new PoloniexRequest().Trading().ReturnCompleteBalances());

        [TestMethod]
        public void Test_PoloniexReturnDepositAddresses() => 
            AssertMethod("Poloniex ReturnDepositAddresses", 
                () => new PoloniexRequest().Trading().ReturnDepositAddresses());

        [TestMethod]
        public void Test_PoloniexReturnDepositsWithdrawals() => 
            AssertMethod("Poloniex ReturnDepositsWithdrawals", 
                () => new PoloniexRequest().Trading().ReturnDepositsWithdrawals(DateTime.Now, DateTime.Now));

        [TestMethod]
        public void Test_PoloniexGenerateNewAddress() => 
            AssertMethod("Poloniex GenerateNewAddress", 
                () => new PoloniexRequest().Trading().GenerateNewAddress());

        [TestMethod]
        public void Test_PoloniexReturnOpenOrders() => 
            AssertMethod("Poloniex ReturnOpenOrders", 
                () => new PoloniexRequest().Trading().ReturnOpenOrders(POLONIEX_TEST_PAIR));

        [TestMethod]
        public void Test_PoloniexReturnOrderTrades() => 
            AssertMethod("Poloniex ReturnOrderTrades", 
                () => new PoloniexRequest().Trading().ReturnOrderTrades(POLONIEX_ORDER_ID));

        [TestMethod]
        public void Test_PoloniexBuy() => 
            AssertMethod("Poloniex Buy", 
                () => new PoloniexRequest().Trading().Buy(POLONIEX_TEST_PAIR, 0, 0));

        [TestMethod]
        public void Test_PoloniexSell() => 
            AssertMethod("Poloniex Sell", 
                () => new PoloniexRequest().Trading().Sell(POLONIEX_TEST_PAIR, 0, 0));

        [TestMethod]
        public void Test_PoloniexCancelOrder() => 
            AssertMethod("Poloniex CancelOrder", 
                () => new PoloniexRequest().Trading().CancelOrder(POLONIEX_ORDER_ID));

        [TestMethod]
        public void Test_PoloniexMoveOrder() => 
            AssertMethod("Poloniex MoveOrder", 
                () => new PoloniexRequest().Trading().MoveOrder(POLONIEX_ORDER_ID, 0));

        [TestMethod]
        public void Test_PoloniexWithdraw() => 
            AssertMethod("Poloniex Withdraw", 
                () => new PoloniexRequest().Trading().Withdraw(POLONIEX_TEST_CURRENCY, 0, TEST_WITHDRAW_ADDRESS));

        [TestMethod]
        public void Test_PoloniexReturnAvailableBalances() => 
            AssertMethod("Poloniex ReturnAvailableBalances", 
                () => new PoloniexRequest().Trading().ReturnAvailableAccountBalances());

        [TestMethod]
        public void Test_PoloniexReturnTradeableBalances() => 
            AssertMethod("Poloniex ReturnTradeableBalances", 
                () => new PoloniexRequest().Trading().ReturnTradeableBalances());

        [AssemblyInitialize]
        public static void Setup(TestContext t)
        {
            //Config.ImportProperties(@"M:\Source\ArbitrageBot\config.txt");
            Config.ImportProperties(@"C:\Users\cmcgee\Desktop\arbot\config.txt");
            Logger.Initialize();
        }

        [TestInitialize]
        public void BeforeEach()
        {

        }

        [TestCleanup]
        public void AfterEach()
        {

        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            Logger.Close();
        }

        private void AssertMethod(string callId, Func<dynamic> method)
        {
            Logger.INFO("********Testing: " + callId + " ********");
            try
            { 
                dynamic data = method.Invoke();
                Assert.IsNotNull((object)data, "Unexpected error calling: " + callId + " returned null");
                Logger.INFO(data.ToString());
            }
            catch (Exception ex)
            {
                Logger.ERROR("Api call failed \n" + ex.Message);
                throw;
            }
            Logger.BREAK();
        }
    }
}
