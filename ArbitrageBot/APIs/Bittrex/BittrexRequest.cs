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

namespace ArbitrageBot.APIs.Bittrex
{
    public class BittrexRequest : Request
    {
        private bool authenticated = false;

        public BittrexRequest()
        {
            Url = "https://bittrex.com/api/v1.1";
        }

        /// <summary>
        /// below three methods allow for method chaining in order to make syntax similar to the url being access
        /// i.e. https://bittrex.com/api/v1.1/market/buylimit?market=.....
        ///      when called from Bittrex.cs - new Bittrex().Market().BuyLimit(market);
        /// </summary>
        /// <returns></returns>
        public BittrexRequest Public()
        {
            Url += "/public";
            return this;
        }

        public BittrexRequest Market()
        {
            Url += "/market";
            authenticated = true;
            return this;
        }

        public BittrexRequest Account()
        {
            Url += "/account";
            authenticated = true;
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
            Url += "/getticker?market=" + market;
            return GetData(Url);
        }

        /// <summary>
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : [{
        ///			"MarketName" : "BTC-LTC",
        ///			"High" : 0.01350000,
        ///			"Low" : 0.01200000,
        ///			"Volume" : 3833.97619253,
        ///			"Last" : 0.01349998,
        ///			"BaseVolume" : 47.03987026,
        ///			"TimeStamp" : "2014-07-09T07:22:16.72",
        ///			"Bid" : 0.01271001,
        ///			"Ask" : 0.01291100,
        ///			"OpenBuyOrders" : 45,
        ///			"OpenSellOrders" : 45,
        ///			"PrevDay" : 0.01229501,
        ///			"Created" : "2014-02-13T00:00:00",
        ///			"DisplayMarketName" : null
        ///
        ///        }
        ///    ]
        ///}
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public dynamic GetMarketSummary(string market)
        {
            Url += "/getmarketsummary?market=" + market;
            return GetData(Url);
        }

        /// <summary>
        /// Used to retrieve the latest trades that have occured for a specific market
        /// "success" : true,
           //	"message" : "",
        //	"result" : [{
        //			"Id" : 319435,
        //			"TimeStamp" : "2014-07-09T03:21:20.08",
        //			"Quantity" : 0.30802438,
        //			"Price" : 0.01263400,
        //			"Total" : 0.00389158,
        //			"FillType" : "FILL",
        //			"OrderType" : "BUY"
        //        }, {
        //			"Id" : 319433,
        //			"TimeStamp" : "2014-07-09T03:21:20.08",
        //			"Quantity" : 0.31820814,
        //			"Price" : 0.01262800,
        //			"Total" : 0.00401833,
        //			"FillType" : "PARTIAL_FILL",
        //			"OrderType" : "BUY"
        //		}, {
        //			"Id" : 319379,
        //			"TimeStamp" : "2014-07-09T02:58:48.127",
        //			"Quantity" : 49.64643541,
        //			"Price" : 0.01263200,
        //			"Total" : 0.62713377,
        //			"FillType" : "FILL",
        //			"OrderType" : "SELL"
        //		}, {
        //			"Id" : 319378,
        //			"TimeStamp" : "2014-07-09T02:58:46.27",
        //			"Quantity" : 0.35356459,
        //			"Price" : 0.01263200,
        //			"Total" : 0.00446622,
        //			"FillType" : "PARTIAL_FILL",
        //			"OrderType" : "BUY"
        //		}
        //	]
        //}
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public dynamic GetMarketHistory(string market)
        {
            Url += "/getmarkethistory?market=" + market;
            return GetData(Url);
        }

        /// <summary>
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : [{
        ///			"MarketCurrency" : "LTC",
        ///			"BaseCurrency" : "BTC",
        ///			"MarketCurrencyLong" : "Litecoin",
        ///			"BaseCurrencyLong" : "Bitcoin",
        ///			"MinTradeSize" : 0.01000000,
        ///			"MarketName" : "BTC-LTC",
        ///			"IsActive" : true,
        ///			"Created" : "2014-02-13T00:00:00"
        ///
        ///        }, {
        ///			"MarketCurrency" : "DOGE",
        ///			"BaseCurrency" : "BTC",
        ///			"MarketCurrencyLong" : "Dogecoin",
        ///			"BaseCurrencyLong" : "Bitcoin",
        ///			"MinTradeSize" : 100.00000000,
        ///			"MarketName" : "BTC-DOGE",
        ///			"IsActive" : true,
        ///			"Created" : "2014-02-13T00:00:00"
        ///		}
        ///    ]
        /// }
        /// </summary>
        /// <returns></returns>
        public dynamic GetMarkets()
        {
            Url += "/getmarkets";
            return GetData(Url);
        }

        /// <summary>
        /// used to place a limit order
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : {
        ///			"uuid" : "e606d53c-8d70-11e3-94b5-425861b86ab6"
        ///		}
        ///}
        /// </summary>
        /// <param name="market"></param>
        /// <param name="quantity"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public dynamic BuyLimit(string market, double quantity, double rate)
        {
            Url += string.Format("/buylimit?apikey={0}&market={1}&quantity={2}&rate={3}");
            return GetData(Url);
        }

        /// <summary>
        /// used to place a limit order
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : {
        ///			"uuid" : "e606d53c-8d70-11e3-94b5-425861b86ab6"
        ///		}
        ///}
        /// </summary>
        /// <param name="market"></param>
        /// <param name="quantity"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public dynamic SellLimit(string market, double quantity, double rate)
        {
            Url += string.Format("/selllimit?apikey={0}&market={1}&quantity={2}&rate={3}");
            return GetData(Url);
        }

        /// <summary>
        /// Used to retrieve all balances from your account
        ///  {
        //	"success" : true,
        //	"message" : "",
        //	"result" : [{
        //			"Currency" : "DOGE",
        //			"Balance" : 0.00000000,
        //			"Available" : 0.00000000,
        //			"Pending" : 0.00000000,
        //			"CryptoAddress" : "DLxcEt3AatMyr2NTatzjsfHNoB9NT62HiF",
        //			"Requested" : false,
        //			"Uuid" : null

        //        }, {
        //			"Currency" : "BTC",
        //			"Balance" : 14.21549076,
        //			"Available" : 14.21549076,
        //			"Pending" : 0.00000000,
        //			"CryptoAddress" : "1Mrcdr6715hjda34pdXuLqXcju6qgwHA31",
        //			"Requested" : false,
        //			"Uuid" : null
        //		}
        //	]
        //}
        /// </summary>
        /// <returns></returns>
        public dynamic GetBalances()
        {
            Url += string.Format("/getbalances?apikey={0}" , KeyLoader.BittrexKeys);
            return GetData(Url);
        }

        /// <summary>
        /// Used to retrieve the balance from your account for a specific currency
        /// {
        //	"success" : true,
        //	"message" : "",
        //	"result" : {
        //		"Currency" : "BTC",
        //		"Balance" : 4.21549076,
        //		"Available" : 4.21549076,
        //		"Pending" : 0.00000000,
        //		"CryptoAddress" : "1MacMr6715hjds342dXuLqXcju6fgwHA31",
        //		"Requested" : false,
        //		"Uuid" : null
        //	}
        //}
        /// 
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public dynamic GetBalance(string currency)
            {
                Url += string.Format("/getbalance?apikey={0}&currency={1}", KeyLoader.BittrexKeys.Item1, currency);
                return GetData(Url);
        }

        /// <summary>
        /// Used to retrieve or generate an address for a specific currency. If one does not exist, the call will fail and return ADDRESS_GENERATING until one is available
        /// {
	    // "success" : true,
	    // "message" : "",
	    // "result" : {
		//  "Currency" : "VTC",
	    //  "Address" : "Vy5SKeKGXUHKS2WVpJ76HYuKAu3URastUo"
	    // }
        //}
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public dynamic GetDepositAddress(string currency)
            {
                Url += string.Format("/getdepositaddress?apikey={0}&currency={1}", KeyLoader.BittrexKeys.Item1, currency);
                return GetData(Url);
            }

        /// <summary>
        /// Used to withdraw funds from your account. note: please account for txfee.
        /// {
        //	"success" : true,
        //	"message" : "",
        //	"result" : {
        //			"uuid" : "68b5a16c-92de-11e3-ba3b-425861b86ab6"
        //	}
        //}
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="quantity"></param>
        /// <param name="address"></param>
        /// <param name="paymentid"></param>
        /// <returns></returns>
        public dynamic Withdraw(string currency, string quantity, string address, string paymentid = "")
            {
                Url += string.Format("/withdraw?apikey={0}&currency={1}&quantity={2}&address={3}", KeyLoader.BittrexKeys.Item1, currency, quantity, address);
                if (paymentid != "")
                {
                    Url += "&paymentid=" + paymentid;
                }
                return GetData(Url);
            }

            public dynamic GetOrder(string uuid)
            {
                Url += string.Format("/getorder?apikey={0}&uuid={1}", KeyLoader.BittrexKeys.Item1, uuid);
                return GetData(Url);
            }

        /// <summary>
        /// takes the string version of a uri
        /// hashes it with byte array version of private key
        /// converts it to hex string
        /// as in version on bitfinex documentation
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected override string GenerateSignature(string uri)
        {
            byte[] uriBytes = Encoding.ASCII.GetBytes(uri);
            byte[] keyBytes = Encoding.ASCII.GetBytes(KeyLoader.BittrexKeys.Item2);
            HMACSHA512 hasher = new HMACSHA512(keyBytes);
            return hasher.ComputeHash(uriBytes)
                .Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s); //turns it back into bytes ¯\_(ツ)_/¯
        }

        /// <summary>
        /// checks if calling an authenticated endpoint
        /// creates an appropriate http request
        /// sends back the dynamic json object
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected override dynamic GetData(string url)
        {
            var request = ((HttpWebRequest)WebRequest.Create(url));
            if (authenticated)
            {
                request.Headers.Add("apisign", GenerateSignature(url));
            }
            try
            {
                WebResponse response = request.GetResponse();
                string raw = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                dynamic data = JsonConvert.DeserializeObject(raw);
                if (data.success == true)
                {
                    return data;
                }
                Logger.ERROR("Unsuccessful bittrex api call: " + url);
                return null;
            }
            catch(Exception ex)
            {
                Logger.ERROR("Failed to access " + url + "\n" + ex.Message);
                return null;
            }
        }
    }
}
