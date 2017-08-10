using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitrageBot.APIs.Bittrex;

namespace ArbitrageBot.APIs
{ 
    public class BittrexOrder : Order
    {
        public BittrexOrder(string id, string currency, string type, double amt) : base(id, currency, type, amt) { }

        public override bool Cancel()
        {
            var data = new BittrexRequest().Market().Cancel(Id);
            if (data.success == false)
                return false;
            else
            {
                OrderManager.GetOrder(Id).Cancel();
                this.IsCancelled = true;
                return true;
            }
        }
    }
}
