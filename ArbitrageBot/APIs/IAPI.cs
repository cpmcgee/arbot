using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageBot.APIs
{
    interface IAPI
    {
        /// <summary>
        /// Gets the price of a given symbol in BTC
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        decimal GetPriceInBtc(string symbol);

        /// <summary>
        /// returns a list of all symbols that actively trade against BTC
        /// </summary>
        /// <returns></returns>
        List<string> GetSymbols();
    }
}
