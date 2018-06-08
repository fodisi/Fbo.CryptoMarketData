using System;
using System.Collections.Generic;
using System.Text;

namespace Fbo.CryptoMarketData.CoinMarketCap
{
    /// <summary>
    /// Represents a set of settings that determines the behavior "https://api.coinmarketcap.com/" API endpoints calls.
    /// </summary>
    /// <remarks>API Call Examples:
    /// <para>Listings endpoint ("listings"):</para>
    /// <para>https://api.coinmarketcap.com/v2/listings/</para>
    /// <para>Ticker endpoint ("ticker"):</para>
    /// <para>https://api.coinmarketcap.com/v2/ticker/</para>
    /// <para>https://api.coinmarketcap.com/v2/ticker/?limit=10</para>
    /// <para><![CDATA[https://api.coinmarketcap.com/v2/ticker/?start=101&limit=10]]></para>
    /// <para><![CDATA[https://api.coinmarketcap.com/v2/ticker/?convert=EUR&limit=10]]></para>
    /// <para><![CDATA[https://api.coinmarketcap.com/v2/ticker/?convert=BTC&limit=10]]></para>
    /// <para><![CDATA[https://api.coinmarketcap.com/v2/ticker/?convert=EURstart=101&limit=10]]></para>
    /// <para>Ticker endpoint with specific id ("ticker/{specific id}"):</para>
    /// <para>https://api.coinmarketcap.com/v2/ticker/1/</para>
    /// <para>https://api.coinmarketcap.com/v2/ticker/1/?convert=EUR</para>
    /// <para>https://api.coinmarketcap.com/v2/ticker/1/?convert=BCH</para>
    /// <para>Global data endpoint ("global"):</para>
    /// <para>https://api.coinmarketcap.com/v2/global/</para>
    /// <para>https://api.coinmarketcap.com/v2/global/?convert=EUR</para>
    /// <para>https://api.coinmarketcap.com/v2/global/?convert=BTC</para>
    /// </remarks>
    public class CmcRequestSettings
    {
        #region Constants
        //API Endpoints
        private const string ListingsEndpoint = "listings";
        private const string TickersEndpoint = "ticker";
        private const string TickerByIdEndpoint = "ticker";
        private const string GlobalEndpoint = "global";

        //API Endpoint Parameters
        private const string LimitParam = "limit";
        private const string StartParam = "start";
        private const string ConvertParam = "convert";

        //Default values
        internal const string DefaultUrl = "https://api.coinmarketcap.com";
        internal const string DefaultVersion = "v2";
        internal const int DefaultStartPosition = 0;
        internal const int DefaultLimit = 100;
        #endregion

        #region Private Members
        private Dictionary<CmcEndpoint, string> Endpoints { get; set; } = new Dictionary<CmcEndpoint, string>
        {
            [CmcEndpoint.Listing] = ListingsEndpoint,
            [CmcEndpoint.Ticker] = TickersEndpoint,
            [CmcEndpoint.TickerSpecificCurrency] = TickerByIdEndpoint,
            [CmcEndpoint.GlobalData] = GlobalEndpoint,
        };
        private Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the base API URL.
        /// </summary>
        /// <value>
        /// The base API URL.
        /// </value>
        public string BaseUrl { get; private set; } = DefaultUrl;
        /// <summary>
        /// Gets the API version.
        /// </summary>
        /// <value>
        /// The API version.
        /// </value>
        public string Version { get; private set; } = DefaultVersion;
        /// <summary>
        /// Gets or sets the API endpoint.
        /// </summary>
        /// <value>
        /// The API endpoint.
        /// </value>
        public CmcEndpoint Endpoint { get; set; } = CmcEndpoint.Listing;
        /// <summary>
        /// Gets or sets the ticker identifier. It is mandatory when <see cref="Endpoint"/> is equal to <see cref="CmcEndpoint.TickerSpecificCurrency"/>.
        /// </summary>
        /// <value>
        /// The ticker identifier.
        /// </value>
        public int TickerId { get; set; }
        /// <summary>
        /// Gets or sets the starting position in the Rank to be returned by the API. It is not mandatory, but can be set when <see cref="Endpoint"/> is equal to <see cref="CmcEndpoint.Ticker"/>.
        /// </summary>
        /// <value>
        /// The start position.
        /// </value>
        public int StartPosition
        {
            get
            {
                return this.Parameters.ContainsKey(StartParam) ? (Int32)this.Parameters[StartParam] : DefaultStartPosition;
            }
            set
            {
                if (value != DefaultStartPosition)
                {
                    if (this.Parameters.ContainsKey(StartParam))
                        this.Parameters[StartParam] = value;
                    else
                        this.Parameters.Add(StartParam, value);
                }
            }
        }
        /// <summary>
        /// Gets or sets the number of tickers returned by the API. It is not mandatory, but can be used when <see cref="Endpoint"/> is equal to <see cref="CmcEndpoint.Ticker"/>.
        /// CoinMarketCap API returns a maximum of 100 tickers per call, so even is this parameter is set to a number bigger than 100, the API will limit to 100 tickers.
        /// </summary>
        /// <value>
        /// The limit of tickers returned.
        /// </value>
        public int Limit
        {
            get
            {
                return this.Parameters.ContainsKey(LimitParam) ? (Int32)this.Parameters[LimitParam] : DefaultLimit;
            }
            set
            {
                if (value != DefaultLimit)
                {
                    if (this.Parameters.ContainsKey(LimitParam))
                        this.Parameters[LimitParam] = value;
                    else
                        this.Parameters.Add(LimitParam, value);
                }
            }
        }
        /// <summary>
        /// Gets or sets the currency converter. It is not mandatory, but can be used when <see cref="Endpoint"/> is any value, except <see cref="CmcEndpoint.Listing"/>.
        /// </summary>
        /// <value>
        /// The currency converter.
        /// </value>
        public string CurrencyConverter
        {
            get
            {
                return this.Parameters.ContainsKey(ConvertParam) ? this.Parameters[ConvertParam].ToString() : String.Empty;
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    if (this.Parameters.ContainsKey(ConvertParam))
                        this.Parameters[ConvertParam] = value;
                    else
                        this.Parameters.Add(ConvertParam, value);
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CmcRequestSettings"/> class.
        /// </summary>
        public CmcRequestSettings() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmcRequestSettings"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        public CmcRequestSettings(CmcEndpoint endpoint)
        {
            this.Endpoint = endpoint;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the relative URL to invoke a specific API endpoint, using specific parameters, according to the properties set to <see cref="CmcRequestSettings"/> instance.
        /// </summary>
        /// <returns>Returns the URL to call the API endpoints.</returns>
        public string GetRelativeUrl()
        {
            StringBuilder str = new StringBuilder($"{this.BaseUrl}/{this.Version}/{this.Endpoints[this.Endpoint]}/");

            if (this.Endpoint.Equals(CmcEndpoint.TickerSpecificCurrency))
                str.Append($"{TickerId}/");

            if (this.Parameters.Count > 0)
            {
                //Adds parameters to the URL in the defined API sequence (convert, start, limit).
                str.Append("?");
                if (this.Parameters.ContainsKey(ConvertParam))
                    str.Append($"{ConvertParam}={this.Parameters[ConvertParam]}&");
                if (this.Parameters.ContainsKey(StartParam))
                    str.Append($"{StartParam}={this.Parameters[StartParam]}&");
                if (this.Parameters.ContainsKey(LimitParam))
                    str.Append($"{LimitParam}={this.Parameters[LimitParam]}&");
                str.Remove(str.Length - 1, 1); // removes the last "&" character
            }

            return str.ToString();
        }
        #endregion
    }
}
