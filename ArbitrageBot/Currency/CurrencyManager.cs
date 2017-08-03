using System;
using System.Collections.Generic;
using ArbitrageBot.Util;
using System.Collections.Concurrent;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Poloniex;
using System.ComponentModel;

namespace ArbitrageBot.CurrencyUtil
{
    static class CurrencyManager
    { 
        private static ConcurrentDictionary<string, Currency> Currencies = new ConcurrentDictionary<string, Currency>();

        public static List<Currency> BittrexCurrencies = new List<Currency>();
        public static List<Currency> BitfinexCurrencies = new List<Currency>();
        public static List<Currency> PoloniexCurrencies = new List<Currency>();

        public static void AddCurrency(string symbol, Currency currency)
        {
            Currencies.TryAdd(symbol, currency);
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

        public static class PriceUpdater
        {
            static BackgroundWorker fullTimeWorker = new BackgroundWorker();
            //static BackgroundWorker btxInitWorker = new BackgroundWorker();
            //static BackgroundWorker bfxInitWorker = new BackgroundWorker();
            //static BackgroundWorker plxInitWorker = new BackgroundWorker();

            private static ConcurrentDictionary<string, Currency> Currencies { get { return CurrencyManager.Currencies; } }

            static bool run = false;

            public static void Start()
            {
                fullTimeWorker.DoWork += new DoWorkEventHandler(RunAsync);
                fullTimeWorker.RunWorkerAsync();
            }

            private static void RunAsync(object sender, DoWorkEventArgs e)
            {
                run = true;
                while (run)
                {
                    UpdatePrices();
                }
            }

            public static void UpdatePrices()
            {
                Bittrex.UpdatePrices();
                Bitfinex.UpdatePrices();
                Poloniex.UpdatePrices();
            }

            public static void Stop()
            {
                run = false;
                fullTimeWorker.CancelAsync();
            }
        }
    }
}

