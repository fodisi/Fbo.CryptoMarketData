using Fbo.CryptoMarketData.CoinMarketCap;
using System.Linq;
using NUnit.Framework;
using System;
using FluentAssertions;
using FluentAssertions.Extensions;
using System.Collections.Generic;

namespace Fbo.CryptoMarketData.UnitTest.Core
{
    [TestFixture]
    public class CmcClientUnitTest
    {
        //Although endpoints are supposed to update every 5 minutes, the test should not fail 
        //if the site is down or unable to update data from the blockchains for a couple of days.
        private const int ExpectedLastUpdatedWithinDays = 10;
        private const int ExpectedQuotesWithoutConversion = 1;
        private const int ExpectedQuotesWithConversion = 2;
        private const int ExpectedCryptoCurrencyCount = 1000;
        private const int ExpectedTickersStandardStartPosition = 1;
        private const int ExpectedTickersStandardLimit = 100;
        private const int ExpectedTickersCustomStartPosition = 101;
        private const int ExpectedTickersCustomLimit = 15;
        private const string ExpectedUsdQuote = "USD";
        private const string ExpectedEurQuote = "EUR";
        private CmcCurrency expectedBtcItem;

        private void AssertMetadata(CmcMetadata metadata, bool expectedCurrencyCount)
        {
            metadata.Should().NotBeNull();
            metadata.Timestamp.Should().BeWithin(ExpectedLastUpdatedWithinDays.Days()).Before(DateTime.UtcNow);
            metadata.ErrorMessage.Should().BeNullOrWhiteSpace();
            if (expectedCurrencyCount)
                metadata.CryptocurrenciesCount.Should().BeGreaterThan(ExpectedCryptoCurrencyCount);
        }

        private void AssertTickerList(Dictionary<string, CmcTicker> tickers, int expectedStartPosition, int expectedLimit)
        {
            tickers.Should().NotBeNull();
            tickers.Should().HaveCount(expectedLimit);
            tickers.First().Value.Rank.Should().Be(expectedStartPosition);
            tickers.Last().Value.Rank.Should().Be(expectedStartPosition + expectedLimit - 1);
        }

        private void AssertTicker(CmcTicker ticker, bool assertIsBitcoin)
        {
            ticker.Should().NotBeNull();
            if (assertIsBitcoin)
            {
                ticker.Id.Should().Be(expectedBtcItem.Id);
                ticker.Name.Should().Be(expectedBtcItem.Name);
                ticker.Symbol.Should().Be(expectedBtcItem.Symbol);
                ticker.WebsiteSlug.Should().Be(expectedBtcItem.WebsiteSlug);
            }
            else
            {
                ticker.Id.Should().BeGreaterThan(0);
                ticker.Name.Should().NotBeNullOrWhiteSpace();
                ticker.Symbol.Should().NotBeNullOrWhiteSpace();
                ticker.WebsiteSlug.Should().NotBeNullOrWhiteSpace();
            }

            ticker.Rank.Should().BeGreaterThan(0);
            ticker.LastUpdated.Should().BeWithin(ExpectedLastUpdatedWithinDays.Days()).Before(DateTime.UtcNow);

            if (ticker.CirculatingSupply.HasValue)
                ticker.CirculatingSupply.Value.Should().BeGreaterThan(0);
            if (ticker.TotalSupply.HasValue)
                ticker.TotalSupply.Value.Should().BeGreaterThan(0);
            if (ticker.MaxSupply.HasValue)
                ticker.MaxSupply.Value.Should().BeGreaterThan(0);
        }

        private void AssertTickerQuotes(Dictionary<string, CmcTickerQuote> quotes, bool expectQuoteConversion)
        {
            quotes.Should().NotBeNull();
            quotes.Should().HaveCount(expectQuoteConversion ? ExpectedQuotesWithConversion : ExpectedQuotesWithoutConversion);
            quotes.Should().ContainKeys(expectQuoteConversion ? new string[] { ExpectedUsdQuote, ExpectedEurQuote } : new string[] { ExpectedUsdQuote });

            var tmpQuote = quotes.First().Value;
            if (tmpQuote.Price.HasValue)
                tmpQuote.Price.Value.Should().BePositive();
            if (tmpQuote.MarketCap.HasValue)
                tmpQuote.MarketCap.Value.Should().BePositive();
            if (tmpQuote.Volume24h.HasValue)
                tmpQuote.Volume24h.Value.Should().BePositive();

            if (expectQuoteConversion)
            {
                tmpQuote = quotes.Last().Value;
                if (tmpQuote.Price.HasValue)
                    tmpQuote.Price.Value.Should().BePositive();
                if (tmpQuote.MarketCap.HasValue)
                    tmpQuote.MarketCap.Value.Should().BePositive();
                if (tmpQuote.Volume24h.HasValue)
                    tmpQuote.Volume24h.Value.Should().BePositive();
            }
        }

        private void AssertGlobalData(CmcGlobalData data)
        {
            data.LastUpdated.Should().BeWithin(ExpectedLastUpdatedWithinDays.Days()).Before(DateTime.UtcNow);
            data.ActiveCryptocurrencies.Should().BeGreaterThan(ExpectedCryptoCurrencyCount);
            data.ActiveMarkets.Should().BeGreaterThan(ExpectedCryptoCurrencyCount);
            data.BitcoinPercentage.Should().BePositive();
        }

        private void AssertGlobalDataQuote(Dictionary<string, CmcGlobalDataQuote> quotes, bool expectQuoteConversion)
        {
            quotes.Should().NotBeNull();
            quotes.Should().HaveCount(expectQuoteConversion ? ExpectedQuotesWithConversion : ExpectedQuotesWithoutConversion);
            quotes.Should().ContainKeys(expectQuoteConversion ? new string[] { ExpectedUsdQuote, ExpectedEurQuote } : new string[] { ExpectedUsdQuote });

            quotes.First().Value.TotalMarketCap.Should().BePositive();
            quotes.First().Value.TotalVolume24h.Should().BePositive();

            if (expectQuoteConversion)
            {
                quotes.Last().Value.TotalMarketCap.Should().BePositive();
                quotes.Last().Value.TotalVolume24h.Should().BePositive();
            }
        }

        [SetUp]
        protected void Setup()
        {
            this.expectedBtcItem = new CmcCurrency()
            {
                Id = 1,
                Name = "Bitcoin",
                Symbol = "BTC",
                WebsiteSlug = "bitcoin"
            };
        }

        [Test]
        public void GetListingsReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetCurrencies();

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, true);
            result.Data.Should().NotBeNullOrEmpty();
            result.Data.Length.Should().Be(result.Metadata.CryptocurrenciesCount);
            result.Data.First().Should().BeEquivalentTo(expectedBtcItem);
        }

        [Test]
        public void GetTickerByIdWithoutConversionReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetTickerById(expectedBtcItem.Id);

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, false);
            this.AssertTicker(result.Data, true);
            this.AssertTickerQuotes(result.Data.Quotes, false);
        }

        [Test]
        public void GetTickerByIdWithConversionReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetTickerById(this.expectedBtcItem.Id, ExpectedEurQuote);

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, false);
            this.AssertTicker(result.Data, true);
            this.AssertTickerQuotes(result.Data.Quotes, true);
        }

        [Test]
        public void GetTickersInRangeStandardStartAndLimitWithoutConvertionReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetTickersInRange(ExpectedTickersStandardStartPosition, ExpectedTickersStandardLimit);

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, false);
            this.AssertTickerList(result.Data, ExpectedTickersStandardStartPosition, ExpectedTickersStandardLimit);
            this.AssertTicker(result.Data.First().Value, true); //first item should be bitcoin, because start position is 1.
            this.AssertTicker(result.Data.Last().Value, false);
            this.AssertTickerQuotes(result.Data.First().Value.Quotes, false);
            this.AssertTickerQuotes(result.Data.Last().Value.Quotes, false);
        }

        [Test]
        public void GetTickersInRangeStandardStartAndLimitWithConvertionReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetTickersInRange(ExpectedTickersStandardStartPosition, ExpectedTickersStandardLimit, ExpectedEurQuote);

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, false);
            this.AssertTickerList(result.Data, ExpectedTickersStandardStartPosition, ExpectedTickersStandardLimit);
            this.AssertTicker(result.Data.First().Value, true); //first item should be bitcoin, because start position is 1.
            this.AssertTicker(result.Data.Last().Value, false);
            this.AssertTickerQuotes(result.Data.First().Value.Quotes, true);
            this.AssertTickerQuotes(result.Data.Last().Value.Quotes, true);
        }

        [Test]
        public void GetTickersInRangeCustomStartAndLimitWithoutConvertionReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetTickersInRange(ExpectedTickersCustomStartPosition, ExpectedTickersCustomLimit);

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, false);
            this.AssertTickerList(result.Data, ExpectedTickersCustomStartPosition, ExpectedTickersCustomLimit);
            this.AssertTicker(result.Data.First().Value, false);
            this.AssertTicker(result.Data.Last().Value, false);
            this.AssertTickerQuotes(result.Data.First().Value.Quotes, false);
            this.AssertTickerQuotes(result.Data.Last().Value.Quotes, false);
        }

        [Test]
        public void GetTickersInRangeCustomStartAndLimitWithConvertionReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetTickersInRange(ExpectedTickersCustomStartPosition, ExpectedTickersCustomLimit, ExpectedEurQuote);

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, false);
            this.AssertTickerList(result.Data, ExpectedTickersCustomStartPosition, ExpectedTickersCustomLimit);
            this.AssertTicker(result.Data.First().Value, false);
            this.AssertTicker(result.Data.Last().Value, false);
            this.AssertTickerQuotes(result.Data.First().Value.Quotes, true);
            this.AssertTickerQuotes(result.Data.Last().Value.Quotes, true);
        }

        [Test]
        public void GetAllTickersWithoutConvertionReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetAllTickers();

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, true);
            this.AssertTickerList(result.Data, ExpectedTickersStandardStartPosition, result.Metadata.CryptocurrenciesCount);
            this.AssertTicker(result.Data.First().Value, true);
            this.AssertTicker(result.Data.Last().Value, false);
            this.AssertTickerQuotes(result.Data.First().Value.Quotes, false);
            this.AssertTickerQuotes(result.Data.Last().Value.Quotes, false);
        }

        [Test]
        public void GetAllTickersWithConvertionReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetAllTickers(ExpectedEurQuote);

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, true);
            this.AssertTickerList(result.Data, ExpectedTickersStandardStartPosition, result.Metadata.CryptocurrenciesCount);
            this.AssertTicker(result.Data.First().Value, true);
            this.AssertTicker(result.Data.Last().Value, false);
            this.AssertTickerQuotes(result.Data.First().Value.Quotes, true);
            this.AssertTickerQuotes(result.Data.Last().Value.Quotes, true);
        }

        [Test]
        public void GetGlobalDataWithoutConversionReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetGlobalData();

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, false);
            this.AssertGlobalData(result.Data);
            this.AssertGlobalDataQuote(result.Data.Quotes, false);
        }

        [Test]
        public void GetGlobalDataWithConversionReturnsExpectedInformation()
        {
            var CmcClient = new CmcClient();
            var result = CmcClient.GetGlobalData(ExpectedEurQuote);

            result.Success.Should().BeTrue(result.Metadata.ErrorMessage);
            this.AssertMetadata(result.Metadata, false);
            this.AssertGlobalData(result.Data);
            this.AssertGlobalDataQuote(result.Data.Quotes, true);
        }


    }
}
