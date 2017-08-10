using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageBot.APIs
{
    public abstract class OrderType
    {
        public static string BUY { get { return "BUY"; } }
        public static string SELL { get { return "SELL"; } }
    }
}
