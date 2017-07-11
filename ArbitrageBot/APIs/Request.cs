using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageBot.APIs
{
    public abstract class Request
    {
        private string url;
        protected abstract dynamic GetData(string url);
    }
}
