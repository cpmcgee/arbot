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

        static bool run = false;

        private static void StartAsyncPriceUpdates()
        {
            run = true;
            while (run)
            {
                Task.WhenAll(
                    Task.Run(() => Bittrex.UpdatePrices()),
                    Task.Run(() => Bitfinex.UpdatePrices()),
                    Task.Run(() => Poloniex.UpdatePrices())).Wait();
            }
        }

        public static void StopAsyncPriceUpdates()
        {
            run = false;
        }
    }
}

