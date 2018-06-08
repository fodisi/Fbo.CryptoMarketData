using NUnit.Framework;
using FluentAssertions;
using Fbo.CryptoMarketData.CoinMarketCap;

namespace Fbo.CryptoMarketData.UnitTest.Core
{
    [TestFixture]
    class CmcRequestUnitTest
    {
        private const int ExpectedLimit = 10;
        private const int ExpectedStartPosition = 101;
        private const int ExpectedTickerId = 1;
        private const string ExpectedEURConversion = "EUR";

        //Listings
        private const string ExpectedListingUrl = "https://api.coinmarketcap.com/v2/listings/";
        //Ticker
        private const string ExpectedTickerUrl = "https://api.coinmarketcap.com/v2/ticker/";
        private const string ExpectedTickerUrlLimit = "https://api.coinmarketcap.com/v2/ticker/?limit=10";
        private const string ExpectedTickerUrlStartLimit = "https://api.coinmarketcap.com/v2/ticker/?start=101&limit=10";
        private const string ExpectedTickerUrlEURConvertLimit = "https://api.coinmarketcap.com/v2/ticker/?convert=EUR&limit=10";
        private const string ExpectedTickerUrlEURConvertStartLimit = "https://api.coinmarketcap.com/v2/ticker/?convert=EUR&start=101&limit=10";
        //Specific ticker
        private const string ExpectedTickerIdUrl = "https://api.coinmarketcap.com/v2/ticker/1/";
        private const string ExpectedTickerIdUrlEURConvert = "https://api.coinmarketcap.com/v2/ticker/1/?convert=EUR";
        //Global data
        private const string ExpectedGlobalDataUrl = "https://api.coinmarketcap.com/v2/global/";
        private const string ExpectedGlobalDataUrlEURConvert = "https://api.coinmarketcap.com/v2/global/?convert=EUR";

        [Test]
        public void ListingUrlReturnsExpected()
        {
            new CmcRequestSettings(CmcEndpoint.Listing).GetRelativeUrl().Should().Be(ExpectedListingUrl);
        }

        [Test]
        public void TickerUrlReturnsExpected()
        {
            new CmcRequestSettings(CmcEndpoint.Ticker).GetRelativeUrl().Should().Be(ExpectedTickerUrl);
        }

        [Test]
        public void TickerUrlLimitReturnsExpected()
        {
            new CmcRequestSettings
            {
                Endpoint = CmcEndpoint.Ticker,
                Limit = ExpectedLimit
            }
            .GetRelativeUrl().Should().Be(ExpectedTickerUrlLimit);
        }

        [Test]
        public void TickerUrlStartLimitReturnsExpected()
        {
            new CmcRequestSettings
            {
                Endpoint = CmcEndpoint.Ticker,
                Limit = ExpectedLimit,
                StartPosition = ExpectedStartPosition
            }
            .GetRelativeUrl().Should().Be(ExpectedTickerUrlStartLimit);
        }

        [Test]
        public void TickerUrlEURConvertLimitReturnsExpected()
        {
            new CmcRequestSettings
            {
                Endpoint = CmcEndpoint.Ticker,
                Limit = ExpectedLimit,
                CurrencyConverter = ExpectedEURConversion
            }
            .GetRelativeUrl().Should().Be(ExpectedTickerUrlEURConvertLimit);
        }

        [Test]
        public void TickerUrlEURConvertStartLimitReturnsExpected()
        {
            new CmcRequestSettings
            {
                Endpoint = CmcEndpoint.Ticker,
                StartPosition = ExpectedStartPosition,
                Limit = ExpectedLimit,
                CurrencyConverter = ExpectedEURConversion
            }
            .GetRelativeUrl().Should().Be(ExpectedTickerUrlEURConvertStartLimit);
        }

        [Test]
        public void ExpectedTickerIdUrlReturnsExpected()
        {
            new CmcRequestSettings
            {
                Endpoint = CmcEndpoint.TickerSpecificCurrency,
                TickerId = ExpectedTickerId
            }
            .GetRelativeUrl().Should().Be(ExpectedTickerIdUrl);
        }

        [Test]
        public void ExpectedTickerIdEURConvertUrlReturnsExpected()
        {
            new CmcRequestSettings
            {
                Endpoint = CmcEndpoint.TickerSpecificCurrency,
                TickerId = ExpectedTickerId,
                CurrencyConverter = ExpectedEURConversion
            }
            .GetRelativeUrl().Should().Be(ExpectedTickerIdUrlEURConvert);
        }

        [Test]
        public void ExpectedGlobalDataUrlReturnsExpected()
        {
            new CmcRequestSettings
            {
                Endpoint = CmcEndpoint.GlobalData
            }
            .GetRelativeUrl().Should().Be(ExpectedGlobalDataUrl);
        }

        [Test]
        public void ExpectedGlobalDataEURConvertUrlReturnsExpected()
        {
            new CmcRequestSettings
            {
                Endpoint = CmcEndpoint.GlobalData,
                CurrencyConverter = ExpectedEURConversion
            }
            .GetRelativeUrl().Should().Be(ExpectedGlobalDataUrlEURConvert);
        }
    }
}
