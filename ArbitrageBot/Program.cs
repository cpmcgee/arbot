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
                Console.WriteLine("Input Ticker to get price");
                string ticker = Console.ReadLine();
                try
                {
                    Console.WriteLine("Price:" + Bittrex.GetPriceInBtc(ticker));
                }
                catch (Exception ex) { }
            }
        }
    }
}
