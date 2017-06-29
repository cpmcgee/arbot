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
    public class BitfinexRequest
    {
        private string url = "https://api.bitfinex.com/v1";

        public dynamic GetTicker(string market)
        {
            url += "/pubticker/" + market;
            return GetData(url);
        }

        /// <summary>
        /// [
        ///  "btcusd",
        ///  "ltcusd",
        ///  "ltcbtc",
        ///  "ethusd",
        ///  "ethbtc",
        ///  "etcbtc",
        ///  "etcusd",
        ///  "bfxusd",
        ///  "bfxbtc",
        ///  "rrtusd",
        ///  "rrtbtc",
        ///  "zecusd",
        ///  "zecbtc"
        ///  ...
        ///]
        /// </summary>
        /// <returns></returns>
        public dynamic GetSymbols()
        {
            url += "/symbools";
            return GetData(url);
        }
        
        /// <summary>
        /// [{
        ///  "pair":"btcusd",
        ///  "price_precision":5,
        ///  "initial_margin":"30.0",
        ///  "minimum_margin":"15.0",
        ///  "maximum_order_size":"2000.0",
        ///  "minimum_order_size":"0.01",
        ///  "expiration":"NA"
        ///},{
        ///  "pair":"ltcusd",
        ///  "price_precision":5,
        ///  "initial_margin":"30.0",
        ///  "minimum_margin":"15.0",
        ///  "maximum_order_size":"5000.0",
        ///  "minimum_order_size":"0.1",
        ///  "expiration":"NA"
        ///},{
        ///  "pair":"ltcbtc",
        ///  "price_precision":5,
        ///  "initial_margin":"30.0",
        ///  "minimum_margin":"15.0",
        ///  "maximum_order_size":"5000.0",
        ///  "minimum_order_size":"0.1",
        ///  "expiration":"NA"
        ///},
        ///...
        ///]
        /// </summary>
        /// <returns></returns>
        public dynamic GetSymbolDetails()
        {
            url += "/symbols_details";
            return GetData(url);
        }

        private dynamic GetData(string url)
        {
            try
            {
                WebResponse response = ((HttpWebRequest)WebRequest.Create(url)).GetResponse();
                string raw = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                return JsonConvert.DeserializeObject(raw);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to access " + url + "\n" + ex.Message);
                throw;
            }
        }
    }
}
