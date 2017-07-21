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


namespace ArbitrageBot.APIs.Bitfinex
{
    public class BitfinexRequest : Request
    {
        public BitfinexRequest()
        {
            Url = "https://api.bitfinex.com/v1";
        }
        
        /// <summary>
        /// {
        //  "mid":"244.755",
        //  "bid":"244.75",
        //  "ask":"244.76",
        //  "last_price":"244.82",
        //  "low":"244.2",
        //  "high":"248.19",
        //  "volume":"7842.11542563",
        //  "timestamp":"1444253422.348340958"
        //}
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public dynamic GetTicker(string market)
        {
            Url += "/pubticker/" + market;
            return GetData(Url);
        }

        /// <summary>
        ///[
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
            Url += "/symbools";
            return GetData(Url);
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
        ///
        ///]
        /// </summary>
        /// <returns></returns>
        public dynamic GetSymbolDetails()
        {
            Url += "/symbols_details";
            return GetData(Url);
        }

        /// <summary>
        /// Creates an authenticated post request with a new order for the account with the given keys
        /// </summary>
        /// <param name="Symbol"></param>
        /// <param name="Amount"></param>
        /// <param name="Price"></param>
        /// <param name="Side"></param>
        /// <param name="Type"></param>
        /// <param name="Exchange"></param>
        /// <param name="Is_Hidden"></param>
        /// <param name="Is_Postonly"></param>
        /// <param name="Use_All_Available"></param>
        /// <param name="Oco_Order"></param>
        /// <param name="Buy_Price_Oco"></param>
        /// <param name="Sell_Price_Oco"></param>
        /// <returns></returns>
        public string PostNewOrder(string Symbol, float Amount, float Price, string Side, string Type, string Exchange, bool Is_Hidden, bool Is_Postonly, int Use_All_Available, bool Oco_Order, float Buy_Price_Oco, float Sell_Price_Oco)
        {
            Url += "/order/new";

            var obj = new
            {
                request = Url,
                nonce = Nonce,
                symbol = Symbol,
                amount = Amount,
                price = Price,
                side = Side,
                type = Type,
                exchange = Exchange,
                is_hidden = Is_Hidden,
                is_postonly = Is_Postonly,
                use_all_available = Use_All_Available,
                ocoorder = Oco_Order,
                buy_price_oco = Buy_Price_Oco,
                sell_price_oco = Sell_Price_Oco 
            };
      
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// takes a string, converts it to byte array, then to base64 string as in order shown in example on bitfinex documentation
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected string EncodeBase64(string payload)
        {
            byte[] jsonBytes = Encoding.ASCII.GetBytes(payload);
            return Convert.ToBase64String(jsonBytes);
        }

        /// <summary>
        /// takes the string version of a json payload,
        /// converts it to string base64
        /// converts it to byte array
        /// hashes it with byte array version of private key
        /// converts it to hex string
        /// as in version on bitfinex documentation
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected override string GenerateSignature(string payload)
        {
            
            byte[] keyBytes = Encoding.ASCII.GetBytes(KeyLoader.BitfinexKeys.Item2);
            HMACSHA384 hasher = new HMACSHA384(keyBytes);
            string payload_base64 = EncodeBase64(payload);
            byte[] payloadBytes = Encoding.ASCII.GetBytes(payload_base64);
            return hasher.ComputeHash(payloadBytes)
                .Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s); //turns it back into bytes ¯\_(ツ)_/¯
        }

        /// <summary>
        /// makes an api call, returns JSON payload
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
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

        /// <summary>
        /// makes an api call with a post and returns the payload
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected dynamic PostData(string Url, string payload)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Content-Type:application/json");
                request.Headers.Add("Accept:application/json");
                request.Headers.Add("X-BFX-APIKEY:" + KeyLoader.BitfinexKeys.Item1);
                request.Headers.Add("X-BFX-PAYLOAD:" + EncodeBase64(payload));
                request.Headers.Add("X-BFX-SIGNATURE:" + GenerateSignature(payload));
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
