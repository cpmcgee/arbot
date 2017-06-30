using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageBot.APIs.Bittrex
{
    public class Bittrex : IAPI
    {
        public decimal GetPriceInBtc(string symbol)
        {
                dynamic jsonData = new BittrexRequest().Public().GetTicker("btc-" + symbol);
                return jsonData.result.Last;
        }

        public List<string> GetSymbols()
        {
            dynamic data = new BittrexRequest().Public().GetMarkets();
            List<string> symbols = new List<string>();
            foreach (var symbol in data.result)
            {
                Console.WriteLine(symbol.MarketCurrency);
                if (symbol.BaseCurrency == "BTC")
                    symbols.Add((string)symbol.MarketCurrency);
            }
            return symbols;
        }
    }
}
