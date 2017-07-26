using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitrageBot.Strategies;
using ArbitrageBot.Util;

namespace ArbitrageBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.ImportProperties(@"M:\Source\ArbitrageBot\config.txt");
            Logger.Initialize();
            new TestStrategy().Run();
            Logger.Close();
        }
    }
}
