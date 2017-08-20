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
        internal static void StartAsyncUpdates(int updateInterval)
        {
            run = true;
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
            Task.WhenAll(
                Task.Run(() => UpdatePrices()),
                Task.Run(() => UpdateBalances())).Wait();
        }

        /// <summary>
        /// reloding all coins from the api
        /// </summary>
        #region Loading Coins

        internal static void LoadCoins()
        {
            if (Currencies.Count == 0)
                Task.WhenAll(
                    Task.Run(() => GetBittrexCoins()),
                    Task.Run(() => GetBitfinexCoins()),
                    Task.Run(() => GetPoloniexCoins())).Wait();
        }

        private static void GetBittrexCoins()
        {
            BittrexCurrencies = new List<Currency>();
            dynamic data = new BittrexRequest().Public().GetCurrencies();
            var coins = data.result;
            foreach (var obj in coins)
            {
                string symbol = (string)obj.Currency;
                if (symbol == "BTC" || !((bool)obj.IsActive)) continue; //only add active coins traded against btc (dont add btc)
                Currency coin = CurrencyManager.GetCurrency(symbol);
                if (symbol == "AEON")
                    return;
                if (coin == null)
                {
                    coin = new Currency(symbol);
                    CurrencyManager.AddCurrency(coin.Symbol.ToUpper(), coin);
                }
                coin.BittrexName = obj.CurrencyLong;
                coin.BittrexBtcPair = ("BTC-" + symbol);
                BittrexCurrencies.Add(coin);
            }
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
                            Logger.ERROR("  currency " + symbol + " was not loaded in bittrex", 4);
                        }
                    }
                }
                Logger.INFO("Succesfully updated BITTREX prices", 3);
            }
            catch (Exception ex)
            {
                Logger.ERROR("Failed up update bittrex prices", 3);
            }
        }
        
        internal static void UpdateBitfinexPrices()
        {
            try
            {
                Parallel.ForEach(BitfinexCurrencies, coin =>
                {
                    var obj = new BitfinexRequest().GetTicker(coin.BitfinexBtcPair);
                    coin.BitfinexAsk = obj.ask;
                    coin.BitfinexBid = obj.bid;
                    coin.BitfinexLast = obj.last_price;
                    coin.BitfinexVolume = obj.volume;
                });
                Logger.INFO("Succesfully updated BITFINEX prices", 3);
            }
            catch(Exception ex)
            {
                Logger.ERROR("Failed up update bitfinex prices", 3);
            }
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
                Logger.INFO("Succesfully updated POLONIEX prices", 3);
            }
            catch (Exception ex)
            {
                Logger.ERROR("Failed up update poloniex prices", 3);
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
                Logger.INFO("Succesfully updated BITTREX balances", 3);
            }
            catch (Exception ex)
            {
                Logger.ERROR("Failed to update poloniex balances", 3);
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
                Logger.INFO("Succesfully updated POLONIEX balances", 3);
            }
            catch(Exception ex)
            {
                Logger.ERROR("Failed to update poloniex balances", 3);
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
                Logger.INFO("Succesfully updated BITFINEX balances", 3);
            }
            catch (Exception ex)
            {
                Logger.ERROR("Failed to update bitfinex balances", 3);
            }
        }

        #endregion
    }
}

