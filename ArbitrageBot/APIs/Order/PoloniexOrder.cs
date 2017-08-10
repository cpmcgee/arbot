using System;
using ArbitrageBot.APIs.Poloniex;

namespace ArbitrageBot.APIs
{
    public class PoloniexOrder : Order
    {
        public PoloniexOrder(string id, string currency, string type, double amt) : base(id, currency, type, amt) { }

        public override bool Cancel()
        {
            try
            {
                var data = new PoloniexRequest().Trading().CancelOrder(Convert.ToInt32(Id));
                if (data.success == 1)
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
    }
}
