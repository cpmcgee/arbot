using System;
using System.Collections.Generic;
using ArbitrageBot.CurrencyUtil;

namespace ArbitrageBot.APIs.Bittrex
{
    public class Bittrex : API
    {
        protected override List<Currency> Currencies
        {
            get
            {
                return CurrencyManager.BittrexCurrencies;
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

        public override void SetKeys(string key, string secret)
        {
            this.Key = key;
            this.Secret = secret;
        }
    }
}
