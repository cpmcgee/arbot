using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageBot.APIs
{
    /// <summary>
    /// A Buy or Sell order placed against BTC
    /// Currently only supporting trading altcoins against BTC
    /// </summary>
    public abstract class Order
    {
        public string Id { get; }
        public string Currency { get; }
        public DateTime TimePlaced { get; }
        public DateTime TimeFulfilled { get; protected set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public bool IsOpen { get; private set; }
        public bool IsCancelled { get; protected set; }

        public Order(string id, string currency, string type, double amt)
        {
            this.Id = id;
            this.Currency = currency;
            this.TimePlaced = DateTime.Now;
            this.Type = type;
            this.Amount = amt;
            this.IsOpen = true;
            this.IsCancelled = false;
        }

        public abstract bool Cancel();

        public virtual void Fulfill()
        {
            this.TimeFulfilled = DateTime.Now;
            this.IsOpen = false;
        }

        public override bool Equals(object obj)
        {
            if (obj is Order)
            {
                if (((Order)obj).Id == this.Id &&
                    ((Order)obj).Amount == this.Amount &&
                    ((Order)obj).Currency == this.Currency)
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Id.GetHashCode();
                hash = (hash * 16777619) ^ TimePlaced.GetHashCode();
                return hash;
            }
        }
    }
}
