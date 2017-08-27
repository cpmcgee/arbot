using System;
using System.Linq;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;
using System.Threading.Tasks;
using ArbitrageBot.APIs.Poloniex;
using ArbitrageBot.CurrencyUtil;
using ArbitrageBot.Util;
using System.Collections.Generic;

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
            //Asynchronously build exchange objects

            Logger.WRITE("Press Enter For Price Differences: ", LogLevel.All);
            while (true)
            {
                double? maxDiff = 0;
                string maxCurrency = "";
                double? totalDiff = 0;
                Console.ReadLine();
                foreach (var coin in CurrencyManager.GetCurrencies())
                {
                    double? max = null;
                    double? min = null;
                    double? diff = null;

                    PrintStats(coin, out max, out min, out diff);

                    if (max != null && min != null)
                    {
                        diff = Math.Abs((double)max - (double)min);
                        totalDiff += diff;
                        if (diff > maxDiff)
                        {
                            maxDiff = diff;
                            maxCurrency = coin.Key;
                        }
                    }

                    Logger.WRITE("  Max: " + max, LogLevel.All);
                    Logger.WRITE("  Min: " + min, LogLevel.All);
                    Logger.WRITE("  Diff: " + diff, LogLevel.All);
                    Logger.BREAK();
                }

                Logger.BREAK();
                Logger.BREAK();

                Logger.WRITE("Total arbitrate opportunity: " + totalDiff, LogLevel.All);
                Logger.WRITE("Max difference: " + maxDiff + " for " + maxCurrency, LogLevel.All);
                PrintStats(maxCurrency);
            }
        }

        private double? Max(double?[] prices)
        {
            return prices.Max();
        }

        private double? Min(double?[] prices)
        {
            return prices.Min();
        }

        private void PrintStats(KeyValuePair<string, Currency> coin, out double? max, out double? min, out double? diff)
        {
            Logger.WRITE("--Getting Prices for " + coin.Value.Symbol + "--", LogLevel.All);
            double? btxPrice = null;
            double? bfxPrice = null;
            double? plxPrice = null;
            max = null;
            min = null;
            diff = null;

            btxPrice = coin.Value.BittrexLast;
            Logger.WRITE("Bittrex: " + btxPrice, LogLevel.All);
            bfxPrice = coin.Value.BitfinexLast;
            Logger.WRITE("Bitfinex: " + bfxPrice, LogLevel.All);
            plxPrice = coin.Value.PoloniexLast;
            Logger.WRITE("Poloniex: " + plxPrice, LogLevel.All);

            max = Max(new double?[] { btxPrice, bfxPrice, plxPrice });
            min = Min(new double?[] { btxPrice, bfxPrice, plxPrice });
        }

        private void PrintStats(string symbol)
        {
            var coin = CurrencyManager.GetCurrency(symbol);
            Logger.WRITE("--Getting Prices for " + coin.Symbol + "--", LogLevel.All);
            Logger.WRITE("Bittrex: " + coin.BittrexLast, LogLevel.All);
            Logger.WRITE("Bitfinex: " + coin.BitfinexLast, LogLevel.All);
            Logger.WRITE("Poloniex: " + coin.PoloniexLast, LogLevel.All);
        }
    }
}
