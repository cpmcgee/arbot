//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ArbitrageBot.CurrencyUtil;
//
//namespace ArbitrageBot.APIs
//{
//    public abstract class API
//    {
//        //api keys
//        protected string Key { get; set; }
//        protected string Secret { get; set; }
//
//        public abstract Order Sell(string currency, double quantity, double price);
//
//        public abstract Order Buy(string currency, double quantity, double price);
//
//        public abstract bool CancelOrder(Order order);
//
//        public abstract List<Order> GetOrders();
//    }
//}
