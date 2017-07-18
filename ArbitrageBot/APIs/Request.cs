using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageBot.APIs
{
    public abstract class Request
    {
        protected string Url { get; set; }
        protected abstract dynamic GetData(string url);

        //protected 
    }
}
