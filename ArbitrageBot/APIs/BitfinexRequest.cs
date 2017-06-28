using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace ArbitrageBot.APIs
{
    public class BitfinexRequest
    {
        private string url = "https://api.bitfinex.com/v1";
        public BitfinexRequest()
        {

        }

        private string GetData(string url)
        {
            WebResponse response = ((HttpWebRequest)WebRequest.Create(url)).GetResponse();
            return new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
        }


        public dynamic GetTicker(string market)
        {
            try
            {
                url += "/pubticker/" + market;
                return JsonConvert.DeserializeObject(GetData(url));
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR GETTING TICKER FOR " + market);
                return null;
            }
        }
       
    }
}
