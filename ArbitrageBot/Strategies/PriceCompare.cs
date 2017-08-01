using System;
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

            Logger.INFO("Price Differences: ");

            foreach (var coin in CurrencyManager.Currencies)
            {
                Logger.WRITE("--Analyzing " + coin.Symbol + "--");
                decimal? btxPrice;
                decimal? bfxPrice;
                decimal? plxPrice;

                btxPrice = coin.BittrexPrice;
                Logger.WRITE("Bittrex: " + btxPrice);
                bfxPrice = coin.BitfinexPrice;
                Logger.WRITE("Bitfinex: " + bfxPrice);
                plxPrice = coin.PoloniexPrice;
                Logger.WRITE("Poloniex: " + plxPrice);

                Logger.BREAK();
            }
            Console.ReadLine();
        }
    }
}
