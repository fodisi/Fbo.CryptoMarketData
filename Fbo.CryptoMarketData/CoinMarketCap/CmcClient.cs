using Fbo.CryptoMarketData.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fbo.CryptoMarketData.CoinMarketCap
{
    /// <summary>
    /// A client wrapper to "https://api.coinmarketcap.com/" API. Provides sync and async methods for calling the public API endpoints (version 2.0).
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
    public class CmcClient
    {
        /// <summary>
        /// Gets a list of the supported fiat currencies conversions.
        /// </summary>
        /// <value>
        /// The supported fiat currencies conversions.
        /// </value>
        public List<string> SupportedFiatCurrencyConverters { get; private set; } = new List<string>()
        {
            "AUD", "BRL", "CAD", "CHF", "CLP", "CNY", "CZK", "DKK", "EUR", "GBP", "HKD", "HUF", "IDR", "ILS", "INR", "JPY",
            "KRW", "MXN", "MYR", "NOK", "NZD", "PHP", "PKR", "PLN", "RUB", "SEK", "SGD", "THB", "TRY", "TWD", "ZAR"
        };

        /// <summary>
        /// Gets a list of the supported cryptocurrencies conversions.
        /// </summary>
        /// <value>
        /// The supported cryptocurrencies conversions.
        /// </value>
        public List<string> SupportedCryptoCurrencyConverters { get; private set; } = new List<string>()
        {
            "BTC", "ETH", "XRP", "LTC", "BCH"
        };

        /// <summary>
        /// Executes a request to "https://api.coinmarketcap.com/" API using the specified settings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settings">The settings determining which endpoint will be called and if any parameter should be submitted to the endpoint.</param>
        /// <returns></returns>
        private async Task<CmcResponse<T>> ExecuteRequest<T>(CmcRequestSettings settings)
        {
            string content = String.Empty;
            try
            {
                using (var http = new HttpClient())
                {
                    var response = await http.GetAsync(settings.GetRelativeUrl());
                    content = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        var error = JsonConvert.DeserializeObject<CmcResponse<T>>(content);
                        error.Metadata.ErrorMessage = $"HTTP Response - Status code: {response.StatusCode}. Message: {error.Metadata.ErrorMessage}.";
                        return error;
                    }

                    //Gets the result and sets success accordingly. Empty error message means success.
                    var result = JsonConvert.DeserializeObject<CmcResponse<T>>(content);
                    return result;
                }
            }
            catch (JsonReaderException jre)
            {
                return new CmcResponse<T>($"Exception: '{nameof(JsonSerializationException)}'." +
                                   $"{Environment.NewLine}" +
                                   $"Error occurred at Path: '{jre.Path}'; Line number: '{jre.LineNumber}', Line position: '{jre.LinePosition}'." +
                                   $"{Environment.NewLine}" +
                                   $"Received data: '{content}'."
                );
            }
            catch (JsonSerializationException jse)
            {
                return new CmcResponse<T>($"Exception: '{nameof(JsonSerializationException)}'." +
                                   $"{Environment.NewLine}" +
                                   $"Message: '{jse.Message}'." +
                                   $"{Environment.NewLine}" +
                                   $"Received data: '{content}'."
                );
            }
            catch (CryptoMarketDataException cmde)
            {
                return new CmcResponse<T>($"Exception: '{nameof(CryptoMarketDataException)}'" +
                                   $"{Environment.NewLine}" +
                                   $"Message: '{cmde.Message}'." +
                                   $"{Environment.NewLine}" +
                                   $"Inner exception: '{(cmde.InnerException != null ? cmde.InnerException.Message : String.Empty)}'."
                );
            }
            catch (Exception e)
            {
                return new CmcResponse<T>($"Exception: '{e.GetType()}'" +
                                   $"{Environment.NewLine}" +
                                   $"Message: '{e.Message}'." +
                                   $"{Environment.NewLine}" +
                                   $"Inner exception: '{(e.InnerException != null ? e.InnerException.Message : String.Empty)}'."
                );
            }
        }

        /// <summary>
        /// Gets a list of currencies provided by the "https://api.coinmarketcap.com/v2/listings/" API endpoint. Performs a synchronous call to the API.
        /// </summary>
        /// <returns>Returns a <see cref="CmcResponse{CmcCurrency[]}"/> with the result provided by the API, or any error occurred during the call.</returns>
        public CmcResponse<CmcCurrency[]> GetCurrencies() => GetCurrenciesAsync().Result;

        /// <summary>
        /// Gets a list of currencies provided by the "https://api.coinmarketcap.com/v2/listings/" API endpoint. Performs an asynchronous call to the API.
        /// </summary>
        /// <returns>Returns a <see cref="Task{CmcResponse{CmcCurrency[]}}"/> with the result provided by the API, or any error occurred during the call.</returns>
        public async Task<CmcResponse<CmcCurrency[]>> GetCurrenciesAsync()
        {
            return await ExecuteRequest<CmcCurrency[]>(new CmcRequestSettings(CmcEndpoint.Listing));
        }

        /// <summary>
        /// Gets a ticker with a specific id provided by the "https://api.coinmarketcap.com/v2/ticker{id}/" API endpoint. Performs a synchronous call to the API.
        /// </summary>
        /// <param name="tickerId">The ticker identifier.</param>
        /// <param name="converter">An optional supported currency converter.</param>
        /// <returns>Returns a <see cref="CmcResponse{CmcTicker}"/> with the result provided by the API, or any error occurred during the call.</returns>
        /// <seealso cref="SupportedFiatCurrencyConverters"/>.
        /// <seealso cref="SupportedCryptoCurrencyConverters"/>.
        public CmcResponse<CmcTicker> GetTickerById(int tickerId, string converter = null)
            => GetTickerByIdAsync(tickerId, converter).Result;

        /// <summary>
        /// Gets a ticker with a specific id provided by the "https://api.coinmarketcap.com/v2/ticker{id}/" API endpoint. Performs an asynchronous call to the API.
        /// </summary>
        /// <param name="tickerId">The ticker identifier.</param>
        /// <param name="converter">An optional supported currency converter.</param>
        /// <returns>Returns a <see cref="Task{CmcResponse{CmcTicker}}"/> with the result provided by the API, or any error occurred during the call.</returns>
        /// <seealso cref="SupportedFiatCurrencyConverters"/>.
        /// <seealso cref="SupportedCryptoCurrencyConverters"/>.
        public async Task<CmcResponse<CmcTicker>> GetTickerByIdAsync(int tickerId, string converter = null)
        {
            return await ExecuteRequest<CmcTicker>(
                new CmcRequestSettings()
                {
                    Endpoint = CmcEndpoint.TickerSpecificCurrency,
                    TickerId = tickerId,
                    CurrencyConverter = converter
                });
        }

        /// <summary>
        /// Gets a list of tickers in a specific range, beginning at a specific position and with a certain quantity of tickers,
        /// provided by the "https://api.coinmarketcap.com/v2/ticker/" API endpoint. Performs a synchronous call to the API.
        /// </summary>
        /// <param name="startPosition">The starting position (range starts at 1, not 0).</param>
        /// <param name="limit">The number of tickers to be returned. The maximum number supported by the API is 100.</param>
        /// <param name="converter">An optional supported currency converter.</param>
        /// <returns>Returns a <see cref="CmcResponse{Dictionary{string, CmcTicker}}"/> with the result provided by the API, or any error occurred during the call.</returns>
        /// <seealso cref="SupportedFiatCurrencyConverters"/>.
        /// <seealso cref="SupportedCryptoCurrencyConverters"/>.
        public CmcResponse<Dictionary<string, CmcTicker>> GetTickersInRange(int startPosition, int limit, string converter = null)
            => GetTickersInRangeAsync(startPosition, limit, converter).Result;

        /// <summary>
        /// Gets a list of tickers in a specific range, beginning at a specific position and with a certain quantity of tickers,
        /// provided by the "https://api.coinmarketcap.com/v2/ticker/" API endpoint. Performs an asynchronous call to the API.
        /// </summary>
        /// <param name="startPosition">The starting position (range starts at 1, not 0).</param>
        /// <param name="limit">The number of tickers to be returned. The maximum number supported by the API is 100.</param>
        /// <param name="converter">An optional supported currency converter.</param>
        /// <returns>Returns a <see cref="Task{CmcResponse{Dictionary{string, CmcTicker}}}"/> with the result provided by the API, or any error occurred during the call.</returns>
        /// <seealso cref="SupportedFiatCurrencyConverters"/>.
        /// <seealso cref="SupportedCryptoCurrencyConverters"/>.
        public async Task<CmcResponse<Dictionary<string, CmcTicker>>> GetTickersInRangeAsync(int startPosition, int limit, string converter = null)
        {
            return await ExecuteRequest<Dictionary<string, CmcTicker>>(
                new CmcRequestSettings()
                {
                    Endpoint = CmcEndpoint.Ticker,
                    CurrencyConverter = converter,
                    StartPosition = startPosition,
                    Limit = limit
                });
        }

        /// <summary>
        /// Gets a list of all tickers provided by the "https://api.coinmarketcap.com/v2/ticker/" API endpoint. Performs a synchronous call to the API.
        /// </summary>
        /// <param name="converter">An optional supported currency converter.</param>
        /// <returns>Returns a <see cref="CmcResponse{Dictionary{string, CmcTicker}}"/> with the result provided by the API, or any error occurred during the call.</returns>
        /// <seealso cref="SupportedFiatCurrencyConverters"/>.
        /// <seealso cref="SupportedCryptoCurrencyConverters"/>.
        public CmcResponse<Dictionary<string, CmcTicker>> GetAllTickers(string converter = null)
            => GetAllTickersAsync(converter).Result;

        /// <summary>
        /// Gets a list of all tickers in a specific range provided by the "https://api.coinmarketcap.com/v2/ticker/" API endpoint. Performs an asynchronous call to the API.
        /// </summary>
        /// <param name="converter">An optional supported currency converter.</param>
        /// <returns>Returns a <see cref="Task{CmcResponse{Dictionary{string, CmcTicker}}}"/> with the result provided by the API, or any error occurred during the call.</returns>
        /// <seealso cref="SupportedFiatCurrencyConverters"/>.
        /// <seealso cref="SupportedCryptoCurrencyConverters"/>.
        public async Task<CmcResponse<Dictionary<string, CmcTicker>>> GetAllTickersAsync(string converter = null)
        {
            var cmcRequest = new CmcRequestSettings()
            {
                Endpoint = CmcEndpoint.Ticker,
                CurrencyConverter = converter,
                StartPosition = CmcRequestSettings.DefaultStartPosition,
                Limit = CmcRequestSettings.DefaultLimit
            };

            var response = await ExecuteRequest<Dictionary<string, CmcTicker>>(cmcRequest);
            if (!response.Success)
                return response;

            int totalCurrencyCount = response.Metadata.CryptocurrenciesCount;
            int currentCounter = response.Data.Count;
            cmcRequest.StartPosition = 1; //set to one, so in the loop it can be incremented by LIMIT (100) => 101, 201, 301...
            var tempResponse = response;
            while (currentCounter < totalCurrencyCount && tempResponse.Success)
            {
                cmcRequest.StartPosition += cmcRequest.Limit;
                tempResponse = await ExecuteRequest<Dictionary<string, CmcTicker>>(cmcRequest);
                if (!tempResponse.Success)
                    return tempResponse;

                foreach (var item in tempResponse.Data)
                {
                    if (!response.Data.ContainsKey(item.Key))
                        response.Data.Add(item.Key, item.Value);
                }

                currentCounter = response.Data.Count;
            }

            return response;
        }

        /// <summary>
        /// Gets the global cryptocurrency market data provided by the "https://api.coinmarketcap.com/v2/global/" API endpoint. Performs a synchronous call to the API.
        /// </summary>
        /// <param name="converter">An optional supported currency converter.</param>
        /// <returns>Returns a <see cref="CmcResponse{CmcGlobalData}"/> with the result provided by the API, or any error occurred during the call.</returns>
        /// <seealso cref="SupportedFiatCurrencyConverters"/>.
        /// <seealso cref="SupportedCryptoCurrencyConverters"/>.
        public CmcResponse<CmcGlobalData> GetGlobalData(string converter = null)
            => GetGlobalDataAsync(converter).Result;

        /// <summary>
        /// Gets the global cryptocurrency market data provided by the "https://api.coinmarketcap.com/v2/global/" API endpoint. Performs an asynchronous call to the API.
        /// </summary>
        /// <param name="converter">An optional supported currency converter.</param>
        /// <returns>Returns a <see cref="Task{CmcResponse{CmcGlobalData}}"/> with the result provided by the API, or any error occurred during the call.</returns>
        /// <seealso cref="SupportedFiatCurrencyConverters"/>.
        /// <seealso cref="SupportedCryptoCurrencyConverters"/>.
        public async Task<CmcResponse<CmcGlobalData>> GetGlobalDataAsync(string converter = null)
        {
            return await ExecuteRequest<CmcGlobalData>(
                new CmcRequestSettings()
                {
                    Endpoint = CmcEndpoint.GlobalData,
                    CurrencyConverter = converter
                });
        }
    }
}
