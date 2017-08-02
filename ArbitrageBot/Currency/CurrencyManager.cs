using System;
using System.Collections.Generic;
using ArbitrageBot.Util;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Poloniex;
using System.ComponentModel;

namespace ArbitrageBot.CurrencyUtil
{
    static class CurrencyManager
    {
        public static Dictionary<string, Currency> Currencies = new Dictionary<string, Currency>();

        public static List<Currency> BittrexCurrencies = new List<Currency>();
        public static List<Currency> BitfinexCurrencies = new List<Currency>();
        public static List<Currency> PoloniexCurrencies = new List<Currency>();

        public static class PriceUpdater
        {
            static BackgroundWorker fullTimeWorker = new BackgroundWorker();
            //static BackgroundWorker btxInitWorker = new BackgroundWorker();
            //static BackgroundWorker bfxInitWorker = new BackgroundWorker();
            //static BackgroundWorker plxInitWorker = new BackgroundWorker();

            static Dictionary<string, Currency> Currencies { get { return CurrencyManager.Currencies; } }

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

