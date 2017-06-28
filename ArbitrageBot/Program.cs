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
            while (true)
            {
                Console.WriteLine("Input Ticker to get difference");
                string ticker = Console.ReadLine();
                try
                {
                    Console.WriteLine("Price Difference:" + (Bitfinex.GetPriceInBtc(ticker) - Bittrex.GetPriceInBtc(ticker)));
                    //Console.WriteLine(Bitfinex.GetPriceInBtc(ticker));
                    //Console.WriteLine(Bittrex.GetPriceInBtc(ticker));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
