using System;
using ArbitrageBot.CurrencyUtil;
using System.Collections.Generic;

namespace ArbitrageBot.APIs.Bitfinex
{
    public static class Bitfinex
    {
        public static List<Currency> Currencies
        {
            get
            {
                return CurrencyManager.BitfinexCurrencies;
            }
        }

        public static List<Order> Orders
        {
            get
            {
                return OrderManager.BitfinexOrders;
            }
        }

        public struct Method
        {
            public const string LITECOIN = "litecoin";
            public const string BITCOIN = "bitcoin";
            public const string ETHEREUM = "ethereum";
            public const string ETHEREUM_CLASSIC = "ethereumc";
            public const string MASTERCOIN = "mastercoin";
            public const string ZCASH = "zcash";
            public const string MONERO = "monero";
            public const string WIRE = "wire";
            public const string DASH = "dash";
            public const string RIPPLE = "ripple";
            public const string EOS = "eos";
        }

        public struct WalletType
        {
            public const string EXCHANGE = "exchange";
            public const string TRADING = "trading";
            public const string FUNDING = "funding";
        }

        

        public static Order Buy(string currency, double quantity, double price)
        {
            string market = "btc" + currency.ToLower();
            var data = new BitfinexRequest().NewOrder(currency, quantity, price, "buy", "limit");
            if (data.success = false)
                return null;
            else
            {
                BitfinexOrder newOrder = new BitfinexOrder(data.result.uuid, currency, OrderType.BUY, Convert.ToDouble(quantity));
                OrderManager.AddOrder(newOrder);
                Orders.Add(newOrder);
                return newOrder;
            }
        }

        public static Order Sell(string currency, double quantity, double price)
        {
            string market = "btc" + currency.ToLower();
            var data = new BitfinexRequest().NewOrder(currency, quantity, price, "sell", "limit");
            if (data.success = false)
                return null;
            else
            {
                BitfinexOrder newOrder = new BitfinexOrder(data.result.uuid, currency, OrderType.BUY, Convert.ToDouble(quantity));
                OrderManager.AddOrder(newOrder);
                Orders.Add(newOrder);
                return newOrder;
            }
        }

        public static bool CancelOrder(Order order)
        {
            return order.Cancel();
        }

        public static void CheckOrders()
        {
            var data = new BitfinexRequest().ActiveOrders();
            List<Order> openOrders = new List<Order>();
            foreach (var obj in data)
            {
                openOrders.Add(OrderManager.GetOrder(obj.id));
            }
            foreach (Order order in OrderManager.BitfinexOrders)
            {
                if (!openOrders.Contains(order))
                    if (order.IsOpen)
                        order.Fulfill();
            }
        }
    }
}
