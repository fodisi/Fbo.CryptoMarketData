using Newtonsoft.Json;

namespace Fbo.CryptoMarketData.CoinMarketCap
{
    /// <summary>
    /// Represents a quotation in USD or another specified currency. It is used by <see cref="Fbo.CryptoMarketData.CoinMarketCap.CmcTicker"/> to store tickers' quote data.
    /// </summary>
    public class CmcTickerQuote
    {
        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [JsonProperty("price")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the volume in the last 24 hours.
        /// </summary>
        /// <value>
        /// The volume in the last 24 hours.
        /// </value>
        [JsonProperty("volume_24h")]
        public decimal? Volume24h { get; set; }

        /// <summary>
        /// Gets or sets the market cap.
        /// </summary>
        /// <value>
        /// The market cap.
        /// </value>
        [JsonProperty("market_cap")]
        public decimal? MarketCap { get; set; }

        /// <summary>
        /// Gets or sets the percent change in the last 1 hour.
        /// </summary>
        /// <value>
        /// The percent change in the last 1 hour.
        /// </value>
        [JsonProperty("percent_change_1h")]
        public decimal? PercentChange1h { get; set; }

        /// <summary>
        /// Gets or sets the percent change in the last 24 hours.
        /// </summary>
        /// <value>
        /// The percent change in the last 24 hours.
        /// </value>
        [JsonProperty("percent_change_24h")]
        public decimal? PercentChange24h { get; set; }

        /// <summary>
        /// Gets or sets the percent change in the last 7 days.
        /// </summary>
        /// <value>
        /// The percent change in the last 7 days.
        /// </value>
        [JsonProperty("percent_change_7d")]
        public decimal? PercentChange7d { get; set; }
    }
}
