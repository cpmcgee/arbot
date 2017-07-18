using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using ArbitrageBot.Util;

namespace ArbitrageBot.APIs.Poloniex
{
    public class PoloniexRequest : Request
    {
        

        public PoloniexRequest()
        {
            Url = "https://poloniex.com";
        }

        public PoloniexRequest Public()
        {
            Url += "/public";
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
            Url += "?command=returnTicker";
            return GetData(Url);
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
            Url += "?command=returnCurrencies";
            return GetData(Url);
        }

        protected override dynamic GetData(string Url)
        {
            try
            {
                WebResponse response = ((HttpWebRequest)WebRequest.Create(Url)).GetResponse();
                string raw = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                return JsonConvert.DeserializeObject(raw);
            }
            catch (Exception ex)
            {
                Logger.ERROR("Failed to access " + Url + "\n" + ex.Message);
                return null;
            }
        }
    }
}
