using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ArbitrageBot.APIs;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Poloniex;

namespace ArbitrageBot
{
    /// <summary>
    /// Separates key retrieval from the API classes, methods are designed so an object inheriting from API.cs can simply pass itself into this class's overloaded GetKeys method
    /// - This KeyLoader looks in the path at FILE_PATH and finds a text file with a name matching the exchange
    /// - It then loads the key and secret from the first two lines in the file (file contains two lines with keys, nothing else)
    /// </summary>
    static class KeyLoader
    {
        private const string FILE_PATH = "M:\\Source\\ArbitrageBot\\keys\\";

        /// <summary>
        /// GetKeys overloaded method takes an object conforming to API abstract class and sets its key attributes
        /// </summary>
        /// <param name="exchange"></param>
        public static void GetKeys(Bittrex exchange)
        {
            Tuple<string, string> keys = ReadFile(FILE_PATH + "bittrex.txt");
            exchange.SetKeys(keys.Item1, keys.Item2);
        }

        public static void GetKeys(Bitfinex exchange)
        {
            Tuple<string, string> keys = ReadFile(FILE_PATH + "bitfinex.txt");
            exchange.SetKeys(keys.Item1, keys.Item2);
        }

        public static void GetKeys(Poloniex exchange)
        {
            Tuple<string, string> keys = ReadFile(FILE_PATH + "poloniex.txt");
            exchange.SetKeys(keys.Item1, keys.Item2);
        }

        /// <summary>
        /// returns first two lines of file in a tuple file should be in format:
        /// --
        /// key
        /// secret
        /// --
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Tuple<string, string> ReadFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return new Tuple<string, string>(sr.ReadLine(), sr.ReadLine());
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Problem getting key from " + path);
                return new Tuple<string, string>("", "");
            }
        }
    }
}
