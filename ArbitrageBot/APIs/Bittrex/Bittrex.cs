using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageBot.APIs.Bittrex
{
    public class Bittrex : API
    {
        public override decimal GetPriceInBtc(string symbol)
        {
            dynamic jsonData = new BittrexRequest().Public().GetTicker("btc-" + symbol);
            return jsonData.result.Last;
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
