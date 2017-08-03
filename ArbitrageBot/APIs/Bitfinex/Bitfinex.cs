using System;
using ArbitrageBot.CurrencyUtil;
using System.Collections.Generic;

namespace ArbitrageBot.APIs.Bitfinex
{
    public class Bitfinex : API
    {
        public Bitfinex()
        {
            GetCoins();
            UpdatePrices();
        }

        public static List<Currency> Currencies
        {
            get
            {
                return CurrencyManager.BitfinexCurrencies;
            }
            set
            {
                CurrencyManager.BitfinexCurrencies = value;
            }
        }

        public override decimal GetPriceInBtc(string symbol)
        {
            dynamic jsonData = new BitfinexRequest().GetTicker(symbol + "btc");
            if (jsonData.message == "Unknown symbol")
                throw new ArgumentException("No bitfinex market for " + symbol.ToUpper() + "/BTC");
            decimal price = Convert.ToDecimal(jsonData.last_price);
            return price;
        }

        public override List<string> GetSymbols()
        {
            dynamic data = new BitfinexRequest().GetSymbolDetails();
            List<string> symbols = new List<string>();
            foreach (var symbol in data)
            {
                string s = symbol.pair;
                if (s.Substring(s.Length - 3) == "btc")
                    symbols.Add(s.Substring(0, s.Length - 3));
            }
            return symbols;
        }

        /// <summary>
        /// reloads all coins from the api
        /// </summary>
        private static void GetCoins()
        {
            Currencies.Clear();
            var pairs = new BitfinexRequest().GetSymbols();
            foreach(var pair in pairs)
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
                    Currencies.Add(coin);
                }
            }
        }

        /// <summary>
        /// reloads prices from api (done in background, dont worry about it)
        /// </summary>
        public static void UpdatePrices()
        {
            foreach (Currency coin in Currencies)
            {
                var obj = new BitfinexRequest().GetTicker(coin.BitfinexBtcPair);
                coin.BitfinexAsk = obj.ask;
                coin.BitfinexBid = obj.bid;
                coin.BitfinexLast = obj.last_price;
                coin.BitfinexVolume = obj.volume;
            }
        }
    }
}
