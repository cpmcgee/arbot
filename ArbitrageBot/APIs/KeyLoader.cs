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
        private const string BITTREX_FILE = "bittrex.txt";
        private const string BITFINEX_FILE = "bitfinex.txt";
        private const string POLONIEX_FILE = "poloniex.txt";
        
        private static Tuple<string, string> bittrexKeys;
        private static Tuple<string, string> bitfinexKeys;
        private static Tuple<string, string> poloniexKeys;

        /// <summary>
        /// gives the public and secret key as item1 and item2 in a tuple
        /// </summary>
        /// <param name="exchange"></param>
        public static Tuple<string, string> BittrexKeys
        {
            get
            {
                if (bittrexKeys == null)
                    ReadFile(BITTREX_FILE);
                return bittrexKeys;
            }
        }

        public static Tuple<string, string> BitfinexKeys
        {
            get
            {
                if (bitfinexKeys == null)
                    ReadFile(BITFINEX_FILE);
                return bitfinexKeys;
            }
        }

        public static Tuple<string, string> PoloniexKeys
        {
            get
            {
                if (poloniexKeys == null)
                    ReadFile(POLONIEX_FILE);
                return poloniexKeys;
            }
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
        public static Tuple<string, string> ReadFile(string file)
        {
            string path = FILE_PATH + file;
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
