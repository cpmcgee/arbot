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

            Bittrex bittrex;
            Bitfinex bitfinex;
            Poloniex poloniex;

            Task.WhenAll(
                Task.Run(() => bittrex = new Bittrex()),
                Task.Run(() => bitfinex = new Bitfinex()),
                Task.Run(() => poloniex = new Poloniex())).Wait(); //Asynchronously build exchange objects

            Logger.INFO("Press Enter For Price Differences: ");
            while (true)
            {
                decimal? maxDiff = 0;
                string maxCurrency = "";
                decimal? totalDiff = 0;
                Console.ReadLine();
                foreach (var coin in CurrencyManager.GetCurrencies())
                {
                    decimal? max = null;
                    decimal? min = null;
                    decimal? diff = null;

                    PrintStats(coin, out max, out min, out diff);

                    if (max != null && min != null)
                    {
                        diff = Math.Abs((decimal)max - (decimal)min);
                        totalDiff += diff;
                        if (diff > maxDiff)
                        {
                            maxDiff = diff;
                            maxCurrency = coin.Key;
                        }
                    }

                    Logger.WRITE("  Max: " + max);
                    Logger.WRITE("  Min: " + min);
                    Logger.WRITE("  Diff: " + diff);
                    Logger.BREAK();
                }


                Logger.BREAK();
                Logger.BREAK();

                Logger.WRITE("Total arbitrate opportunity: " + totalDiff);
                Logger.WRITE("Max difference: " + maxDiff + " for " + maxCurrency);
                PrintStats(maxCurrency);
            }
        }

        private decimal? Max(decimal?[] prices)
        {
            return prices.Max();
        }

        private decimal? Min(decimal?[] prices)
        {
            return prices.Min();
        }

        private void PrintStats(KeyValuePair<string, Currency> coin, out decimal? max, out decimal? min, out decimal? diff)
        {
            Logger.WRITE("--Getting Prices for " + coin.Value.Symbol + "--");
            decimal? btxPrice = null;
            decimal? bfxPrice = null;
            decimal? plxPrice = null;
            max = null;
            min = null;
            diff = null;

            btxPrice = coin.Value.BittrexLast;
            Logger.WRITE("Bittrex: " + btxPrice);
            bfxPrice = coin.Value.BitfinexLast;
            Logger.WRITE("Bitfinex: " + bfxPrice);
            plxPrice = coin.Value.PoloniexLast;
            Logger.WRITE("Poloniex: " + plxPrice);

            max = Max(new decimal?[] { btxPrice, bfxPrice, plxPrice });
            min = Min(new decimal?[] { btxPrice, bfxPrice, plxPrice });
        }

        private void PrintStats(string symbol)
        {
            var coin = CurrencyManager.GetCurrency(symbol);
            Logger.WRITE("--Getting Prices for " + coin.Symbol + "--");
            Logger.WRITE("Bittrex: " + coin.BittrexLast);
            Logger.WRITE("Bitfinex: " + coin.BitfinexLast);
            Logger.WRITE("Poloniex: " + coin.PoloniexLast);
        }
    }
}
