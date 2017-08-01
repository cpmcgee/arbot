using System;
using System.Collections.Generic;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Poloniex;
using System.ComponentModel;

namespace ArbitrageBot.CurrencyUtil
{
    static class CurrencyManager
    {
        public static List<Currency> Currencies = new List<Currency>();

        public static List<Currency> BittrexCurrencies = new List<Currency>();
        public static List<Currency> BitfinexCurrencies = new List<Currency>();
        public static List<Currency> PoloniexCurrencies = new List<Currency>();

        public static void Initialize()
        {
            foreach (string c in SupportedSymbols)
            {
                Currency coin = new Currency(c);
                Currencies.Add(coin);
            }
            Currency.PriceUpdater.LoadPrices();
            Currency.PriceUpdater.Start();
        }

        public static IReadOnlyCollection<string> SupportedSymbols = new List<string>
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
            "IOTA",
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

