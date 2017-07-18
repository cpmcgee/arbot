﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using ArbitrageBot.Util;

namespace ArbitrageBot.APIs.Bittrex
{
    public class BittrexRequest : Request
    {
        public BittrexRequest()
        {
            Url = "https://bittrex.com/api/v1.1";
        }

        public BittrexRequest Public()
        {
            Url += "/public";
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

        protected override dynamic GetData(string Url)
        {
            try
            {
                WebResponse response = ((HttpWebRequest)WebRequest.Create(Url)).GetResponse();
                string raw = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                return JsonConvert.DeserializeObject(raw);
            }
            catch(Exception ex)
            {
                Logger.ERROR("Failed to access " + Url + "\n" + ex.Message);
                return null;
            }
        }
    }
}
