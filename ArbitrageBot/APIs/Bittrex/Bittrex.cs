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
            try
            {
                dynamic jsonData = new BittrexRequest().Public().GetTicker("btc-" + symbol);
                return jsonData.result.Last;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> GetSymbols()
        {
            try
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
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
