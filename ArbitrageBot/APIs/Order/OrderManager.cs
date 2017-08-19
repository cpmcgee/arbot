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
        internal static List<Order> BittrexOrders { get; private set; } = new List<Order>();
        internal static List<Order> BitfinexOrders { get; private set; } = new List<Order>();
        internal static List<Order> PoloniexOrders { get; private set; } = new List<Order>();

        internal static void AddOrder(BittrexOrder order)
        {
            Orders.TryAdd(order.Id, order);
            BittrexOrders.Add(order);
        }

        internal static void AddOrder(BitfinexOrder order)
        {
            Orders.TryAdd(order.Id, order);
            BitfinexOrders.Add(order);
        }
        
        internal static void AddOrder(PoloniexOrder order)
        {
            Orders.TryAdd(order.Id, order);
            PoloniexOrders.Add(order);
        }

        /// <summary>
        /// returns the value in the dictionary
        /// returns null if not found
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal static Order GetOrder(string orderId)
        {
            Order order = null;
            Orders.TryGetValue(orderId, out order);
            return order;
        }

        internal static bool HasOrder(string orderId)
        {
            return Orders.ContainsKey(orderId);
        }

        internal static ConcurrentDictionary<string, Order> GetOrders()
        {
            return Orders;
        }

        static bool run = false;
        internal static void StopAsyncOrderChecking() { run = false; }
        internal static void StartAsyncOrderChecking()
        {
            run = true;
            Task.Run(() => CheckOrdersLoop());
        }
        private static void CheckOrdersLoop()
        {
            while (run)
            {
                CheckOrders();
            }
        }

        internal static void CheckOrders()
        {
            Task.WhenAll(
                        Task.Run(() => CheckBittrexOrders()),
                        Task.Run(() => CheckBitfinexOrders()),
                        Task.Run(() => CheckPoloniexOrders())).Wait();
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
