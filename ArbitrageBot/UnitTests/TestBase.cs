using System;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Poloniex;
using ArbitrageBot.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitrageBot.Strategies;
using System.Net;

namespace ArbitrageBot.UnitTests
{
    [TestClass]
    public class TestBase
    {
        [AssemblyInitialize]
        public static void Setup(TestContext t)
        {
            Config.ImportProperties(@"M:\Source\ArbitrageBot\config.txt");
            //Config.ImportProperties(@"C:\Users\cmcgee\Desktop\arbot\config.txt");
            Logger.Initialize(5);
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            Logger.Close();
        }
    }
}
