using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitrageBot.APIs.Bittrex;
using Microsoft.CSharp;
using Newtonsoft.Json;


namespace UnitTests
{
    [TestClass]
    public class ApiUnitTest
    {
        //const string MARKET_STRING = "btc-ltc";
        

        //__Public Methods__

        //[TestMethod]
        //public void GetTicker_Test()
        //{
        //    dynamic data = new BittrexRequest().Public().GetTicker(MARKET_STRING).success;
        //    bool assert = data.success;
        //    Assert.IsNotNull(data, "Data for bittrex GetTicker request was null");
        //}

        //__Market Methods__


        //__Account Methods__

        //[TestMethod]
        //public void GetOrderHistory_Test()
        //{
        //    dynamic data = new BittrexRequest().Public().GetOrderHistory();
        //    Assert.IsNotNull(data, "Data for bittrex request was null");
        //    Assert.IsTrue(data.success, "Invalid api call");
        //}
    }
}
