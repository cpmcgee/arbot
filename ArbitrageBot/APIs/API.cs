using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageBot.APIs
{
    public abstract class API
    {
        //api keys
        protected string Key { get; set; }
        protected string Secret { get; set; }
        protected Dictionary<string, decimal> btcPrices;

        public API()
        {
            btcPrices = new Dictionary<string, decimal>();
        }

        /// <summary>
        /// Gets the price of a given symbol in BTC
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public abstract decimal GetPriceInBtc(string symbol);

        /// <summary>
        /// returns a list of all symbols that actively trade against BTC
        /// </summary>
        /// <returns></returns>
        public abstract List<string> GetSymbols();

        /// <summary>
        /// sets the key and secret key of the exchange
        /// </summary>
        /// <param name="key"></param>
        /// <param name="secret"></param>
        public abstract void SetKeys(string key, string secret);
    }
}
