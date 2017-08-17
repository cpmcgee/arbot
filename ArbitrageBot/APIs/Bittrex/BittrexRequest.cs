using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using ArbitrageBot.Util;

namespace ArbitrageBot.APIs.Bittrex
{
    public class BittrexRequest : Request
    {
        protected bool authenticated = false;

        public BittrexRequest()
        {
            Url = "https://bittrex.com/api/v1.1";
        }

        /// <summary>
        /// below three methods allow for method chaining in order to make syntax similar to the url being access
        /// i.e. https://bittrex.com/api/v1.1/market/buylimit?market=.....
        ///      when called from Bittrex.cs - new Bittrex().Market().BuyLimit(market);
        /// By enforcing different types returned for each type of API call, intellisense will show...
        ///   ...what methods are available while writing a request
        /// These subclasses are implemented within this file and simply contain the methods
        /// </summary>
        /// <returns></returns>
        public PublicBittrexRequest Public()
        {
            return new PublicBittrexRequest();
        }

        public MarketBittrexRequest Market()
        {
            return new MarketBittrexRequest();
        }

        public AccountBittrexRequest Account()
        {
            return new AccountBittrexRequest();
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
            byte[] uriBytes = Encoding.UTF8.GetBytes(uri);
            byte[] keyBytes = Encoding.UTF8.GetBytes(KeyLoader.BittrexKeys.Item2);
            HMACSHA512 hasher = new HMACSHA512(keyBytes);
            return hasher.ComputeHash(uriBytes)
                .Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s); 
        }

        /// <summary>
        /// checks if calling an authenticated endpoint
        /// creates an appropriate http request
        /// sends back the dynamic json object
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected override dynamic GetData()
        {
            if (authenticated)
            {
                Url += "&nonce=" + Nonce;
            }
            var request = ((HttpWebRequest)WebRequest.Create(Url));
            if (authenticated)
            {
                request.Headers.Add("apisign", GenerateSignature(Url));
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
                Logger.ERROR("Unsuccessful bittrex api call: " + Url + "\n" + data.message);
                return null;
            }
            catch (WebException wex)
            {
                string error = new StreamReader(
                                     ((HttpWebResponse)wex.Response)
                                     .GetResponseStream())
                                     .ReadToEnd();
                throw new WebException("Failed api call: " + Url + "\n" + error);
                //Logger.ERROR("Failed to access " + Url + "\n" + error);
                //return null;
            }
        }

        protected override dynamic PostData(object payload)
        {
            throw new NotImplementedException();
        }
    }

    public class PublicBittrexRequest : BittrexRequest
    {
        public PublicBittrexRequest()
        {
            Url = "https://bittrex.com/api/v1.1/public";
            authenticated = false;
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
            return this.GetData();
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
            return GetData();
        }

        /// <summary>
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : [{
        ///			"Currency" : "BTC",
        ///			"CurrencyLong" : "Bitcoin",
        ///			"MinConfirmation" : 2,
        ///			"TxFee" : 0.00020000,
        ///			"IsActive" : true,
        ///			"CoinType" : "BITCOIN",
        ///			"BaseAddress" : null
        ///
        ///        }, {
        ///			"Currency" : "LTC",
        ///			"CurrencyLong" : "Litecoin",
        ///			"MinConfirmation" : 5,
        ///			"TxFee" : 0.00200000,
        ///			"IsActive" : true,
        ///			"CoinType" : "BITCOIN",
        ///			"BaseAddress" : null
        ///		}
        ///    ]
        ///}
        /// </summary>
        /// <returns></returns>
        public dynamic GetCurrencies()
        {
            Url += "/getcurrencies";
            return GetData();
        }

        /// <summary>
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : [{
        ///			"MarketName" : "BTC-888",
        ///			"High" : 0.00000919,
        ///			"Low" : 0.00000820,
        ///			"Volume" : 74339.61396015,
        ///			"Last" : 0.00000820,
        ///			"BaseVolume" : 0.64966963,
        ///			"TimeStamp" : "2014-07-09T07:19:30.15",
        ///			"Bid" : 0.00000820,
        ///			"Ask" : 0.00000831,
        ///			"OpenBuyOrders" : 15,
        ///			"OpenSellOrders" : 15,
        ///			"PrevDay" : 0.00000821,
        ///			"Created" : "2014-03-20T06:00:00",
        ///			"DisplayMarketName" : null
        ///
        ///        }, {
        ///			"MarketName" : "BTC-A3C",
        ///			"High" : 0.00000072,
        ///			"Low" : 0.00000001,
        ///			"Volume" : 166340678.42280999,
        ///			"Last" : 0.00000005,
        ///			"BaseVolume" : 17.59720424,
        ///			"TimeStamp" : "2014-07-09T07:21:40.51",
        ///			"Bid" : 0.00000004,
        ///			"Ask" : 0.00000005,
        ///			"OpenBuyOrders" : 18,
        ///			"OpenSellOrders" : 18,
        ///			"PrevDay" : 0.00000002,
        ///			"Created" : "2014-05-30T07:57:49.637",
        ///			"DisplayMarketName" : null
        ///		}
        ///    ]
        ///}
        /// </summary>
        /// <returns></returns>
        public dynamic GetMarketSummaries()
        {
            Url += "/getmarketsummaries";
            return GetData();
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
            return GetData();
        }

        /// <summary>
        /// Used to retrieve the latest trades that have occured for a specific market
        /// "success" : true,
        ///	"message" : "",
        ///	"result" : [{
        ///			"Id" : 319435,
        ///			"TimeStamp" : "2014-07-09T03:21:20.08",
        ///			"Quantity" : 0.30802438,
        ///			"Price" : 0.01263400,
        ///			"Total" : 0.00389158,
        ///			"FillType" : "FILL",
        ///			"OrderType" : "BUY"
        ///        }, {
        ///			"Id" : 319433,
        ///			"TimeStamp" : "2014-07-09T03:21:20.08",
        ///			"Quantity" : 0.31820814,
        ///			"Price" : 0.01262800,
        ///			"Total" : 0.00401833,
        ///			"FillType" : "PARTIAL_FILL",
        ///			"OrderType" : "BUY"
        ///		}, {
        ///			"Id" : 319379,
        ///			"TimeStamp" : "2014-07-09T02:58:48.127",
        ///			"Quantity" : 49.64643541,
        ///			"Price" : 0.01263200,
        ///			"Total" : 0.62713377,
        ///			"FillType" : "FILL",
        ///			"OrderType" : "SELL"
        ///		}, {
        ///			"Id" : 319378,
        ///			"TimeStamp" : "2014-07-09T02:58:46.27",
        ///			"Quantity" : 0.35356459,
        ///			"Price" : 0.01263200,
        ///			"Total" : 0.00446622,
        ///			"FillType" : "PARTIAL_FILL",
        ///			"OrderType" : "BUY"
        ///		}
        ///	]
        ///}
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public dynamic GetMarketHistory(string market)
        {
            Url += "/getmarkethistory?market=" + market;
            return GetData();
        }

        /// <summary>
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : {
        ///		"buy" : [{
        ///				"Quantity" : 12.37000000,
        ///				"Rate" : 0.02525000
        ///
        ///            }
        ///		],
        ///		"sell" : [{
        ///				"Quantity" : 32.55412402,
        ///				"Rate" : 0.02540000
        ///
        ///            }, {
        ///				"Quantity" : 60.00000000,
        ///				"Rate" : 0.02550000
        ///			}, {
        ///				"Quantity" : 60.00000000,
        ///				"Rate" : 0.02575000
        ///			}, {
        ///				"Quantity" : 84.00000000,
        ///				"Rate" : 0.02600000
        ///			}
        ///		]
        ///	}
        ///}
        /// </summary>
        /// <param name="market"></param>
        /// <param name="type"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public dynamic GetOrderBook(string market, string type, string depth = "")
        {
            Url += String.Format("/getorderbook?market={0}&type={1}", market, type);
            if (depth != "")
            {
                Url += "&depth=" + depth;
            }
            return GetData();
        }
    }

    public class MarketBittrexRequest : BittrexRequest
    {
        public MarketBittrexRequest()
        {
            Url = "https://bittrex.com/api/v1.1/market";
            authenticated = true;
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
            Url += string.Format("/buylimit?apikey={0}&market={1}&quantity={2}&rate={3}", KeyLoader.BittrexKeys.Item1, market, quantity, rate);
            return GetData();
        }

        /// <summary>
        /// used to place a limit order
        /// {
        ///	    "success" : true,
        ///	    "message" : "",
        ///	    "result" : 
        ///	    {
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
            Url += string.Format("/selllimit?apikey={0}&market={1}&quantity={2}&rate={3}", KeyLoader.BittrexKeys.Item1, market, quantity, rate);
            return GetData();
        }

        /// <summary>
        /// cancel an order
        /// {
        ///     "success" : true
        ///     "message" : ""
        ///     "result" : null
        /// }    
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public dynamic Cancel(string uuid)
        {
            Url += string.Format("/cancel?apikey={0}&uuid={1}", KeyLoader.BittrexKeys.Item1, uuid);
            return GetData();
        }

        /// <summary>
        /// Gets the open orders
        /// {
	    ///"success" : true,
	    ///"message" : "",
	    ///"result" : [{
	    ///		"Uuid" : null,
	    ///		"OrderUuid" : "09aa5bb6-8232-41aa-9b78-a5a1093e0211",
	    ///		"Exchange" : "BTC-LTC",
	    ///		"OrderType" : "LIMIT_SELL",
	    ///		"Quantity" : 5.00000000,
	    ///		"QuantityRemaining" : 5.00000000,
	    ///		"Limit" : 2.00000000,
	    ///		"CommissionPaid" : 0.00000000,
	    ///		"Price" : 0.00000000,
	    ///		"PricePerUnit" : null,
	    ///		"Opened" : "2014-07-09T03:55:48.77",
	    ///		"Closed" : null,
	    ///		"CancelInitiated" : false,
	    ///		"ImmediateOrCancel" : false,
	    ///		"IsConditional" : false,
	    ///		"Condition" : null,
	    ///		"ConditionTarget" : null
        ///
        ///    }, {
	    ///		"Uuid" : null,
	    ///		"OrderUuid" : "8925d746-bc9f-4684-b1aa-e507467aaa99",
	    ///		"Exchange" : "BTC-LTC",
	    ///		"OrderType" : "LIMIT_BUY",
	    ///		"Quantity" : 100000.00000000,
	    ///		"QuantityRemaining" : 100000.00000000,
	    ///		"Limit" : 0.00000001,
	    ///		"CommissionPaid" : 0.00000000,
	    ///		"Price" : 0.00000000,
	    ///		"PricePerUnit" : null,
	    ///		"Opened" : "2014-07-09T03:55:48.583",
	    ///		"Closed" : null,
	    ///		"CancelInitiated" : false,
	    ///		"ImmediateOrCancel" : false,
	    ///		"IsConditional" : false,
	    ///		"Condition" : null,
	    ///		"ConditionTarget" : null
	    ///	}
	    ///]
        ///}
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public dynamic GetOpenOrders(string market = "")
        {
            Url += string.Format("/getopenorders?apikey={0}", KeyLoader.BittrexKeys.Item1);
            if (market != null)
            {
                Url += "&market=" + market;
            }
            return GetData();
        }
    }

    public class AccountBittrexRequest : BittrexRequest
    {
        public AccountBittrexRequest()
        {
            Url = "https://bittrex.com/api/v1.1/account";
            authenticated = true;
        }
        
        /// <summary>
        /// gets the deposit history for the given currency
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : [{
        ///			"PaymentUuid" : "554ec664-8842-4fe9-b491-06225becbd59",
        ///			"Currency" : "BTC",
        ///			"Amount" : 0.00156121,
        ///			"Address" : "1K37yQZaGrPKNTZ5KNP792xw8f7XbXxetE",
        ///			"Opened" : "2014-07-11T03:41:25.323",
        ///			"Authorized" : true,
        ///			"PendingPayment" : false,
        ///			"TxCost" : 0.00020000,
        ///			"TxId" : "70cf6fdccb9bd38e1a930e13e4ae6299d678ed6902da710fa3cc8d164f9be126",
        ///			"Canceled" : false,
        ///			"InvalidAddress" : false
        ///
        ///        }, {
        ///			"PaymentUuid" : "d3fdf168-3d8e-40b6-8fe4-f46e2a7035ea",
        ///			"Currency" : "BTC",
        ///			"Amount" : 0.11800000,
        ///			"Address" : "1Mrcar6715hjds34pdXuLqXcju6QgwHA31",
        ///			"O
        ///			pened" : "2014-07-03T20:27:07.163",
        ///			"Authorized" : true,
        ///			"PendingPayment" : false,
        ///			"TxCost" : 0.00020000,
        ///			"TxId" : "3efd41b3a051433a888eed3ecc174c1d025a5e2b486eb418eaaec5efddda22de",
        ///			"Canceled" : false,
        ///			"InvalidAddress" : false
        ///		}
        ///    ]
        ///}
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public dynamic GetDepositHistory(string currency = "")
        {
            Url += string.Format("/getdeposithistory?apikey={0}", KeyLoader.BittrexKeys.Item1);
            if (currency != "")
            {
                Url += "&currency=" + currency;
            }
            return GetData();
        }

        /// <summary>
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : [{
        ///			"PaymentUuid" : "b52c7a5c-90c6-4c6e-835c-e16df12708b1",
        ///			"Currency" : "BTC",
        ///			"Amount" : 17.00000000,
        ///			"Address" : "1DeaaFBdbB5nrHj87x3NHS4onvw1GPNyAu",
        ///			"Opened" : "2014-07-09T04:24:47.217",
        ///			"Authorized" : true,
        ///			"PendingPayment" : false,
        ///			"TxCost" : 0.00020000,
        ///			"TxId" : null,
        ///			"Canceled" : true,
        ///			"InvalidAddress" : false
        ///
        ///        }, {
        ///			"PaymentUuid" : "f293da98-788c-4188-a8f9-8ec2c33fdfcf",
        ///			"Currency" : "XC",
        ///			"Amount" : 7513.75121715,
        ///			"Address" : "XVnSMgAd7EonF2Dgc4c9K14L12RBaW5S5J",
        ///			"Opened" : "2014-07-08T23:13:31.83",
        ///			"Authorized" : true,
        ///			"PendingPayment" : false,
        ///			"TxCost" : 0.00002000,
        ///			"TxId" : "b4a575c2a71c7e56d02ab8e26bb1ef0a2f6cf2094f6ca2116476a569c1e84f6e",
        ///			"Canceled" : false,
        ///			"InvalidAddress" : false
        ///		}
        ///	]
        ///}
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public dynamic GetWithdrawalHistory(string currency = "")
        {
            Url += string.Format("/getwithdrawalhistory?apikey={0}", KeyLoader.BittrexKeys.Item1); //add api key
            if (currency != "")
            {
                Url += "&currency=" + currency;
            }
            return GetData();
        }

        /// <summary>
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : [{
        ///			"OrderUuid" : "fd97d393-e9b9-4dd1-9dbf-f288fc72a185",
        ///			"Exchange" : "BTC-LTC",
        ///			"TimeStamp" : "2014-07-09T04:01:00.667",
        ///			"OrderType" : "LIMIT_BUY",
        ///			"Limit" : 0.00000001,
        ///			"Quantity" : 100000.00000000,
        ///			"QuantityRemaining" : 100000.00000000,
        ///			"Commission" : 0.00000000,
        ///			"Price" : 0.00000000,
        ///			"PricePerUnit" : null,
        ///			"IsConditional" : false,
        ///			"Condition" : null,
        ///			"ConditionTarget" : null,
        ///			"ImmediateOrCancel" : false
        ///
        ///        }, {
        ///			"OrderUuid" : "17fd64d1-f4bd-4fb6-adb9-42ec68b8697d",
        ///			"Exchange" : "BTC-ZS",
        ///			"TimeStamp" : "2014-07-08T20:38:58.317",
        ///			"OrderType" : "LIMIT_SELL",
        ///			"Limit" : 0.00002950,
        ///			"Quantity" : 667.03644955,
        ///			"QuantityRemaining" : 0.00000000,
        ///			"Commission" : 0.00004921,
        ///			"Price" : 0.01968424,
        ///			"PricePerUnit" : 0.00002950,
        ///			"IsConditional" : false,
        ///			"Condition" : null,
        ///			"ConditionTarget" : null,
        ///			"ImmediateOrCancel" : false
        ///		}
        ///	]
        ///}
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public dynamic GetOrderHistory(string market = "")
        {
            Url += string.Format("/getorderhistory?apikey={0}", KeyLoader.BittrexKeys.Item1);
            if (market != "")
            {
                Url += "&market=" + market;
            }
            return GetData();
        }

        /// <summary>
        /// Used to retrieve all balances from your account
        ///  {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : [{
        ///			"Currency" : "DOGE",
        ///			"Balance" : 0.00000000,
        ///			"Available" : 0.00000000,
        ///			"Pending" : 0.00000000,
        ///			"CryptoAddress" : "DLxcEt3AatMyr2NTatzjsfHNoB9NT62HiF",
        ///			"Requested" : false,
        ///			"Uuid" : null
        ///
        ///        }, {
        ///			"Currency" : "BTC",
        ///			"Balance" : 14.21549076,
        ///			"Available" : 14.21549076,
        ///			"Pending" : 0.00000000,
        ///			"CryptoAddress" : "1Mrcdr6715hjda34pdXuLqXcju6qgwHA31",
        ///			"Requested" : false,
        ///			"Uuid" : null
        ///		}
        ///	]
        ///}
        /// </summary>
        /// <returns></returns>
        public dynamic GetBalances()
        {
            Url += string.Format("/getbalances?apikey={0}", KeyLoader.BittrexKeys.Item1);
            return GetData();
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
            return GetData();
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
            return GetData();
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
            return GetData();
        }

        /// <summary>
        /// {
        ///	"success" : true,
        ///	"message" : "",
        ///	"result" : {
        ///		"AccountId" : null,
        ///		"OrderUuid" : "0cb4c4e4-bdc7-4e13-8c13-430e587d2cc1",
        ///		"Exchange" : "BTC-SHLD",
        ///		"Type" : "LIMIT_BUY",
        ///		"Quantity" : 1000.00000000,
        ///		"QuantityRemaining" : 1000.00000000,
        ///		"Limit" : 0.00000001,
        ///		"Reserved" : 0.00001000,
        ///		"ReserveRemaining" : 0.00001000,
        ///		"CommissionReserved" : 0.00000002,
        ///		"CommissionReserveRemaining" : 0.00000002,
        ///		"CommissionPaid" : 0.00000000,
        ///		"Price" : 0.00000000,
        ///		"PricePerUnit" : null,
        ///		"Opened" : "2014-07-13T07:45:46.27",
        ///		"Closed" : null,
        ///		"IsOpen" : true,
        ///		"Sentinel" : "6c454604-22e2-4fb4-892e-179eede20972",
        ///		"CancelInitiated" : false,
        ///		"ImmediateOrCancel" : false,
        ///		"IsConditional" : false,
        ///		"Condition" : "NONE",
        ///		"ConditionTarget" : null
        ///	}
        ///}
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public dynamic GetOrder(string uuid)
        {
            Url += string.Format("/getorder?apikey={0}&uuid={1}", KeyLoader.BittrexKeys.Item1, uuid);
            return GetData();
        }
    }
}
