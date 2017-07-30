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
        private string payload

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
            string payload = "command=returnBalances";
            payload += "&nonce=" + Nonce;
            return PostData(payload);
        }

        /// <summary>
        /// {"LTC":{"available":"5.015","onOrders":"1.0025","btcValue":"0.078"},"NXT:{...} ... }
        /// </summary>
        /// <returns>k</returns>
        public dynamic ReturnCompleteBalances()
        {
            string payload = "command=returnCompleteBalances";
            payload += "&nonce=" + Nonce;
            return PostData(payload);
        }

        /// <summary>
        /// {"BTC":"19YqztHmspv2egyD6jQM3yn81x5t5krVdJ","LTC":"LPgf9kjv9H1Vuh4XSaKhzBe8JHdou1WgUB", ... "ITC":"Press Generate.." ... }
        /// </summary>
        /// <returns></returns>
        public dynamic ReturnDepositAddresses()
        {
            string payload = "command=returnDepositAddresses";
            payload += "&nonce=" + Nonce;
            return PostData(payload);
        }

        /// <summary>
        /// {"deposits":
        ///[{"currency":"BTC","address":"...","amount":"0.01006132","confirmations":10,
        ///"txid":"17f819a91369a9ff6c4a34216d434597cfc1b4a3d0489b46bd6f924137a47701","timestamp":1399305798,"status":"COMPLETE"},{"currency":"BTC","address":"...","amount":"0.00404104","confirmations":10, 
        ///"txid":"7acb90965b252e55a894b535ef0b0b65f45821f2899e4a379d3e43799604695c","timestamp":1399245916,"status":"COMPLETE"}],
        ///"withdrawals":[{"withdrawalNumber":134933,"currency":"BTC","address":"1N2i5n8DwTGzUq2Vmn9TUL8J1vdr1XBDFg","amount":"5.00010000",
        ///"timestamp":1399267904,"status":"COMPLETE: 36e483efa6aff9fd53a235177579d98451c4eb237c210e66cd2b9a2d4a988f8e","ipAddress":"..."}]}
        /// </summary>
        /// <returns></returns>
        public dynamic ReturnDepositsWithdrawals(DateTime timeStart, DateTime timeEnd)
        {
            string start = UnixTimeStamp(timeStart); //unix timestamp conversion
            string end = UnixTimeStamp(timeEnd);
            string payload = "command=returnDepositAddresses";
            payload += "&nonce=" + Nonce;
            payload += "&start=" + start;
            payload += "&end=" + end;
            return PostData(payload);
        }

        /// <summary>
        /// {"success":1,"response":"CKXbbs8FAVbtEa397gJHSutmrdrBrhUMxe"}
        /// </summary>
        /// <returns></returns>
        public dynamic GenerateNewAddress()
        {
            string payload = "command=generateNewAddress";
            payload += "&nonce=" + Nonce;
            return PostData(payload);
        }

        /// <summary>
        /// currencyPair as "BTC_XCP" 
        /// [{"orderNumber":"120466","type":"sell","rate":"0.025","amount":"100","total":"2.5"},{"orderNumber":"120467","type":"sell","rate":"0.04","amount":"100","total":"4"}, ... ]
        /// 
        /// currencyPair "all"
        /// {"BTC_1CR":[],"BTC_AC":[{"orderNumber":"120466","type":"sell","rate":"0.025","amount":"100","total":"2.5"},{"orderNumber":"120467","type":"sell","rate":"0.04","amount":"100","total":"4"}], ... }
        /// </summary>
        /// <param name="currencyPair"></param>
        /// <returns></returns>
        public dynamic ReturnOpenOrders(string currencyPair)
        {
            string payload = "command=returnOpenOrders";
            payload += "&nonce=" + Nonce;
            payload += "&currencyPair=" + currencyPair;
            return PostData(payload);
        }

        /// <summary>
        /// [{ "globalTradeID": 25129732, "tradeID": "6325758", "date": "2016-04-05 08:08:40", "rate": "0.02565498", "amount": "0.10000000", "total": "0.00256549", "fee": "0.00200000", "orderNumber": "34225313575", "type": "sell", "category": "exchange" }, { "globalTradeID": 25129628, "tradeID": "6325741", "date": "2016-04-05 08:07:55", "rate": "0.02565499", "amount": "0.10000000", "total": "0.00256549", "fee": "0.00200000", "orderNumber": "34225195693", "type": "buy", "category": "exchange" }, ... ]
        /// 
        /// currencyPair "all" --
        /// {"BTC_MAID": [ { "globalTradeID": 29251512, "tradeID": "1385888", "date": "2016-05-03 01:29:55", "rate": "0.00014243", "amount": "353.74692925", "total": "0.05038417", "fee": "0.00200000", "orderNumber": "12603322113", "type": "buy", "category": "settlement" }, { "globalTradeID": 29251511, "tradeID": "1385887", "date": "2016-05-03 01:29:55", "rate": "0.00014111", "amount": "311.24262497", "total": "0.04391944", "fee": "0.00200000", "orderNumber": "12603319116", "type": "sell", "category": "marginTrade" }, ... ],"BTC_LTC":[ ... ] ... }
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="currencyPair"></param>
        /// <returns></returns>
        public dynamic ReturnTradeHistory(DateTime start, DateTime end, string currencyPair)
        {
            string payload = "command=returnTradeHistory";
            payload += "&nonce=" + Nonce;
            payload += "&currencyPair=" + currencyPair;
            payload += "&start=" + UnixTimeStamp(start);
            payload += "&end=" + UnixTimeStamp(end);
            return PostData(payload);
        }

        /// <summary>
        /// [{"globalTradeID": 20825863, "tradeID": 147142, "currencyPair": "BTC_XVC", "type": "buy", "rate": "0.00018500", "amount": "455.34206390", "total": "0.08423828", "fee": "0.00200000", "date": "2016-03-14 01:04:36"}, ...]
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public dynamic ReturnOrderTrades(int orderNumber)
        {
            string payload = "command=returnOrderTrades";
            payload += "&nonce=" + Nonce;
            payload += "&orderNumber=" + orderNumber;
            return PostData(payload);
        }

        /// <summary>
        /// {"orderNumber":31226040,"resultingTrades":[{"amount":"338.8732","date":"2014-10-18 23:03:21","rate":"0.00000173","total":"0.00058625","tradeID":"16164","type":"buy"}]}
        /// </summary>
        /// <param name="currencyPair"></param>
        /// <param name="rate"></param>
        /// <param name="amount"></param>
        /// <param name="fillOrKill"></param>
        /// <param name="immediateOrCancel"></param>
        /// <param name="postOnly"></param>
        /// <returns></returns>
        public dynamic Buy(string currencyPair, decimal rate, decimal amount, bool fillOrKill = false, bool immediateOrCancel = false, bool postOnly = false)
        {
            string payload = "command=buy";
            payload += "&currencyPair=" + currencyPair;
            payload += "&rate=" + rate.ToString();
            payload += "&amount=" + amount.ToString();
            payload += fillOrKill ? "&fillOrKill=1" : "";
            payload += immediateOrCancel ? "&immediateOrCancel=1" : "";
            payload += postOnly ? "&postOnly=1" : "";
            return PostData(payload);
        }

        /// <summary>
        /// same output as Buy
        /// </summary>
        /// <param name="currencyPair"></param>
        /// <param name="rate"></param>
        /// <param name="amount"></param>
        /// <param name="fillOrKill"></param>
        /// <param name="immediateOrCancel"></param>
        /// <param name="postOnly"></param>
        /// <returns></returns>
        public dynamic Sell(string currencyPair, decimal rate, decimal amount, bool fillOrKill = false, bool immediateOrCancel = false, bool postOnly = false)
        {
            string payload = "command=buy";
            payload += "&currencyPair=" + currencyPair;
            payload += "&rate=" + rate.ToString();
            payload += "&amount=" + amount.ToString();
            payload += fillOrKill ? "&fillOrKill=1" : "";
            payload += immediateOrCancel ? "&immediateOrCancel=1" : "";
            payload += postOnly ? "&postOnly=1" : "";
            return PostData(payload);
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
        protected dynamic PostData(string payload)
        {
            try
            {
                var request = CreateRequest();
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

        private string UnixTimeStamp(DateTime dt)
        {
            return (dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
        }
    }
}
