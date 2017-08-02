﻿using System;
using System.Collections.Generic;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Poloniex;
using ArbitrageBot.CurrencyUtil;
using ArbitrageBot.Util;

namespace ArbitrageBot.Strategies
{
    /// <summary>
    /// A very simple example strategy:
    /// Trivial comparison of coin prices on Poloniex vs. Bittrex
    /// </summary>
    public class PriceCompare : IStrategy
    {
        public void Run()
        {
            Bittrex bittrex = new Bittrex();
            Bitfinex bitfinex = new Bitfinex();
            Poloniex poloniex = new Poloniex();

            Logger.INFO("Press Enter For Price Differences: ");
            while (true)
            {
                Console.ReadLine();
                foreach (var coin in CurrencyManager.Currencies)
                {
                    Logger.WRITE("--Analyzing " + coin.Value.Symbol + "--");
                    decimal? btxPrice;
                    decimal? bfxPrice;
                    decimal? plxPrice;

                    btxPrice = coin.Value.BittrexLast;
                    Logger.WRITE("Bittrex: " + btxPrice);
                    bfxPrice = coin.Value.BitfinexLast;
                    Logger.WRITE("Bitfinex: " + bfxPrice);
                    plxPrice = coin.Value.PoloniexLast;
                    Logger.WRITE("Poloniex: " + plxPrice);

                    Logger.BREAK();
                }
            }
        }
    }
}