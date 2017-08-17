using System;
using System.Collections.Generic;
using ArbitrageBot.Util;
using System.Collections.Concurrent;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Poloniex;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ArbitrageBot.CurrencyUtil
{
    /// <summary>
    /// This class actively manages currencies available on the exchanges and their properties
    /// </summary>
    static class CurrencyManager
    { 
        //dictionary that contains references to all currencies had by all exchanges by their capitalized symbol
        private static ConcurrentDictionary<string, Currency> Currencies = new ConcurrentDictionary<string, Currency>();

        //Lists that contain the currencies held by each exchange
        public static List<Currency> BittrexCurrencies = new List<Currency>();
        public static List<Currency> BitfinexCurrencies = new List<Currency>();
        public static List<Currency> PoloniexCurrencies = new List<Currency>();

        public static bool AddCurrency(string symbol, Currency currency)
        {
            return Currencies.TryAdd(symbol, currency);
        }

        /// <summary>
        /// returns the value in the dictionary
        /// returns null if not found
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static Currency GetCurrency(string symbol)
        {
            Currency currency = null;
            Currencies.TryGetValue(symbol, out currency);
            return currency;
        }

        public static bool HasCurrency(string symbol)
        {
            return Currencies.ContainsKey(symbol);
        }

        public static ConcurrentDictionary<string, Currency> GetCurrencies()
        {
            return Currencies;
        }

        public static void LoadCoins()
        {
            Task.WhenAll(
                    Task.Run(() => GetBittrexCoins()),
                    Task.Run(() => GetBitfinexCoins()),
                    Task.Run(() => GetPoloniexCoins())).Wait();
        }


        #region Price Updates


        public static void UpdatePrices()
        {
            Task.WhenAll(
                    Task.Run(() => UpdateBittrexPrices()),
                    Task.Run(() => UpdateBitfinexPrices()),
                    Task.Run(() => UpdatePoloniexPrices())).Wait();
        }
        
        public static void UpdateBittrexPrices()
        {
            var markets = new BittrexRequest().Public().GetMarketSummaries().result;
            foreach (var obj in markets)
            {
                string[] pair = ((string)obj.MarketName).Split('-');
                string baseCurrency = pair[0];
                string symbol = pair[1];
                if (baseCurrency.Equals("BTC"))
                {
                    Currency coin = CurrencyManager.GetCurrency(symbol.ToUpper());
                    if (coin != null)
                    {
                        coin.BittrexLast = obj.Last;
                        coin.BittrexAsk = obj.Ask;
                        coin.BittrexBid = obj.Bid;
                        coin.BittrexVolume = obj.Volume;
                    }
                }
            }
        }

        public static void GetBittrexCoins()
        {
            BittrexCurrencies.Clear();
            dynamic data = new BittrexRequest().Public().GetCurrencies();
            var coins = data.result;
            foreach (var obj in coins)
            {
                string symbol = (string)obj.Currency;
                if (symbol == "BTC" || !((bool)obj.IsActive)) continue; //only add active coins traded against btc (dont add btc)
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
        }

        /// <summary>
        /// reloads all coins from the api
        /// </summary>
        private static void GetBitfinexCoins()
        {
            var pairs = new BitfinexRequest().GetSymbols();
            BitfinexCurrencies.Clear();
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

        /// <summary>
        /// reloads prices from api (done in background, dont worry about it)
        /// </summary>
        public static void UpdateBitfinexPrices()
        {
            foreach (Currency coin in BitfinexCurrencies)
            {
                var obj = new BitfinexRequest().GetTicker(coin.BitfinexBtcPair);
                coin.BitfinexAsk = obj.ask;
                coin.BitfinexBid = obj.bid;
                coin.BitfinexLast = obj.last_price;
                coin.BitfinexVolume = obj.volume;
            }
        }

        private static void GetPoloniexCoins()
        {
            PoloniexCurrencies.Clear();
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

        public static void UpdatePoloniexPrices()
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
                    coin.PoloniexVolume = obj.Value.quoteVolum;
                }
            }
        }
        
        static bool run = false;

        private static void StartAsyncPriceUpdates()
        {
            run = true;
            while (run)
            {
                UpdatePrices();
            }
        }

        public static void StopAsyncPriceUpdates()
        {
            run = false;
        }

        #endregion

        #region Balance Updates

        //update balances here

        public static void UpdateBitfinexBalances()
        {
            var data = new BitfinexRequest().WalletBalances();
            foreach (var obj in data)
            {
                Currencies[obj.currency] += Convert.ToDouble(obj.available);
            }
        }

        #endregion
    }
}

