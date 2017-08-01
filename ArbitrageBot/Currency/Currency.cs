using System;
using System.Collections.Generic;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Poloniex;
using System.ComponentModel;

namespace ArbitrageBot.CurrencyUtil
{
    public class Currency
    {
        public string Symbol { get; private set; }

        public decimal ?BittrexPrice { get; private set; }

        public decimal ?BitfinexPrice { get; private set; }

        public decimal ?PoloniexPrice { get; private set; }

        private string BittrexBtcPair
        {
            get
            {
                return "BTC-" + Symbol;
            }
        }

        private string BitfinexBtcPair
        {
            get
            {
                return Symbol + "btc";
            }
        }

        private string PoloniexBtcPair
        {
            get
            {
                return "BTC_" + Symbol;
            }
        }

        public Currency(string name)
        {
            this.Symbol = name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Currency))
                return false;
            else
                return ((Currency)obj).Symbol.ToUpper() == Symbol.ToUpper();
        }

        public static class PriceUpdater
        {
            static BackgroundWorker fullTimeWorker = new BackgroundWorker();
            static BackgroundWorker btxInitWorker = new BackgroundWorker();
            static BackgroundWorker bfxInitWorker = new BackgroundWorker();
            static BackgroundWorker plxInitWorker = new BackgroundWorker();

            static List<Currency> Currencies { get { return CurrencyManager.Currencies; } }

            static Bittrex bittrex = new Bittrex();
            static Bitfinex bitfinex = new Bitfinex();
            static Poloniex poloniex = new Poloniex();

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

            public static void LoadPrices()
            {
                btxInitWorker.DoWork += new DoWorkEventHandler(LoadBittrexPrices);
                bfxInitWorker.DoWork += new DoWorkEventHandler(LoadBitfinexPrices);
                plxInitWorker.DoWork += new DoWorkEventHandler(LoadPoloniexPrices);
                btxInitWorker.RunWorkerAsync();
                bfxInitWorker.RunWorkerAsync();
                plxInitWorker.RunWorkerAsync();
            }

            public static void UpdatePrices()
            {
                foreach (Currency coin in Currencies)
                {
                    if (coin.BittrexPrice != null)
                        coin.BittrexPrice = bittrex.GetPriceInBtc(coin.Symbol);
                    if (coin.BitfinexPrice != null)
                        coin.BitfinexPrice = bitfinex.GetPriceInBtc(coin.Symbol);
                    if (coin.PoloniexPrice != null)
                        coin.PoloniexPrice = poloniex.GetPriceInBtc(coin.Symbol);
                }
            }

            public static void LoadBittrexPrices(object sender, DoWorkEventArgs e)
            {
                foreach (Currency coin in Currencies)
                {
                    try
                    {
                        coin.BittrexPrice = bittrex.GetPriceInBtc(coin.Symbol);
                        if (coin.BittrexPrice != null)
                            CurrencyManager.BittrexCurrencies.Add(coin);
                    }
                    catch
                    {
                        coin.BitfinexPrice = null;
                    }
                }
            }

            public static void LoadBitfinexPrices(object sender, DoWorkEventArgs e)
            {
                foreach (Currency coin in Currencies)
                {
                    try
                    {
                        coin.BitfinexPrice = bitfinex.GetPriceInBtc(coin.Symbol);
                        if (coin.BitfinexPrice != null)
                            CurrencyManager.BitfinexCurrencies.Add(coin);
                    }
                    catch
                    {
                        coin.BitfinexPrice = null;
                    }
                }
            }

            public static void LoadPoloniexPrices(object sender, DoWorkEventArgs e)
            {
                foreach (Currency coin in Currencies)
                {
                    try
                    {
                        coin.PoloniexPrice = poloniex.GetPriceInBtc(coin.Symbol);
                        if (coin.PoloniexPrice != null)
                            CurrencyManager.PoloniexCurrencies.Add(coin);
                    }
                    catch
                    {
                        coin.PoloniexPrice = null;
                    }
                }
            }

            public static void Stop()
            {
                run = false;
                fullTimeWorker.CancelAsync();
            }
        }
    }
}