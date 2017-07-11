﻿using System;
using System.Collections.Generic;

namespace ArbitrageBot.APIs.Bitfinex
{
    public class Bitfinex : API
    {
        public override decimal GetPriceInBtc(string symbol)
        {
            try
            {

                dynamic jsonData = new BitfinexRequest().GetTicker(symbol + "btc");
                return Convert.ToDecimal(jsonData.last_price);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override List<string> GetSymbols()
        {
            try
            {
                dynamic data = new BitfinexRequest().GetSymbolDetails();
                List<string> symbols = new List<string>();
                foreach (var symbol in data)
                {
                    string s = symbol.pair;
                    if (s.Substring(s.Length - 3) == "btc")
                        symbols.Add(s.Substring(0, s.Length - 3));
                }
                return symbols;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void SetKeys(string key, string secret)
        {
            this.key = key;
            this.secret = secret;
        }
    }
}
