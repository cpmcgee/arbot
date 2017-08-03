using System;
using System.Collections.Generic;
using ArbitrageBot.CurrencyUtil;

namespace ArbitrageBot.APIs.Poloniex
{
    public class Poloniex : API
    {
        public Poloniex()
        {
            GetCoins();
            UpdatePrices();
        }

        public static List<Currency> Currencies
        {
            get
            {
                return CurrencyManager.PoloniexCurrencies;
            }
            set
            {
                CurrencyManager.PoloniexCurrencies = value;
            }
        }

        public override decimal GetPriceInBtc(string symbol)
        {
            dynamic data = new PoloniexRequest().Public().ReturnTicker();
            foreach (var coin in data)
            {
                if (coin.Name == "BTC_" + symbol.ToUpper())
                {
                    decimal price = Convert.ToDecimal(coin.Value.last);
                    return price;
                }
            }
            throw new ArgumentException("No poloniex market for " + symbol + "/BTC");
        }

        public override List<string> GetSymbols()
        {
            dynamic data = new PoloniexRequest().Public().ReturnCurrencies();
            List<string> symbols = new List<string>();
            foreach (var symbol in data)
            {
                symbols.Add((string)symbol.Name);
            }
            return symbols;
        }

        private static void GetCoins()
        {
            Currencies.Clear();
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
                    Currencies.Add(coin);
                }
            }
        }

        public static void UpdatePrices()
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
    }
}
