﻿using System;
using ArbitrageBot.Strategies;
using ArbitrageBot.Util;

namespace ArbitrageBot
{
    class Program
    {
        static void Main(string[] args)
        {
            //Config.ImportProperties(@"M:\Source\ArbitrageBot\config.txt");
            Config.ImportProperties(@"C:\Users\cmcgee\Desktop\arbot\config.txt");
            Logger.Initialize();
            new TestStrategy().Run();
            Logger.Close();
        }
    }
}
