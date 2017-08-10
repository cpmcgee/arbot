using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitrageBot.APIs.Bitfinex;

namespace ArbitrageBot.APIs
{
    public class BitfinexOrder : Order
    {
        public BitfinexOrder(string id, string currency, string type, double amt) : base(id, currency, type, amt) { }

        public override bool Cancel()
        {
            var data = new BitfinexRequest().CancelOrder(Convert.ToInt32(Id));
            if (data.success == false)
                return false;
            else
            {
                OrderManager.GetOrder(Id).Cancel();
                return true;
            }
        }
    }
}

