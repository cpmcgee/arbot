using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using ArbitrageBot.Util;


namespace ArbitrageBot.APIs.Bitfinex
{
    public class BitfinexRequest : Request
    {
        string req = "/v1"; //will be built upon to match tail end of Url as needed for POST requests
         
        public BitfinexRequest()
        {
            Url = "https://api.bitfinex.com/v1";
        }
        
        /// <summary>
        /// {
        ///  "mid":"244.755",
        ///  "bid":"244.75",
        ///  "ask":"244.76",
        ///  "last_price":"244.82",
        ///  "low":"244.2",
        ///  "high":"248.19",
        ///  "volume":"7842.11542563",
        ///  "timestamp":"1444253422.348340958"
        //}
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public dynamic GetTicker(string market)
        {
            Url += "/pubticker/" + market;
            return GetData();
        }

        /// <summary>
        /// [{
        ///  "period":1,
        ///  "volume":"7967.96766158"
        ///},{
        ///  "period":7,
        ///  "volume":"55938.67260266"
        ///},{
        ///  "period":30,
        ///  "volume":"275148.09653645"
        ///}]
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public dynamic GetStats(string symbol)
        {
            Url += "/stats/" + symbol;
            return GetData();
        }

        /// <summary>
        /// {
        ///  "bids":[{
        ///    "price":"574.61",
        ///    "amount":"0.1439327",
        ///    "timestamp":"1472506127.0"
        ///  }],
        ///  "asks":[{
        ///    "price":"574.62",
        ///    "amount":"19.1334",
        ///    "timestamp":"1472506126.0"
        ///  }]
        ///}
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public dynamic GetOrderBook(string symbol)
        {
            Url += "/book/" + symbol;
            return GetData();
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
            Url += "/symbols";
            return GetData();
        }

        /// <summary>
        /// [{
        ///  "timestamp":1444266681,
        ///  "tid":11988919,
        ///  "price":"244.8",
        ///  "amount":"0.03297384",
        ///  "exchange":"bitfinex",
        ///  "type":"sell"
        ///}]
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public dynamic GetTrades(string symbol)
        {
            Url += "/trades/" + symbol;
            return GetData();
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
            return GetData();
        }

        /// <summary>
        /// [{
        ///  "maker_fees":"0.1",
        ///  "taker_fees":"0.2",
        ///  "fees":[{
        ///    "pairs":"BTC",
        ///    "maker_fees":"0.1",
        ///    "taker_fees":"0.2"
        ///   },{
        ///    "pairs":"LTC",
        ///    "maker_fees":"0.1",
        ///    "taker_fees":"0.2"
        ///   },
        ///   {
        ///    "pairs":"ETH",
        ///    "maker_fees":"0.1",
        ///    "taker_fees":"0.2"
        ///  }]
        ///}]
        /// </summary>
        /// <returns></returns>
        public dynamic AccountInfo()
        {
            Url += "/account_infos";
            req += "/account_infos";
            return PostData(new
            {
                request = req,
                nonce = Nonce
            });
        }


        //TOOO:
        //_____Implement "Account Fees"
        //_____Implement "Summary" 



        /// <summary>
        /// return the address of a wallet
        /// 
        /// {
        ///  "result":"success",
        ///  "method":"bitcoin",
        ///  "currency":"BTC",
        ///  "address":"1A2wyHKJ4KWEoahDHVxwQy3kdd6g1qiSYV"
        ///}
        /// </summary>
        /// <param name="method">wallet type to return (i.e. "bitcoin" or "litecoin")</param>
        /// <param name="name">type of wallet (i.e. "trading", "exchange", "deposit")</param>
        /// <param name="renew">if true will return a new wallet address and RENEW YOUR WALLET (CAUTION)</param>
        /// <returns></returns>
        public dynamic Deposit(string wallet, string name, bool renew = false)
        {
            Url += "/deposit/new";
            req += "/deposit/new";
            return PostData(new
            {
                request = req,
                nonce = Nonce,
                method = wallet,
                wallet_name = name,
                renew = renew ? 1 : 0
            });
        }



        //TODO:
        //_____Implement "Key Permissions"
        //_____Implement "Margin Information"



        /// <summary>
        /// [{
        ///  "type":"deposit",
        ///  "currency":"btc",
        ///  "amount":"0.0",
        ///  "available":"0.0"
        ///},{
        ///  "type":"deposit",
        ///  "currency":"usd",
        ///  "amount":"1.0",
        ///  "available":"1.0"
        ///},{
        ///  "type":"exchange",
        ///  "currency":"btc",
        ///  "amount":"1",
        ///  "available":"1"
        ///},{
        ///  "type":"exchange",
        ///  "currency":"usd",
        ///  "amount":"1",
        ///  "available":"1"
        ///},{
        ///  "type":"trading",
        ///  "currency":"btc",
        ///  "amount":"1",
        ///  "available":"1"
        ///},{
        ///  "type":"trading",
        ///  "currency":"usd",
        ///  "amount":"1",
        ///  "available":"1"
        ///},
        ///...]
        /// </summary>
        /// <returns></returns>
        public dynamic WalletBalances()
        {
            Url += "/balances";
            req += "/balances";
            return PostData(new
            {
                request = req,
                nonce = Nonce
            });
        }

        /// <summary>
        /// transfer from one wallet to another i.e. from "exchange" to "deposit" wallet
        /// [{
        ///  "status":"success",
        ///  "message":"1.0 USD transfered from Exchange to Deposit"
        ///}]
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="coin"></param>
        /// <param name="typefrom"></param>
        /// <param name="typeto"></param>
        /// <returns></returns>
        public dynamic Transfer(string amt, string coin, string typefrom, string typeto)
        {
            Url += "/transfer";
            req += "/transfer";
            return PostData(new
            {
                request = req,
                nonce = Nonce,
                amount = amt,
                currency = coin,
                walletfrom = typefrom,
                walletto = typeto
            });
        }

        /// <summary>
        /// [{
        ///  "status":"success",
        ///  "message":"Your withdrawal request has been successfully submitted.",
        ///  "withdrawal_id":586829
        ///}]
        /// </summary>
        /// <param name="coin"></param>
        /// <param name="walletType"></param>
        /// <param name="amount"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public dynamic Withdraw(string method, string walletType, string amt, string addr)
        {
            Url += "/withdraw";
            req += "/withdraw";
            return PostData(new
            {
                request = req,
                nonce = Nonce,
                withdraw_type = method,
                walletselected = walletType,
                amount = amt,
                address = addr
            });
        }

        /// <summary>
        /// Creates an authenticated post request with a new order for the account with the given keys
        /// </summary>
        /// <param name="Symbol">market eg "BTCLTC"</param>
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
        public dynamic NewOrder(string Symbol, string Amount, string Price, string Side, string Type, float Buy_Price_Oco = 0, float Sell_Price_Oco = 0, string Exchange = "bitfinex", bool Is_Hidden = false, bool Is_Postonly = false, int Use_All_Available = 0, bool Oco_Order = false)
        {
            Url += "/order/new";
            req += "/order/new";
            return PostData(new
            {
                request = req,
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
            });
        }



        //TODO:
        //Implement "Multiple New Orders"


        /// <summary>
        /// cancel an order
        /// 
        /// {
        ///  "id":446915287,
        ///  "symbol":"btcusd",
        ///  "exchange":null,
        ///  "price":"239.0",
        ///  "avg_execution_price":"0.0",
        ///  "side":"sell",
        ///  "type":"trailing stop",
        ///  "timestamp":"1444141982.0",
        ///  "is_live":true,
        ///  "is_cancelled":false,
        ///  "is_hidden":false,
        ///  "was_forced":false,
        ///  "original_amount":"1.0",
        ///  "remaining_amount":"1.0",
        ///  "executed_amount":"0.0"
        ///}
        /// </summary>
        /// <param name="orderId">id returned when created</param>
        /// <returns></returns>
        public dynamic CancelOrder(int orderId)
        {
            Url += "/order/cancel";
            req += "/order/cancel";
            return PostData(new
            {
                request = req,
                nonce = Nonce,
                order_id = orderId
            });
        }

        public dynamic CancelOrders(int[] ids)
        {
            Url += "/order/cancel/multi";
            req += "/order/cancel/multi";
            return PostData(new
            {
                request = req,
                nonce = Nonce,
                order_ids = ids
            });
        }

        public dynamic CancelAllOrders()
        {
            Url += "/order/cancel/all";
            req += "/order/cancel/all";
            return PostData(new
            {
                request = req,
                nonce = Nonce
            });
        }



        //TODO:
        //Implement "Replace Order"



        /// <summary>
        /// {
        ///  "id":448411153,
        ///  "symbol":"btcusd",
        ///  "exchange":null,
        ///  "price":"0.01",
        ///  "avg_execution_price":"0.0",
        ///  "side":"buy",
        ///  "type":"exchange limit",
        ///  "timestamp":"1444276570.0",
        ///  "is_live":false,
        ///  "is_cancelled":true,
        ///  "is_hidden":false,
        ///  "oco_order":null,
        ///  "was_forced":false,
        ///  "original_amount":"0.01",
        ///  "remaining_amount":"0.01",
        ///  "executed_amount":"0.0"
        ///}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic OrderStatus(int id)
        {
            Url += "/order/status";
            req += "/order/status";
            return PostData(new
            {
                request = req,
                nonce = Nonce,
                order_id = id
            });
        }

        /// <summary>
        /// [{
        ///  "id":448411365,
        ///  "symbol":"btcusd",
        ///  "exchange":"bitfinex",
        ///  "price":"0.02",
        ///  "avg_execution_price":"0.0",
        ///  "side":"buy",
        ///  "type":"exchange limit",
        ///  "timestamp":"1444276597.0",
        ///  "is_live":true,
        ///  "is_cancelled":false,
        ///  "is_hidden":false,
        ///  "was_forced":false,
        ///  "original_amount":"0.02",
        ///  "remaining_amount":"0.02",
        ///  "executed_amount":"0.0"
        ///}]
        /// </summary>
        /// <returns></returns>
        public dynamic ActiveOrders()
        {
            Url += "/orders";
            req += "/orders";
            return PostData(new
            {
                request = req,
                nonce = Nonce
            });
        }

        public dynamic PastTrades(string marketpair, DateTime dateAfter, int limit = 100, bool reverseOrder = false)
        {
            Url += "/mytrades";
            req += "/mytrades";
            return PostData(new
            {
                request = req,
                nonce = Nonce,
                symbol = marketpair,
                timestamp = UnixTimeStamp(dateAfter),
                limt_trades = limit,
                reverse = reverseOrder ? 1 : 0
            });
        }


        //Several margin trading methods left out


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
                .Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s); 
        }

        /// <summary>
        /// makes an api call, returns JSON payload
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        protected override dynamic GetData()
        {
            try
            {
                WebResponse response = ((HttpWebRequest)WebRequest.Create(Url)).GetResponse();
                string raw = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                return JsonConvert.DeserializeObject(raw);
            }
            catch (WebException wex)
            {
                StreamReader sr = new StreamReader(((HttpWebResponse)wex.Response).GetResponseStream());
                Logger.ERROR("Failed to access " + Url + "\n" + sr.ReadToEnd());
                return null;
            }
        }

        protected HttpWebRequest CreateRequest(object payload)
        {
            payload = JsonConvert.SerializeObject(payload);
            var request = ((HttpWebRequest)WebRequest.Create(Url));
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Headers.Add("X-BFX-APIKEY", KeyLoader.BitfinexKeys.Item1);
            request.Headers.Add("X-BFX-PAYLOAD", EncodeBase64((string)payload));
            request.Headers.Add("X-BFX-SIGNATURE", GenerateSignature((string)payload));
            return request;
        }

        /// <summary>
        /// makes an api call with a post and returns the payload
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected override dynamic PostData(object payload)
        {
            var request = CreateRequest(payload);

            try
            {
                payload = JsonConvert.SerializeObject(payload) as string;
                new StreamWriter(request.GetRequestStream()).Write(payload);
                WebResponse response = request.GetResponse();
                string raw = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                return JsonConvert.DeserializeObject(raw);
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
    }
}
