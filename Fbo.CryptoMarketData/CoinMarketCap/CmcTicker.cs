using Fbo.CryptoMarketData.JsonUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fbo.CryptoMarketData.CoinMarketCap
{
    /// <summary>
    /// Represents a currency ticker listed on "https://api.coinmarketcap.com/", returned by calling the API Endpoints "ticker/" or "ticker/{specific currency}/".
    /// </summary>
    public class CmcTicker
    {
        /// <summary>
        /// Gets or sets the ticker identifier.
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

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>
        /// The rank.
        /// </value>
        [JsonProperty("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the circulating supply.
        /// </summary>
        /// <value>
        /// The circulating supply.
        /// </value>
        [JsonProperty("circulating_supply")]
        public Decimal? CirculatingSupply { get; set; }

        /// <summary>
        /// Gets or sets the total supply.
        /// </summary>
        /// <value>
        /// The total supply.
        /// </value>
        [JsonProperty("total_supply")]
        public Decimal? TotalSupply { get; set; }

        /// <summary>
        /// Gets or sets the maximum supply.
        /// </summary>
        /// <value>
        /// The maximum supply.
        /// </value>
        [JsonProperty("max_supply")]
        public Decimal? MaxSupply { get; set; }

        /// <summary>
        /// Gets or sets the quotes for the ticker.
        /// </summary>
        /// <value>
        /// The quotes.
        /// </value>
        [JsonProperty("quotes")]
        public Dictionary<string, CmcTickerQuote> Quotes { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the last data update.
        /// </summary>
        /// <value>
        /// The date and time of the last data update.
        /// </value>
        [JsonProperty("last_updated"), JsonConverter(typeof(UnixTimestampNullableDateTimeConverter))]
        public DateTime? LastUpdated { get; set; }
    }
}
