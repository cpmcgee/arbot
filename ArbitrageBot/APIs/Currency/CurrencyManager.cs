using System;
using System.Collections.Generic;
using ArbitrageBot.Util;
using System.Collections.Concurrent;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Poloniex;
using System.Threading;
using System.Threading.Tasks;

namespace ArbitrageBot.CurrencyUtil
{
    /// <summary>
    /// This class actively manages currencies available on the exchanges and their properties
    /// </summary>
    static class CurrencyManager
    {
        //dictionary that contains references to all currencies had by all exchanges by their capitalized symbol
        private static ConcurrentDictionary<string, Currency> Currencies { get; set; } = new ConcurrentDictionary<string, Currency>();

        //Lists that contain the currencies held by each exchange
        internal static List<Currency> BittrexCurrencies { get; private set; } = new List <Currency>();
        internal static List<Currency> BitfinexCurrencies { get; private set; } = new List<Currency>();
        internal static List<Currency> PoloniexCurrencies { get; private set; } = new List<Currency>();

        internal static bool AddCurrency(string symbol, Currency currency)
        {
            return Currencies.TryAdd(symbol, currency);
        }

        /// <summary>
        /// returns the value in the dictionary
        /// returns null if not found
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal static Currency GetCurrency(string symbol)
        {
            Currency currency = null;
            Currencies.TryGetValue(symbol, out currency);
            return currency;
        }

        internal static bool HasCurrency(string symbol)
        {
            return Currencies.ContainsKey(symbol);
        }

        internal static ConcurrentDictionary<string, Currency> GetCurrencies()
        {
            return Currencies;
        }

        static bool run = false;
        internal static void StopAsyncUpdates() { run = false; }
        /// <summary>
        /// Start Asynchronous price and balance updates
        /// </summary>
        /// <param name="updateInterval">milliseconds between each full update (updates can take sometimes take up to 20 seconds)</param>
        internal static void StartAsyncUpdates(int updateInterval)
        {
            run = true;
            Logger.WRITE("Starting concurrent price and balance updates...", LogLevel.Info);
            Task.Run(() => UpdatePricesBalancesLoop(updateInterval));
        }
        private static void UpdatePricesBalancesLoop(int updateInterval)
        {
            while (run)
            {
                UpdatePricesBalances();
                Thread.Sleep(updateInterval);
            }
        }
        internal static void UpdatePricesBalances()
        {
            Logger.WRITE("Updating prices and balances", LogLevel.Info);
            Task.WhenAll(
                Task.Run(() => UpdatePrices()),
                Task.Run(() => UpdateBalances())).Wait();
        }

        /// <summary>
        /// reloding all coins from the apis
        /// </summary>
        #region Loading Coins

        internal static void LoadCoins()
        {
            if (Currencies.Count == 0)
            {
                Logger.WRITE("Loading coins available for trading", LogLevel.Info);
                Task.WhenAll(
                    Task.Run(() => GetBittrexCoins()),
                    Task.Run(() => GetBitfinexCoins()),
                    Task.Run(() => GetPoloniexCoins())).Wait();
            }
        }

        private static void GetBittrexCoins()
        {
            BittrexCurrencies = new List<Currency>();
            dynamic data = new BittrexRequest().Public().GetCurrencies();
            var coins = data.result;
            foreach (var obj in coins)
            {
                string symbol = (string)obj.Currency;
                if (symbol == "BTC")
                    continue; //only add active coins traded against btc (dont add btc)
                Currency coin = CurrencyManager.GetCurrency(symbol);
                if (coin == null)
                {
                    coin = new Currency(symbol);
                    CurrencyManager.AddCurrency(coin.Symbol.ToUpper(), coin);
                }
                coin.BittrexName = obj.CurrencyLong;
                coin.BittrexBtcPair = ("BTC-" + symbol);
                BittrexCurrencies.Add(coin);
            }
            Logger.WRITE("Succesfully loaded currencies available on Bittrex.", LogLevel.Info);
        }

        private static void GetBitfinexCoins()
        {
            var pairs = new BitfinexRequest().GetSymbols();
            BitfinexCurrencies = new List<Currency>();
            foreach (var pair in pairs)
            {
                string s = pair.ToString();
                if (s.Substring(s.Length - 3) == "btc")
                {
                    string symbol = s.Substring(0, s.Length - 3);
                    Currency coin = CurrencyManager.GetCurrency(symbol.ToUpper());
                    if (coin == null)
                    {
                        coin = new Currency(symbol.ToUpper());
                        CurrencyManager.AddCurrency(coin.Symbol.ToUpper(), coin);
                    }
                    coin.BitfinexBtcPair = pair;
                    coin.Symbol = symbol.ToUpper();
                    coin.BitfinexBtcPair = pair;
                    BitfinexCurrencies.Add(coin);
                }
            }
            Logger.WRITE("Succesfully loaded currencies available on Bitfinex.", LogLevel.Info);
        }

        private static void GetPoloniexCoins()
        {
            PoloniexCurrencies = new List<Currency>();
            var data = new PoloniexRequest().Public().ReturnTicker();
            foreach (var obj in data)
            {
                string[] pair = ((string)obj.Name).Split('_');
                string baseCurrency = pair[0];
                string symbol = pair[1].ToUpper();
                if (baseCurrency == "BTC")
                {
                    Currency coin = CurrencyManager.GetCurrency(symbol.ToUpper());
                    if (coin == null)
                    {
                        coin = new Currency(symbol.ToUpper());
                        CurrencyManager.AddCurrency(coin.Symbol.ToUpper(), coin);
                    }
                    coin.PoloniexBtcPair = obj.Name;
                    coin.PoloniexBid = obj.Value.highestBid;
                    coin.PoloniexAsk = obj.Value.lowestAsk;
                    coin.PoloniexLast = obj.Value.last;
                    coin.PoloniexVolume = obj.Value.quoteVolume;
                    PoloniexCurrencies.Add(coin);
                }
            }
            Logger.WRITE("Succesfully loaded currencies available on Poloniex.", LogLevel.Info);
        }

        #endregion

        /// <summary>
        /// updating prices of all coins from api
        /// </summary>
        #region Price Updates


        private static void UpdatePrices()
        {
            Task.WhenAll(
                    Task.Run(() => UpdateBittrexPrices()),
                    Task.Run(() => UpdateBitfinexPrices()),
                    Task.Run(() => UpdatePoloniexPrices())).Wait();
        }
        
        internal static void UpdateBittrexPrices()
        {
            try
            {
                var markets = new BittrexRequest().Public().GetMarketSummaries().result;
                foreach (var obj in markets)
                {
                    string[] pair = ((string)obj.MarketName).Split('-');
                    string baseCurrency = pair[0];
                    string symbol = pair[1].ToUpper();
                    if (baseCurrency.Equals("BTC"))
                    {
                        try
                        {
                            Currency coin = CurrencyManager.GetCurrency(symbol);
                            coin.BittrexLast = obj.Last;
                            coin.BittrexAsk = obj.Ask;
                            coin.BittrexBid = obj.Bid;
                            coin.BittrexVolume = obj.Volume;
                        }
                        catch (Exception ex)
                        {
                            Logger.WRITE("  currency " + symbol + " was not loaded in bittrex", LogLevel.Warning);
                        }
                    }
                }
                Logger.WRITE("Succesfully updated BITTREX prices", LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.WRITE("Failed to update bittrex prices \n" + ex.Message, LogLevel.Error);
            }
        }
        
        internal static void UpdateBitfinexPrices()
        {
            //unfortunately bitfinex makes you call the api for each ticker, so we do it in parallel to save time
            Parallel.ForEach(BitfinexCurrencies, coin =>
            {
                try
                {
                    var obj = new BitfinexRequest().GetTicker(coin.BitfinexBtcPair);
                    coin.BitfinexAsk = obj.ask;
                    coin.BitfinexBid = obj.bid;
                    coin.BitfinexLast = obj.last_price;
                    coin.BitfinexVolume = obj.volume;
                    Logger.WRITE("Updated bitfinex price for " + coin.Symbol, LogLevel.Debug);
                }
                catch (Exception ex)
                {
                    Logger.WRITE("Failed to update bitfinex price for "+ coin.Symbol + "\n" + ex.Message, LogLevel.Error);
                }
            });
            Logger.WRITE("Updated BITFINEX prices", LogLevel.Info);
        }
        
        private static void UpdatePoloniexPrices()
        {
            try
            {
                var data = new PoloniexRequest().Public().ReturnTicker();
                foreach (var obj in data)
                {
                    string[] pair = ((string)obj.Name).Split('_');
                    string baseCurrency = pair[0];
                    string symbol = pair[1].ToUpper();
                    if (baseCurrency == "BTC")
                    {
                        Currency coin = CurrencyManager.GetCurrency(symbol);

                        coin.PoloniexBid = obj.Value.highestBid;
                        coin.PoloniexAsk = obj.Value.lowestAsk;
                        coin.PoloniexLast = obj.Value.last;
                        coin.PoloniexVolume = obj.Value.quoteVolume;
                    }
                }
                Logger.WRITE("Succesfully updated POLONIEX prices", LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.WRITE("Failed to update poloniex prices \n" + ex.Message, LogLevel.Error);
            }
        }

        
        #endregion

        /// <summary>
        /// updating wallet balances from api
        /// </summary>
        #region Balance Updates

        private static void UpdateBalances()
        {
            Task.WhenAll(
                    Task.Run(() => UpdateBittrexBalances()),
                    Task.Run(() => UpdateBitfinexBalances()),
                    Task.Run(() => UpdatePoloniexBalances())).Wait();
        }

        internal static void UpdateBittrexBalances()
        {
            try
            {
                var data = new BittrexRequest().Account().GetBalances().result;
                foreach (var obj in data)
                {
                    Currency currency = null;
                    Currencies.TryGetValue(obj.Currency.ToString(), out currency);
                    if (currency != null)
                        currency.BittrexBalance = Convert.ToDouble(obj.Available);
                }
                Logger.WRITE("Succesfully updated BITTREX balances", LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.WRITE("Failed to update poloniex balances", LogLevel.Error);
            }
        }

        internal static void UpdatePoloniexBalances()
        {
            try
            {
                var data = new PoloniexRequest().Trading().ReturnBalances();
                foreach (var obj in data)
                {
                    Currency currency = null;
                    Currencies.TryGetValue(obj.Name.ToString(), out currency);
                    if (currency != null)
                        currency.PoloniexBalance = Convert.ToDouble(obj.Value);
                }
                Logger.WRITE("Succesfully updated POLONIEX balances", LogLevel.Info);
            }
            catch(Exception ex)
            {
                Logger.WRITE("Failed to update poloniex balances", LogLevel.Error);
            }
        }
        
        internal static void UpdateBitfinexBalances()
        {
            try
            {
                var data = new BitfinexRequest().WalletBalances();
                foreach (var obj in data)
                {
                    Currency currency = null;
                    Currencies.TryGetValue(obj.currency.ToString(), out currency);
                    if (currency != null)
                        currency.BitfinexBalance += Convert.ToDouble(obj.available);
                }
                Logger.WRITE("Succesfully updated BITFINEX balances", LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.WRITE("Failed to update bitfinex balances", LogLevel.Error);
            }
        }

        #endregion
    }
}

