using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
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

        public PoloniexRequest Trading()
        {
            Url += "/tradingApi";
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
            return GetData();
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
            return GetData();
        }

        
        public dynamic ReturnBalances()
        {
            //Url += "?command=returnBalances";
            return PostData(new
            {
                command = "returnBalances",
                nonce = Nonce
            });
        }

        protected override string GenerateSignature(string data)
        {
            byte[] uriBytes = Encoding.UTF8.GetBytes(data);
            byte[] keyBytes = Encoding.UTF8.GetBytes(KeyLoader.PoloniexKeys.Item2);
            HMACSHA512 hasher = new HMACSHA512(keyBytes);
            return hasher.ComputeHash(uriBytes)
                .Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s); //turns it back into bytes ¯\_(ツ)_/¯
        }

        /// <summary>
        /// makes an api call with a post and returns the payload
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected dynamic PostData(object payload)
        {
            try
            {
                var request = ((HttpWebRequest)WebRequest.Create(Url));
                payload = JsonConvert.SerializeObject(payload) as string;
                request.Method = "POST";
                //request.Accept = "application/json";
                //request.ContentType = "application/json";
                request.Headers.Add("Key", KeyLoader.PoloniexKeys.Item1);
                request.Headers.Add("Sign", GenerateSignature((string)payload));
                WebResponse response = request.GetResponse();
                string raw = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return JsonConvert.DeserializeObject(raw);
            }
            catch (WebException wex)
            {
                StreamReader sr = new StreamReader(((HttpWebResponse)wex.Response).GetResponseStream());
                Logger.ERROR("Failed to access " + Url + "\n" + sr.ReadToEnd());
                return null;
            }
            catch (Exception ex)
            {
                Logger.ERROR("Error creating request for " + Url + "\n" + ex.Message);
                return null;
            }
        }

        protected override dynamic GetData()
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
