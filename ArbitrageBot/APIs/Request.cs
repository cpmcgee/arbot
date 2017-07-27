using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArbitrageBot.APIs
{
    public abstract class Request
    {
        /// <summary>
        /// used to build the url of a api call before sending to GetData()
        /// </summary>
        protected string Url { get; set; }

        /// <summary>
        /// gets the current time in millis to include with authenticated api calls
        /// this nonce generator is tested to work with bittrex and bitfinex 7/26/2017
        /// </summary>
        protected static string Nonce
        {
            get
            {
                DateTime min = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan span = DateTime.UtcNow - min;
                return long.MaxValue.ToString() + span.TotalMilliseconds.ToString();
            }
        }
        //protected static string Nonce { get { return Guid.NewGuid().ToString(); } }
      
        /// <summary>
        /// handles the http portion of the API call
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected abstract dynamic GetData();

        /// <summary>
        /// hashes some data to create a signature for an authenticated endpoint
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract string GenerateSignature(string data);
    }
}
