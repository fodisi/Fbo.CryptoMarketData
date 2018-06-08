using Newtonsoft.Json;
using System;

namespace Fbo.CryptoMarketData.CoinMarketCap
{
    /// <summary>
    /// Represents the result obtained by calling "https://api.coinmarketcap.com/" API endpoints.
    /// </summary>
    /// <typeparam name="T">The expected data type to be used for <see cref="Data"/>. It varies according to the API endpoint invoked.
    /// <para>listing: <c>CmcResponse{<see cref="CmcCurrency[]"/>}</c>.</para>
    /// <para>ticker => <c>CmcResponse{<see cref="Dictionary{string, CmcTicker}"/>}</c>,
    /// where <see cref="Dictionary{TKey}"/> is <see cref="String"/> and <see cref="Dictionary{TValue}"/> is <see cref="CmcTicker"/>.</para>
    /// <para>ticker/{specific currency} => <c>CmcResponse{<see cref="CmcTicker"/>}</c>.</para>
    /// <para>global => <c>CmcResponse{<see cref="CmcGlobalData"/>}</c>.</para>
    /// </typeparam>
    /// <seealso cref="Fbo.CryptoMarketData.CoinMarketCap.CmcCurrency"/>
    /// <seealso cref="Fbo.CryptoMarketData.CoinMarketCap.CmcTicker"/>
    /// <seealso cref="Fbo.CryptoMarketData.CoinMarketCap.CmcGlobalData"/>
    public class CmcResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CmcResponse{T}"/> class.
        /// </summary>
        public CmcResponse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmcResponse{T}"/> class.
        /// </summary>
        /// <param name="error">An error message that will be assigned to <see cref="Metadata"/>.<see cref="CmcMetadata.ErrorMessage"/>.</param>
        public CmcResponse(string error)
        {
            this.Metadata.ErrorMessage = error;
        }

        /// <summary>
        /// Gets the data provided by the API endpoint.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("data")]
        public T Data { get; internal set; }

        /// <summary>
        /// Gets the metadata provided by the API endpoint.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        [JsonProperty("metadata")]
        public CmcMetadata Metadata { get; internal set; } = new CmcMetadata();

        /// <summary>
        /// Gets a value indicating whether this <see cref="CmcResponse{T}"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool Success { get { return ((this.Metadata != null) &&  (String.IsNullOrWhiteSpace(this.Metadata.ErrorMessage))); } }

    }
}
