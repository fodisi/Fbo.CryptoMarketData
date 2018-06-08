using Fbo.CryptoMarketData.JsonUtils;
using Newtonsoft.Json;
using System;

namespace Fbo.CryptoMarketData.CoinMarketCap
{
    /// <summary>
    /// Represents the metadata information returned by all endpoints available on "https://api.coinmarketcap.com/".
    /// The property <see cref="CmcMetadata.CryptocurrenciesCount"/> is returned by the endpoints "listings" and "ticker", but not by "ticker/{specific currency}" and "global".
    /// </summary>
    public class CmcMetadata
    {
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        [JsonProperty("timestamp"), JsonConverter(typeof(UnixTimestampNullableDateTimeConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the number of active cryptocurrencies.
        /// </summary>
        /// <value>
        /// The number of active cryptocurrencies.
        /// </value>
        [JsonProperty("num_cryptocurrencies")]
        public int CryptocurrenciesCount { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        [JsonProperty("error")]
        public string ErrorMessage { get; set; }
    }
}
