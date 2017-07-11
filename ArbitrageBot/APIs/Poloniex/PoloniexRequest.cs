using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace ArbitrageBot.APIs.Poloniex
{
    public class PoloniexRequest : Request
    {
        private string url = "https://poloniex.com";

        public PoloniexRequest Public()
        {
            url += "/public";
            return this;
        }

        /// <summary>
        /// {"
        /// BTC_LTC":{"last":"0.0251","lowestAsk":"0.02589999","highestBid":"0.0251","percentChange":"0.02390438",
        /// "baseVolume":"6.16485315","quoteVolume":"245.82513926"},"BTC_NXT":{"last":"0.00005730","lowestAsk":"0.00005710",
        /// "highestBid":"0.00004903","percentChange":"0.16701570","baseVolume":"0.45347489","quoteVolume":"9094"}, 
        /// ... }
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public dynamic ReturnTicker()
        {
            url += "?command=returnTicker";
            return GetData(url);
        }

        /// <summary>
        /// {
        /// "1CR":{"maxDailyWithdrawal":10000,"txFee":0.01,"minConf":3,"disabled":0},
        /// "ABY":{"maxDailyWithdrawal":10000000,"txFee":0.01,"minConf":8,"disabled":0}, 
        /// ... }
        /// </summary>
        /// <returns></returns>
        public dynamic ReturnCurrencies()
        {
            url += "?command=returnCurrencies";
            return GetData(url);
        }

        protected override dynamic GetData(string url)
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
