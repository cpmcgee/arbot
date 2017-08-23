using System;
using ArbitrageBot.APIs.Poloniex;

namespace ArbitrageBot.APIs
{
    public class PoloniexOrder : Order
    {
        public PoloniexOrder(string id, string currency, string type, double amt) : base(id, currency, type, amt) { }

        public override bool Cancel()
        {
            var data = new PoloniexRequest().Trading().CancelOrder(Convert.ToInt32(Id));
            if (data.success == 1)
            {
                this.IsCancelled = true;
                this.IsOpen = false;
                this.TimeCancelled = DateTime.Now;
                return true;
            }
            return false;
        }
    }
}
