using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitrageBot.APIs;

namespace ArbitrageBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bittrex bittrex = new Bittrex();
            Bitfinex bitfinex = new Bitfinex();
            for (int i = 0; i == 0; i++)
            {
                //Console.WriteLine("Input Ticker to get difference");
                //string ticker = Console.ReadLine();

                //Console.WriteLine("Price Difference:" + (bitfinex.GetPriceInBtc(ticker) - bittrex.GetPriceInBtc(ticker)));
                //Console.WriteLine(Bitfinex.GetPriceInBtc(ticker));
                //Console.WriteLine(Bittrex.GetPriceInBtc(ticker));

                Console.WriteLine("Price Differences:\n");
                foreach (string symbol in bitfinex.GetSymbols())
                {
                    try
                    {
                        Console.WriteLine(symbol + ":" + (bitfinex.GetPriceInBtc(symbol) - bittrex.GetPriceInBtc(symbol)));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Couldnt compare price for: " + symbol);
                    }
                }
                Console.ReadLine();
            }
        }
    }
}
