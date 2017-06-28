using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace ArbitrageBot.APIs
{
    public static class Bittrex
    {
        private const string BASE_URL = "https://bittrex.com/api/v1.1";

        public static decimal GetPriceInBtc(string symbol)
        {
            try
            {
                dynamic jsonData = new BittrexRequest().Public().GetTicker("btc-" + symbol);
                return jsonData.result.Last;
            }
            return 0;
        }
    }
}
