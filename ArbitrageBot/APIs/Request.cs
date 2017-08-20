using System;

namespace ArbitrageBot.APIs
{
    public abstract class Request
    {
        /// <summary>
        /// used to build the url of a api call before sending to GetData()
        /// </summary>
        internal string Url { get; set; }

        protected const int TIMEOUT_MILLISECONDS = 5000;

        /// <summary>
        /// gets the current time in millis to include with authenticated api calls
        /// this nonce generator is tested to work with bittrex and bitfinex 7/26/2017
        /// </summary>
        protected string Nonce
        {
            get
            {
                DateTime min = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan span = DateTime.UtcNow - min;
                return long.MaxValue.ToString() + span.TotalMilliseconds.ToString();
            }
        }

        /// <summary>
        /// takes a datetime and converts it to a unix timestamp, needed as params for a handful of APIs
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected string UnixTimeStamp(DateTime dt)
        {
            return (dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
        }

        /// <summary>
        /// handles the http portion of the API call
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected abstract dynamic GetData();

        /// <summary>
        /// handles http portion of post calls
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected abstract dynamic PostData(object payload);

        /// <summary>
        /// hashes some data to create a signature for an authenticated endpoint
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract string GenerateSignature(string data);

    }
}
