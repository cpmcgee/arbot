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
            Config.ImportProperties("C:\\Users\\cmcgee\\Desktop\\arbot\\config.txt");
            Logger.Initialize();
            new TestStrategy().Run();
            Logger.Close();
        }
    }
}
