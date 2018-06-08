using Newtonsoft.Json;

namespace Fbo.CryptoMarketData.CoinMarketCap
{
    /// <summary>
    /// Represents a quotation in USD or another specified currency. It is used by <see cref="Fbo.CryptoMarketData.CoinMarketCap.CmcGlobalData"/> to store global market's quote data.
    /// </summary>
    public class CmcGlobalDataQuote
    {
        /// <summary>
        /// Gets or sets the total market cap.
        /// </summary>
        /// <value>
        /// The total market cap.
        /// </value>
        [JsonProperty("total_market_cap")]
        public decimal TotalMarketCap { get; set; }

        /// <summary>
        /// Gets or sets the total volume in the last 24 hours.
        /// </summary>
        /// <value>
        /// The total volume in the last 24 hours.
        /// </value>
        [JsonProperty("total_volume_24h")]
        public decimal TotalVolume24h { get; set; }
    }
}
