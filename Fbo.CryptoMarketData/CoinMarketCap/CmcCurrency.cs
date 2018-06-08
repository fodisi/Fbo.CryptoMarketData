using Newtonsoft.Json;
using System;

namespace Fbo.CryptoMarketData.CoinMarketCap
{
    /// <summary>
    /// Represents a currency listed on "https://api.coinmarketcap.com/", returned by calling the API Endpoint "listings/".
    /// </summary>
    public class CmcCurrency
    {
        /// <summary>
        /// Gets or sets the currency identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        [JsonProperty("symbol")]
        public String Symbol { get; set; }

        /// <summary>
        /// Gets or sets the website slug.
        /// </summary>
        /// <value>
        /// The website slug.
        /// </value>
        [JsonProperty("website_slug")]
        public String WebsiteSlug { get; set; }
    }
}
