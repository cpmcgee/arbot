using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitrageBot.Strategies;

namespace ArbitrageBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new TestStrategy().Run();
        }
    }
}
