using Fbo.CryptoMarketData.CoinMarketCap;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace Fbo.CryptoMarketData.UnitTest.Core
{
    [TestFixture]
    public class CmcObjectsJsonSerializationTests
    {
        private bool SerializeAndCompare<T>(string jsonContent, bool ignoreCurrencyCountProperty)
        {
            string reserializedJson = String.Empty;
            using (var sr = new StringReader(jsonContent))
            using (var jr = new JsonTextReader(sr))
            {
                var serial = new JsonSerializer { Formatting = Formatting.Indented };
                var obj = serial.Deserialize<CmcResponse<T>>(jr);

                if (ignoreCurrencyCountProperty)
                {
                    var jsonResolver = new IgnorePropertySerializerContractResolver(new List<String> { nameof(CmcMetadata.CryptocurrenciesCount) });
                    var serializerSettings = new JsonSerializerSettings { ContractResolver = jsonResolver };
                    reserializedJson = JsonConvert.SerializeObject(obj, Formatting.Indented, serializerSettings);
                }
                else
                {
                    reserializedJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
                }
            }

            return (String.Compare(jsonContent, reserializedJson, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) == 0);
        }

        [Test]
        public void ListingsSerializedObjectsAreEqual()
        {
            string jsonContent = @"
                            {
                                ""data"": [
                                    {
                                        ""id"": 1, 
                                        ""name"": ""Bitcoin"", 
                                        ""symbol"": ""BTC"", 
                                        ""website_slug"": ""bitcoin""
                                    }, 
                                    {
                                        ""id"": 2, 
                                        ""name"": ""Litecoin"", 
                                        ""symbol"": ""LTC"", 
                                        ""website_slug"": ""litecoin""
                                    }, 
                                    {
                                        ""id"": 3, 
                                        ""name"": ""Namecoin"", 
                                        ""symbol"": ""NMC"", 
                                        ""website_slug"": ""namecoin""
                                    }
                                ], 
                                ""metadata"": {
                                    ""timestamp"": 1525451579, 
                                    ""num_cryptocurrencies"": 1610, 
                                    ""error"": null
                                }
                            }";

            SerializeAndCompare<CmcCurrency[]>(jsonContent, false).Should().BeTrue();
        }

        [Test]
        public void TickersWithoutCurrencyConversionSerializedObjectsAreEqual()
        {
            string jsonContent = @"
                            {
                                ""data"": {
                                    ""1"": {
                                            ""id"": 1, 
                                        ""name"": ""Bitcoin"", 
                                        ""symbol"": ""BTC"", 
                                        ""website_slug"": ""bitcoin"", 
                                        ""rank"": 1, 
                                        ""circulating_supply"": 17014812.0, 
                                        ""total_supply"": 17014812.0, 
                                        ""max_supply"": 21000000.0, 
                                        ""quotes"": {
                                                ""USD"": {
                                                    ""price"": 9648.51, 
                                                ""volume_24h"": 9578390000.0, 
                                                ""market_cap"": 164167583730.0, 
                                                ""percent_change_1h"": -0.69, 
                                                ""percent_change_24h"": 2.92, 
                                                ""percent_change_7d"": 3.92
                                                }
                                            }, 
                                        ""last_updated"": 1525442371
                                    }, 
                                    ""1027"": {
                                            ""id"": 1027, 
                                        ""name"": ""Ethereum"", 
                                        ""symbol"": ""ETH"", 
                                        ""website_slug"": ""ethereum"", 
                                        ""rank"": 2, 
                                        ""circulating_supply"": 99224898.0, 
                                        ""total_supply"": 99224898.0, 
                                        ""max_supply"": null, 
                                        ""quotes"": {
                                                ""USD"": {
                                                    ""price"": 794.479, 
                                                ""volume_24h"": 3939350000.0, 
                                                ""market_cap"": 78832097613.0, 
                                                ""percent_change_1h"": -0.3, 
                                                ""percent_change_24h"": 6.64, 
                                                ""percent_change_7d"": 17.71
                                                }
                                            }, 
                                        ""last_updated"": 1525442357
                                    }, 
                                    ""52"": {
                                            ""id"": 52, 
                                        ""name"": ""Ripple"", 
                                        ""symbol"": ""XRP"", 
                                        ""website_slug"": ""ripple"", 
                                        ""rank"": 3, 
                                        ""circulating_supply"": 39178259468.0, 
                                        ""total_supply"": 99992263539.0, 
                                        ""max_supply"": 100000000000.0, 
                                        ""quotes"": {
                                                ""USD"": {
                                                    ""price"": 0.901755, 
                                                ""volume_24h"": 1075760000.0, 
                                                ""market_cap"": 35329191367.0, 
                                                ""percent_change_1h"": -0.82, 
                                                ""percent_change_24h"": 3.8, 
                                                ""percent_change_7d"": 6.1
                                                }
                                            }, 
                                        ""last_updated"": 1525442341
                                    }
                                }, 
                                ""metadata"": {
                                    ""timestamp"": 1525442286, 
                                    ""num_cryptocurrencies"": 1610, 
                                    ""error"": null
                                }
                            }";

            SerializeAndCompare<Dictionary<string, CmcTicker>>(jsonContent, false).Should().BeTrue();
        }

        [Test]
        public void TickersWithCurrencyConversionSerializedObjectsAreEqual()
        {
            string jsonContent = @"
                                {
                                    ""data"": {
                                        ""1"": {
                                                ""id"": 1, 
                                            ""name"": ""Bitcoin"", 
                                            ""symbol"": ""BTC"", 
                                            ""website_slug"": ""bitcoin"", 
                                            ""rank"": 1, 
                                            ""circulating_supply"": 17014912.0, 
                                            ""total_supply"": 17014912.0, 
                                            ""max_supply"": 21000000.0, 
                                            ""quotes"": {
                                                    ""USD"": {
                                                        ""price"": 9663.38, 
                                                    ""volume_24h"": 9410870000.0, 
                                                    ""market_cap"": 164421560323.0, 
                                                    ""percent_change_1h"": -0.27, 
                                                    ""percent_change_24h"": 2.78, 
                                                    ""percent_change_7d"": 4.16
                                                    }, 
                                                ""EUR"": {
                                                        ""price"": 8074.32412942, 
                                                    ""volume_24h"": 7863337126.330001, 
                                                    ""market_cap"": 137383914522.0, 
                                                    ""percent_change_1h"": -0.27, 
                                                    ""percent_change_24h"": 2.78, 
                                                    ""percent_change_7d"": 4.16
                                                }
                                                }, 
                                            ""last_updated"": 1525444772
                                        }, 
                                        ""1027"": {
                                                ""id"": 1027, 
                                            ""name"": ""Ethereum"", 
                                            ""symbol"": ""ETH"", 
                                            ""website_slug"": ""ethereum"", 
                                            ""rank"": 2, 
                                            ""circulating_supply"": 99225464.0, 
                                            ""total_supply"": 99225464.0, 
                                            ""max_supply"": null, 
                                            ""quotes"": {
                                                    ""USD"": {
                                                        ""price"": 792.549, 
                                                    ""volume_24h"": 3868450000.0, 
                                                    ""market_cap"": 78641042490.0, 
                                                    ""percent_change_1h"": -1.04, 
                                                    ""percent_change_24h"": 6.1, 
                                                    ""percent_change_7d"": 17.41
                                                    }, 
                                                ""EUR"": {
                                                        ""price"": 662.221449891, 
                                                    ""volume_24h"": 3232318213.55, 
                                                    ""market_cap"": 65709230822.0, 
                                                    ""percent_change_1h"": -1.04, 
                                                    ""percent_change_24h"": 6.1, 
                                                    ""percent_change_7d"": 17.41
                                                }
                                                }, 
                                            ""last_updated"": 1525444757
                                        }, 
                                        ""52"": {
                                                ""id"": 52, 
                                            ""name"": ""Ripple"", 
                                            ""symbol"": ""XRP"", 
                                            ""website_slug"": ""ripple"", 
                                            ""rank"": 3, 
                                            ""circulating_supply"": 39178259468.0, 
                                            ""total_supply"": 99992263539.0, 
                                            ""max_supply"": 100000000000.0, 
                                            ""quotes"": {
                                                    ""USD"": {
                                                        ""price"": 0.902515, 
                                                    ""volume_24h"": 1069180000.0, 
                                                    ""market_cap"": 35358966844.0, 
                                                    ""percent_change_1h"": -0.58, 
                                                    ""percent_change_24h"": 3.6, 
                                                    ""percent_change_7d"": 6.23
                                                    }, 
                                                ""EUR"": {
                                                        ""price"": 0.7541045309, 
                                                    ""volume_24h"": 893362971.62, 
                                                    ""market_cap"": 29544502977.0, 
                                                    ""percent_change_1h"": -0.58, 
                                                    ""percent_change_24h"": 3.6, 
                                                    ""percent_change_7d"": 6.23
                                                }
                                                }, 
                                            ""last_updated"": 1525444741
                                        }
                                        }, 
                                    ""metadata"": {
                                        ""timestamp"": 1525444608, 
                                        ""num_cryptocurrencies"": 1610, 
                                        ""error"": null
                                    }
                                }";

            SerializeAndCompare<Dictionary<string, CmcTicker>>(jsonContent, false).Should().BeTrue();
        }

        [Test]
        public void TickerSpecificCurrencyWithoutCurrencyConversionSerializedObjectsAreEqual()
        {
            string jsonContent = @"
                                {
                                    ""data"": {
                                        ""id"": 1, 
                                        ""name"": ""Bitcoin"", 
                                        ""symbol"": ""BTC"", 
                                        ""website_slug"": ""bitcoin"", 
                                        ""rank"": 1, 
                                        ""circulating_supply"": 17014912.0, 
                                        ""total_supply"": 17014912.0, 
                                        ""max_supply"": 21000000.0, 
                                        ""quotes"": {
                                                ""USD"": {
                                                    ""price"": 9663.55, 
                                                ""volume_24h"": 9389440000.0, 
                                                ""market_cap"": 164424452858.0, 
                                                ""percent_change_1h"": -0.19, 
                                                ""percent_change_24h"": 2.77, 
                                                ""percent_change_7d"": 4.18
                                                }
                                            }, 
                                        ""last_updated"": 1525445072
                                    }, 
                                    ""metadata"": {
                                        ""timestamp"": 1525445184, 
                                        ""error"": null
                                    }
                                }";

            SerializeAndCompare<CmcTicker>(jsonContent, true).Should().BeTrue();
        }

        [Test]
        public void TickerSpecificCurrencyWithCurrencyConversionSerializedObjectsAreEqual()
        {
            string jsonContent = @"
                                {
                                    ""data"": {
                                        ""id"": 1, 
                                        ""name"": ""Bitcoin"", 
                                        ""symbol"": ""BTC"", 
                                        ""website_slug"": ""bitcoin"", 
                                        ""rank"": 1, 
                                        ""circulating_supply"": 17014912.0, 
                                        ""total_supply"": 17014912.0, 
                                        ""max_supply"": 21000000.0, 
                                        ""quotes"": {
                                                ""USD"": {
                                                    ""price"": 9670.07, 
                                                ""volume_24h"": 9386510000.0, 
                                                ""market_cap"": 164535390084.0, 
                                                ""percent_change_1h"": -0.11, 
                                                ""percent_change_24h"": 2.82, 
                                                ""percent_change_7d"": 4.26
                                                }, 
                                            ""EUR"": {
                                                    ""price"": 8079.91401913, 
                                                ""volume_24h"": 7842982909.09, 
                                                ""market_cap"": 137479026003.0, 
                                                ""percent_change_1h"": -0.11, 
                                                ""percent_change_24h"": 2.82, 
                                                ""percent_change_7d"": 4.26
                                            }
                                            }, 
                                        ""last_updated"": 1525445371
                                    }, 
                                    ""metadata"": {
                                        ""timestamp"": 1525445276, 
                                        ""error"": null
                                    }
                                }";

            SerializeAndCompare<CmcTicker>(jsonContent, true).Should().BeTrue();
        }

        [Test]
        public void TickerSpecificCurrencyWithInvalidIdSerializedObjectsAreEqual()
        {
            string jsonContent = @"
                                {
                                    ""data"": null, 
                                    ""metadata"": {
                                        ""timestamp"": 1525137187, 
                                        ""error"": ""id not found""
                                    }
                                }";

            SerializeAndCompare<CmcTicker>(jsonContent, true).Should().BeTrue();
        }

        [Test]
        public void GlobalDataWithoutCurrencyConversionSerializedObjectsAreEqual()
        {
            string jsonContent = @"
                            {
                                ""data"": {
                                    ""active_cryptocurrencies"": 1610, 
                                    ""active_markets"": 10686, 
                                    ""bitcoin_percentage_of_market_cap"": 36.1, 
                                    ""quotes"": {
                                            ""USD"": {
                                                ""total_market_cap"": 455043058262.0, 
                                                ""total_volume_24h"": 32157638321.0
                                            }
                                        }, 
                                    ""last_updated"": 1525387472
                                }, 
                                ""metadata"": {
                                    ""timestamp"": 1525387517, 
                                    ""error"": null
                                }
                            }";

            SerializeAndCompare<CmcGlobalData>(jsonContent, true).Should().BeTrue();
        }

        [Test]
        public void GlobalDataWithCurrencyConversionSerializedObjectsAreEqual()
        {
            string jsonContent = @"
                            {
                                ""data"": {
                                    ""active_cryptocurrencies"": 1610, 
                                    ""active_markets"": 10699, 
                                    ""bitcoin_percentage_of_market_cap"": 36.13, 
                                    ""quotes"": {
                                            ""USD"": {
                                                ""total_market_cap"": 455122748778.0, 
                                            ""total_volume_24h"": 30441476759.0
                                            }, 
                                        ""EUR"": {
                                                ""total_market_cap"": 380281908846.0, 
                                            ""total_volume_24h"": 25435649880.0
                                        }
                                        }, 
                                    ""last_updated"": 1525442072
                                }, 
                                ""metadata"": {
                                    ""timestamp"": 1525442013, 
                                    ""error"": null
                                }
                            }";

            SerializeAndCompare<CmcGlobalData>(jsonContent, true).Should().BeTrue();
        }
    }
}
