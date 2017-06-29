using System;
using System.Collections.Generic;

namespace ArbitrageBot.APIs
{
    public class Bitfinex : API
    { 
        public decimal GetPriceInBtc(string symbol)
        {
            dynamic jsonData = new BitfinexRequest().GetTicker(symbol + "btc");
            //decimal ret = 
                return Convert.ToDecimal(jsonData.last_price);
            //return ret;
        }

        public List<string> GetSymbols()
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
    }
}
