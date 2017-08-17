using System;
using System.Collections.Generic;
using ArbitrageBot.CurrencyUtil;

namespace ArbitrageBot.APIs.Bittrex
{
    public static class Bittrex
    {
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
                BittrexOrder newOrder = new BittrexOrder(data.result.uuid, currency, OrderType.BUY, quantity);
                OrderManager.AddOrder(newOrder);
                return newOrder;
            }
        }

        public static Order Sell(string currency, double quantity, double price)
        {
            string market = "BTC-" + currency.ToUpper();
            var data = new BittrexRequest().Market().SellLimit(market, quantity, price);
            if (data.success == false)
                return null;
            else
            {
                BittrexOrder newOrder = new BittrexOrder(data.result.uuid, currency, OrderType.SELL, quantity);
                OrderManager.AddOrder(newOrder);
                Orders.Add(newOrder);
                return newOrder;
            }
        }

        public static bool CancelOrder(BittrexOrder order)
        {
            return order.Cancel();
        }

        public static double GetBalance(string symbol)
        {
            return CurrencyManager.GetCurrency(symbol).BittrexBalance;
        }
    }
}
