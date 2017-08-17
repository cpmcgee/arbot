using System;
using System.Collections.Generic;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Poloniex;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ArbitrageBot.APIs
{
    class OrderManager
    {
        private static ConcurrentDictionary<string, Order> Orders = new ConcurrentDictionary<string, Order>();

        //Lists that contain the orders held by each exchange
        public static List<Order> BittrexOrders = new List<Order>();
        public static List<Order> BitfinexOrders = new List<Order>();
        public static List<Order> PoloniexOrders = new List<Order>();

        public static bool AddOrder(Order order)
        {
            return Orders.TryAdd(order.Id, order);
        }

        /// <summary>
        /// returns the value in the dictionary
        /// returns null if not found
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static Order GetOrder(string orderId)
        {
            Order order = null;
            Orders.TryGetValue(orderId, out order);
            return order;
        }

        public static bool HasOrder(string orderId)
        {
            return Orders.ContainsKey(orderId);
        }

        public static ConcurrentDictionary<string, Order> GetOrders()
        {
            return Orders;
        }

        static bool run = false;

        private static void StartAsyncOrderChecking()
        {
            run = true;
            while (run)
            {
                Task.WhenAll(
                    Task.Run(() => CheckBittrexOrders()),
                    Task.Run(() => CheckBitfinexOrders()),
                    Task.Run(() => CheckPoloniexOrders())).Wait();
            }
        }

        public static void StopAsyncOrderChecking()
        {
            run = false;
        }

        private static void CheckBitfinexOrders()
        {
            var data = new Bitfinex.BitfinexRequest().ActiveOrders();
            List<Order> openOrders = new List<Order>();
            foreach (var obj in data)
            {
                openOrders.Add(OrderManager.GetOrder(obj.id));
            }
            foreach (Order order in BitfinexOrders)
            {
                if (!openOrders.Contains(order))
                    if (order.IsOpen)
                        order.Fulfill();
            }
        }

        private static void CheckBittrexOrders()
        {
            var data = new BittrexRequest().Market().GetOpenOrders().result;
            List<Order> openOrders = new List<Order>();
            foreach (var obj in data)
            {
                openOrders.Add(OrderManager.GetOrder(obj.Uuid));
            }
            foreach (Order order in BittrexOrders)
            {
                if (!openOrders.Contains(order))
                    if (order.IsOpen)
                        order.Fulfill();
            }
        }

        private static void CheckPoloniexOrders()
        {
            var data = new PoloniexRequest().Trading().ReturnOpenOrders();
            List<Order> openOrders = new List<Order>();
            foreach (var obj in data)
            {
                foreach (var order in obj.Value)
                {
                    openOrders.Add(OrderManager.GetOrder((string)order.orderNumber));
                }
            }
            foreach (Order order in PoloniexOrders)
            {
                if (!openOrders.Contains(order))
                    if (order.IsOpen)
                        order.Fulfill();
            }
        }
    }
}
