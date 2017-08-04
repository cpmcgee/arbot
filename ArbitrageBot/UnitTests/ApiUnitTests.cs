using System;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Poloniex;
using ArbitrageBot.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace ArbitrageBot.Strategies
{
    /// <summary>
    /// A quick unit test suite i was able to make using lots of copy and paste
    /// this tests to make sure all the api calls are working
    /// </summary>
    [TestClass]
    public class ApiUnitTests : IStrategy
    {
        //Bittrex bittrex = new Bittrex();
        //Bitfinex bitfinex = new Bitfinex();
        //Poloniex poloniex = new Poloniex();

        const string TEST_WITHDRAW_ADDRESS = "";

        const string BITTREX_TEST_MARKET = "BTC-LTC";
        const string BITTREX_TEST_CURRENCY = "LTC";
        static int BITTREX_ORDER_ID = 0;

        const string BITFINEX_TEST_SYMBOL = "btcusd";
        const string BITFINEX_TEST_CURRENCY = "ltc";
        const string BITFINEX_TEST_WALLET = Bitfinex.WalletType.EXCHANGE;
        static int BITFINEX_ORDER_ID = 0;

        const string POLONIEX_TEST_PAIR = "BTC_LTC";
        const string POLONIEX_TEST_CURRENCY = "LTC";
        static int POLONIEX_ORDER_ID = 0;

        public void Run()
        {
            var list = this.GetType().GetMethods();
            foreach (var method in this.GetType().GetMethods())
            {
                var name = method.Name;
                if (method.Name.Contains("Test"))
                {
                    try
                    {
                        method.Invoke(this, null);
                    }
                    catch (Exception ex)
                    {
                        Logger.ERROR(ex.Message);
                    }
                }
            }
            Console.ReadLine();
        }

        //---Bittrex API call tests---//

        //public endpoints
        [TestMethod]
        public void Test_BittrexGetMarkets()  => new BittrexRequest().Public().GetMarkets();

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
        public void Test_BitfinexNewOrder() => new BitfinexRequest().NewOrder(BITFINEX_TEST_SYMBOL, "0", "0", "buy", "market", false, 0, 0);

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

        [AssemblyInitialize]
        public static void Setup(TestContext t)
        {
            Config.ImportProperties(@"M:\Source\ArbitrageBot\config.txt");
            //Config.ImportProperties(@"C:\Users\cmcgee\Desktop\arbot\config.txt");
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
    }
}
