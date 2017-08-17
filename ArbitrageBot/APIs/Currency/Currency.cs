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

        public string BittrexName { get; internal set; }

        public string BitfinexName { get; internal set; }

        public string PoloniexName { get; internal set; }


        public string BittrexVolume { get; internal set; }

        public string BitfinexVolume { get; internal set; }

        public string PoloniexVolume { get; internal set; }


        public string BittrexBtcPair { get; internal set; }

        public string BitfinexBtcPair { get; internal set; }

        public string PoloniexBtcPair { get; internal set; }


        public double BittrexBalance { get; }

        public double BitfinexBalance { get; }

        public double PoloniexBalance { get; }


        public double ?BittrexBid { get; internal set; }

        public double ?BittrexAsk { get; internal set; }

        public double ?BittrexLast { get; internal set; }

        public double? BitfinexBid { get; internal set; }

        public double? BitfinexAsk { get; internal set; }

        public double? BitfinexLast { get; internal set; }

        public double? PoloniexBid { get; internal set; }

        public double? PoloniexAsk { get; internal set; }

        public double? PoloniexLast { get; internal set; }



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