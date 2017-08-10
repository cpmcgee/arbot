using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageBot.APIs
{
    public class PoloniexOrder : Order
    {
        public PoloniexOrder(string id, string currency, string type, double amt) : base(id, currency, type, amt) { }

        public override bool Cancel()
        {
            return Poloniex.Poloniex.CancelOrder(this);
        }
    }
}
