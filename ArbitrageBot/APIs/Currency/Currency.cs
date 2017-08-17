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


        public double BittrexBalance { get; }

        public double BitfinexBalance { get; }

        public double PoloniexBalance { get; }


        public double ?BittrexBid { get; internal set; }

        public double ?BittrexAsk { get; set; }

        public double ?BittrexLast { get; set; }

        public double? BitfinexBid { get; set; }

        public double? BitfinexAsk { get; set; }

        public double? BitfinexLast { get; set; }

        public double? PoloniexBid { get; set; }

        public double? PoloniexAsk { get; set; }

        public double? PoloniexLast { get; set; }



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