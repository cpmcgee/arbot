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
    public class BittrexRequest
    {
        private string url = "https://bittrex.com/api/v1.1";

        public BittrexRequest()
        {

        }

        public BittrexRequest Public()
        {
            url += "/public";
            return this;
        }

        /// <summary>
        /// Gets JSON data from bittrex 
        /// Response Example: 
        /// {
        /// 	"success" : true,
        /// 	"message" : "",
        /// 	"result" : {
        /// 		"Bid" : 2.05670368,
        /// 		"Ask" : 3.35579531,
        /// 		"Last" : 3.35579531
        /// 	}
        /// }
        /// </summary>
        /// <param name="market">string literal for market (ex: "btc-ltc"</param>
        /// <returns></returns>
        public dynamic GetTicker(string market)
        {
            try
            {
                url += "/getticker?market=" + market;
                return JsonConvert.DeserializeObject(GetData(url));
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR GETTING TICKER FOR " + market);
                return null;
            }
        }

        public dynamic GetMarketSummary(string market)
        {
            try
            {
                url += "/getmarketsummary?market=" + market;
                return JsonConvert.DeserializeObject(GetData(url));
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR GETTING MARKET SUMMARY FOR " + market);
                return null;
            }
        }

        private string GetData(string url)
        {
            WebResponse response = ((HttpWebRequest)WebRequest.Create(url)).GetResponse();
            return new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
        }
    }
}
