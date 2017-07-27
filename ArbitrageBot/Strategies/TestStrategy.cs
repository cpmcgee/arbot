using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Poloniex;
using ArbitrageBot.Util;
using ArbitrageBot.APIs;

namespace ArbitrageBot.Strategies
{
    public class TestStrategy : IStrategy
    {
        public void Run()
        {
            Bittrex bittrex = new Bittrex();
            Bitfinex bitfinex = new Bitfinex();
            Poloniex poloniex = new Poloniex();
            decimal diffsum = 0;

            dynamic data = new BittrexRequest().Account().GetOrderHistory();
            dynamic data2 = new BitfinexRequest().WalletBallances();
            //Logger.INFO("Price Differences: ");
            //foreach (var s in coins)
            //{
            //    try
            //    {
            //        decimal diff = poloniex.GetPriceInBtc(s) - bittrex.GetPriceInBtc(s);
            //        diffSum += Math.Abs(diff);
            //        Logger.INFO(s + "--" + (diff > 0 ? "Bittrex" : "Poloniex") + " lower by: " + diff);
            //    }
            //    catch (ArgumentException ex)
            //    {
            //        Logger.ERROR(ex.Message);
            //    }
            //    //Console.ReadLine();
            //}
            //Logger.INFO("Total arbitrage opportunity: " + diffSum);
            Console.ReadLine();
        }

        List<string> coins = new List<string>
        {
            "ABY",
            "AEON",
            "AMP",
            "ARDR",
            "AUR",
            "BCY",
            "BLK",
            "BLOCK",
            "BTCD",
            "BTS",
            "BURST",
            "CLAM",
            "CURE",
            "DASH",
            "DCR",
            "DGB",
            "DOGE",
            "EFL",
            "EMC2",
            "ETC",
            "ETH",
            "EXP",
            "FCT",
            "FLDC",
            "FLO",
            "GAME",
            "GEO",
            "GNO",
            "GNT",
            "GRC",
            "GRS",
            "IOC",
            "LBC",
            "LSK",
            "LTC",
            "MAID",
            "MYR",
            "NAUT",
            "NAV",
            "NBT",
            "NEOS",
            "NXC",
            "NXT",
            "OMNI",
            "PINK",
            "POT",
            "PPC",
            "QTL",
            "RADS",
            "RBY",
            "RDD",
            "REP",
            "SBD",
            "SC",
            "SJCX",
            "SLR",
            "STEEM",
            "STRAT",
            "SYS",
            "TRUST",
            "VIA",
            "VOX",
            "VRC",
            "VTC",
            "XCP",
            "XDN",
            "XEM",
            "XMG",
            "XMR",
            "XRP",
            "XST",
            "XVC",
            "ZEC"
        };
    }
}
