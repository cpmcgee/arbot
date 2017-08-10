using System;
using System.Collections.Generic;
using ArbitrageBot.CurrencyUtil;

namespace ArbitrageBot.APIs.Bittrex
{
    public static class Bittrex
    {
        public static void Intialize()
        {
            GetCoins();
            UpdatePrices();
        }

        public static List<Currency> Currencies
        {
            get
            {
                return CurrencyManager.BittrexCurrencies;
            }
        }

        public static List<Order> Orders
        {
            get
            {
                return OrderManager.BittrexOrders;
            }
        }

        public static Order Buy(string currency, double quantity, double price)
        {
            string market = "BTC-" + currency.ToUpper();
            var data = new BittrexRequest().Market().BuyLimit(market, quantity, price);
            if (data.success == false)
                return null;
            else
            {
                BittrexOrder newOrder = new BittrexOrder(data.result.uuid, currency, OrderType.BUY, (decimal)quantity);
                OrderManager.AddOrder(newOrder);
                return newOrder;
            }
        }

        public static  Order Sell(string currency, double quantity, double price)
        {
            string market = "BTC-" + currency.ToUpper();
            var data = new BittrexRequest().Market().SellLimit(market, quantity, price);
            if (data.success == false)
                return null;
            else
            {
                BittrexOrder newOrder = new BittrexOrder(data.result.uuid, currency, OrderType.SELL, (decimal)quantity);
                OrderManager.AddOrder(newOrder);
                return newOrder;
            }
        }

        public static bool CancelOrder(Order order)
        {
            var data = new BittrexRequest().Market().Cancel(order.Id);
            if (data.success == false)
                return false;
            else
                return true;
        }

        private static void GetCoins()
        {
            Currencies.Clear();
            dynamic data = new BittrexRequest().Public().GetCurrencies();
            var coins = data.result;
            foreach (var obj in coins)
            {
                string symbol = (string)obj.Currency;
                if (symbol == "BTC" || !((bool)obj.IsActive)) continue; //only add active coins traded against btc (dont add btc)
                Currency coin = CurrencyManager.GetCurrency(symbol);
                if (coin == null)
                {
                    coin = new Currency(symbol);
                    CurrencyManager.AddCurrency(coin.Symbol.ToUpper(), coin);
                }
                coin.BittrexName = obj.CurrencyLong;
                coin.BittrexBtcPair = ("BTC-" + symbol);
                Currencies.Add(coin);
            }
        }

        /// <summary>
        /// reloads the coins from the API, 
        /// is automatically handled
        /// </summary>
        public static void UpdatePrices()
        {
            var markets = new BittrexRequest().Public().GetMarketSummaries().result;
            foreach (var obj in markets)
            {
                string[] pair = ((string)obj.MarketName).Split('-');
                string baseCurrency = pair[0];
                string symbol = pair[1];
                if (baseCurrency.Equals("BTC"))
                {
                    Currency coin = CurrencyManager.GetCurrency(symbol.ToUpper());
                    if (coin != null)
                    {
                        coin.BittrexLast = obj.Last;
                        coin.BittrexAsk = obj.Ask;
                        coin.BittrexBid = obj.Bid;
                        coin.BittrexVolume = obj.Volume;
                    }
                }
            }
        }

        public static void CheckOrders()
        {
            var data = new BittrexRequest().Market().GetOpenOrders().result;
            List<Order> openOrders = new List<Order>();
            foreach (var obj in data)
            {
                openOrders.Add(OrderManager.GetOrder(obj.Uuid));
            }
            foreach (Order order in OrderManager.BittrexOrders)
            {
                if (!openOrders.Contains(order))
                    order.Fulfill();
            }
        }
    }
}
