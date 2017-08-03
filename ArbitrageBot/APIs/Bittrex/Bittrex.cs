using System;
using System.Collections.Generic;
using ArbitrageBot.CurrencyUtil;

namespace ArbitrageBot.APIs.Bittrex
{
    public class Bittrex : API
    {
        public Bittrex()
        {
            GetCoins();
            UpdatePrices();
        }

        public static List<Currency> Currencies
        {
            get
            {
                return CurrencyManager.BittrexCurrencies;
            }
            set
            {
                CurrencyManager.BittrexCurrencies = value;
            }
        }

        public override decimal GetPriceInBtc(string symbol)
        {
            dynamic jsonData = new BittrexRequest().Public().GetTicker("btc-" + symbol);
            if (jsonData.message == "INVALID_MARKET" || jsonData.success == false)
                throw new ArgumentException("Failed request or no bittrex market for " + symbol + "/BTC");
            decimal price = Convert.ToDecimal(jsonData.result.Last);
            return price;
        }

        public override List<string> GetSymbols()
        {
            dynamic data = new BittrexRequest().Public().GetMarkets();
            List<string> symbols = new List<string>();
            foreach (var symbol in data.result)
            {
                if (symbol.BaseCurrency == "BTC")
                    symbols.Add((string)symbol.MarketCurrency);
            }
            return symbols;
        }

        public static void GetCoins()
        {
            Currencies.Clear();
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
                Currencies.Add(coin);
            }
        }

        /// <summary>
        /// reloads the coins from the API, 
        /// is automatically handled
        /// </summary>
        public static void UpdatePrices()
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
    }
}
