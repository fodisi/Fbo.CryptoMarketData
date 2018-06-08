using Fbo.CryptoMarketData.JsonUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fbo.CryptoMarketData.CoinMarketCap
{
    /// <summary>
    /// Represents the global data available on "https://api.coinmarketcap.com/", returned by calling the API Endpoint "global/".
    /// </summary>
    public class CmcGlobalData
    {
        /// <summary>
        /// Gets or sets the number of active cryptocurrencies.
        /// </summary>
        /// <value>
        /// The number of active cryptocurrencies.
        /// </value>
        [JsonProperty("active_cryptocurrencies")]
        public int ActiveCryptocurrencies { get; set; }

        /// <summary>
        /// Gets or sets the number of active markets.
        /// </summary>
        /// <value>
        /// The number of active markets.
        /// </value>
        [JsonProperty("active_markets")]
        public int ActiveMarkets { get; set; }

        /// <summary>
        /// Gets or sets the bitcoin percentage of the total market cap.
        /// </summary>
        /// <value>
        /// The bitcoin percentage.
        /// </value>
        [JsonProperty("bitcoin_percentage_of_market_cap")]
        public decimal BitcoinPercentage { get; set; }

        /// <summary>
        /// Gets or sets the quotes for the global market.
        /// </summary>
        /// <value>
        /// The quotes for the global market.
        /// </value>
        [JsonProperty("quotes")]
        public Dictionary<string, CmcGlobalDataQuote> Quotes { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the last data update.
        /// </summary>
        /// <value>
        /// The date and time of the last data update.
        /// </value>
        [JsonProperty("last_updated"), JsonConverter(typeof(UnixTimestampNullableDateTimeConverter))]
        public DateTime LastUpdated { get; set; }
    }
}
