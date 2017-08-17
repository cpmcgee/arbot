using System;
using System.Collections.Generic;
using ArbitrageBot.CurrencyUtil;

namespace ArbitrageBot.APIs.Poloniex
{
    public static class Poloniex
    {
        public static List<Currency> Currencies
        {
            get
            {
                return CurrencyManager.PoloniexCurrencies;
            }
        }

        public static List<Order> Orders
        {
            get
            {
                return OrderManager.BittrexOrders;
            }
        }

        public static PoloniexOrder Buy(string currency, double quantity, double price)
        {
            string pair = "BTC_" + currency.ToUpper();
            try
            {
                var data = new PoloniexRequest().Trading().Buy(pair, price, quantity);
                var newOrder = new PoloniexOrder((string)data.orderNumber, currency, OrderType.BUY, quantity);
                Orders.Add(newOrder);
                OrderManager.AddOrder(newOrder);
                return newOrder;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PoloniexOrder Sell(string currency, double quantity, double price)
        {
            string pair = "BTC_" + currency.ToUpper();
            try
            {
                var data = new PoloniexRequest().Trading().Sell(pair, price, quantity);
                var order = new PoloniexOrder((string)data.orderNumber, currency, OrderType.SELL, quantity);
                Orders.Add(order);
                OrderManager.AddOrder(order);
                return order;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool CancelOrder(Order order)
        {
            return order.Cancel();
        }

        public static double GetBalance(string symbol)
        {
            return CurrencyManager.GetCurrency(symbol).PoloniexBalance;
        }
    }
}
