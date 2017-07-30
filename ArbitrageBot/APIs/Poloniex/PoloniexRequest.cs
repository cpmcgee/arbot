using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.Numerics;
using ArbitrageBot.Util;

namespace ArbitrageBot.APIs.Poloniex
{
    public class PoloniexRequest : Request
    {
        new string Nonce
        {
            get
            {
                DateTime DateTimeUnixEpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return (new BigInteger(Math.Round(DateTime.UtcNow.Subtract(DateTimeUnixEpochStart).TotalMilliseconds * 1000, MidpointRounding.AwayFromZero)).ToString());
            }
        }

        private HttpClient client;

        public PoloniexRequest()
        {
            Url = "https://poloniex.com";
            client = new HttpClient();
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

        /// <summary>
        /// returns balances in wallets
        /// 
        /// {"BTC":"0.59098578","LTC":"3.31117268", ... }
        /// </summary>
        /// <returns></returns>
        public dynamic ReturnBalances()
        {
            return PostData(new Dictionary<string, object>
            {
                { "command", "returnBalances" },
                { "nonce", Nonce }
            });
        }

        protected override string GenerateSignature(string data)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);
            byte[] keyBytes = Encoding.ASCII.GetBytes(KeyLoader.PoloniexKeys.Item2);
            HMACSHA512 hasher = new HMACSHA512(keyBytes);
            return hasher.ComputeHash(dataBytes)
                .Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s); 
        }

        /// <summary>
        /// creates the parameters for the post call, poloniex does not take JSON
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private string CreateForm(Dictionary<string, object> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, object> param in dict)
            {
                if (sb.Length != 0)
                    sb.Append("&");
                sb.Append(param.Key);
                sb.Append("=");
                sb.Append(param.Value);
            }
            return sb.ToString();
        }

        /// <summary>
        /// creates a webrequest object for post calls
        /// </summary>
        /// <returns></returns>
        private HttpWebRequest CreateRequest()
        {
            var request = WebRequest.CreateHttp(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return request;
        }

        /// <summary>
        /// makes an api call with a post and returns the payload
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected dynamic PostData(object data)
        {
            try
            {
                var request = CreateRequest();
                string payload = CreateForm(data as Dictionary<string, object>);
                byte[] payloadBytes = Encoding.ASCII.GetBytes(payload);
                request.Headers["Sign"] = GenerateSignature(payload);
                request.Headers["Key"] = KeyLoader.PoloniexKeys.Item1;
                request.ContentLength = payloadBytes.Length;
                request.GetRequestStream().Write(payloadBytes, 0, payloadBytes.Length);
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
