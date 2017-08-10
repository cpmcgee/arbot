using System;
using System.Collections.Generic;
using ArbitrageBot.CurrencyUtil;

namespace ArbitrageBot.APIs.Poloniex
{
    public static class Poloniex
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
            try
            {
                var data = new PoloniexRequest().Trading().CancelOrder(Convert.ToInt32(order.Id));
                if (data.success == 1)
                    return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            return false;
        }

        private static void GetCoins()
        {
            Currencies.Clear();
            var data = new PoloniexRequest().Public().ReturnTicker();
            foreach (var obj in data)
            {
                string[] pair = ((string)obj.Name).Split('_');
                string baseCurrency = pair[0];
                string symbol = pair[1].ToUpper();
                if (baseCurrency == "BTC")
                {
                    Currency coin = CurrencyManager.GetCurrency(symbol.ToUpper());
                    if (coin == null)
                    {
                        coin = new Currency(symbol.ToUpper());
                        CurrencyManager.AddCurrency(coin.Symbol.ToUpper(), coin);
                    }
                    coin.PoloniexBtcPair = obj.Name;
                    coin.PoloniexBid = obj.Value.highestBid;
                    coin.PoloniexAsk = obj.Value.lowestAsk;
                    coin.PoloniexLast = obj.Value.last;
                    coin.PoloniexVolume = obj.Value.quoteVolume;
                    Currencies.Add(coin);
                }
            }
        }

        public static void UpdatePrices()
        {
            var data = new PoloniexRequest().Public().ReturnTicker();
            foreach (var obj in data)
            {
                string[] pair = ((string)obj.Name).Split('_');
                string baseCurrency = pair[0];
                string symbol = pair[1].ToUpper();
                if (baseCurrency == "BTC")
                {
                    Currency coin = CurrencyManager.GetCurrency(symbol);
                    coin.PoloniexBid = obj.Value.highestBid;
                    coin.PoloniexAsk = obj.Value.lowestAsk;
                    coin.PoloniexLast = obj.Value.last;
                    coin.PoloniexVolume = obj.Value.quoteVolum;
                }
            }
        }
    }
}
