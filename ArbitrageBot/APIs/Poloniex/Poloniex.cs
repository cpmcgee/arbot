using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitrageBot.APIs;

namespace ArbitrageBot.APIs.Poloniex
{
    public class Poloniex : IAPI
    {
        
        public List<string> GetSymbols()
        {
            try
            {
                dynamic data = new PoloniexRequest().Public().ReturnCurrencies();
                List<string> symbols = new List<string>();
                foreach (var symbol in data)
                {
                    symbols.Add((string)symbol.Name);
                }
                return symbols;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public decimal GetPriceInBtc(string ticker)
        {
            try
            { 
                dynamic data = new PoloniexRequest().Public().ReturnTicker();
                foreach (var coin in data)
                {
                    if (coin.Name == "BTC_" + ticker.ToUpper())
                        return coin.Value.last;
                }
                throw new Exception("Could not get ticker for:  " + ticker);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
