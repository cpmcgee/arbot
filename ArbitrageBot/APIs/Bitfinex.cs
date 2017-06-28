using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageBot.APIs
{
    public static class Bitfinex
    { 

        public static decimal GetPriceInBtc(string symbol)
        {
            dynamic jsonData = new BitfinexRequest().GetTicker(symbol + "btc");
            decimal ret = Convert.ToDecimal(jsonData.last_price);
            return ret;
        }
    }
}
