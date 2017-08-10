using System;
using System.Collections.Generic;
using ArbitrageBot.APIs.Bitfinex;
using ArbitrageBot.APIs.Bittrex;
using ArbitrageBot.APIs.Poloniex;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ArbitrageBot.CurrencyUtil
{
    public class Currency
    {

        public Currency(string symbol)
        {
            Symbol = symbol;
        }

        public string Symbol { get; set; }

        public string BittrexName { get; set; }

        public string BitfinexName { get; set; }

        public string PoloniexName { get; set; }


        public string BittrexVolume { get; set; }

        public string BitfinexVolume { get; set; }

        public string PoloniexVolume { get; set; }


        public string BittrexBtcPair { get; set; }

        public string BitfinexBtcPair { get; set; }

        public string PoloniexBtcPair { get; set; }


        public decimal ?BittrexBid { get; set; }

        public decimal ?BittrexAsk { get; set; }

        public decimal ?BittrexLast { get; set; }

        public decimal? BitfinexBid { get; set; }

        public decimal? BitfinexAsk { get; set; }

        public decimal? BitfinexLast { get; set; }

        public decimal? PoloniexBid { get; set; }

        public decimal? PoloniexAsk { get; set; }

        public decimal? PoloniexLast { get; set; }



        public override bool Equals(object obj)
        {
            if (!(obj is Currency))
                return false;
            else
                return ((Currency)obj).Symbol.ToUpper() == Symbol.ToUpper();
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Symbol.GetHashCode();
                hash = (hash * 16777619) ^ Symbol.ToLower().GetHashCode();
                return hash;
            }
        }
    }
}