using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;

namespace ArbitrageBot.Strategies
{
    public class TestStrategy : IStrategy
    {
        public void Run()
        {
            Bittrex bittrex = new Bittrex();
            Bitfinex bitfinex = new Bitfinex();

            Console.WriteLine("Price Differences: \n");
            foreach (var s in bitfinex.GetSymbols())
            {
                try
                {
                    Console.WriteLine(s + ": " + (bittrex.GetPriceInBtc(s) - bitfinex.GetPriceInBtc(s)));
                }
                catch (Exception ex)
                {
               
                }

            }
            Console.ReadLine();
        }
    }
}
