# Fbo.CryptoMarketData - CoinMarketCap Client Documentation
	
	
Every call to the API using "CmcClient" returns a "CmcResponse" object.

This object has three properties:

- Success: a boolean value indicating whether the response from the API was successful or not;
	
- Data: The data returned from the API. The type of this object varies according to the endpoint.
	
- Metadata: a CmcMetadata object containing metadata returned from the API. The content of this object varies according to the endpoint.
	
	
Basic Usage:
	
	// Creates a new instance of the client.
	var cmcClient = new CmcClient();
	
	// Gets all currencies listed on CoinMarketCap using endpoint: https://api.coinmarketcap.com/v2/listings/
	// CmcResponse.Data type will be "CmcCurrency[]".
	
	// Synchronous call.
	var response = cmcClient.GetCurrencies();
	
	// Asynchronous call
	var response = await cmcClient.GetCurrenciesAsync()

	...

	if (response.Success)
	{
		// do something with response.Data
		// do something with response.Metadata
	}
	else
	{
		// do something
		// can get the error message with response.Metadata.ErrorMessage
	}
	
	
	

Additional Call Examples:

	// Creates a new instance of the client.
	var cmcClient = new CmcClient();
	
	// Ticker by Id
	cmcClient.GetTickerById(1, "EUR");	// With "EUR" (Euro) conversion.
	cmcClient.GetTickerById(1);			// Without conversion.

	// Get Tickers. CoinMarketCap API returns a max of 100 tickers per call.
	cmcClient.GetTickersInRange(1, 10, "BTC");	// Tickers ranked betweeen 01 and 10 (inclusive) with "BTC" (bitcoin) conversion.
	cmcClient.GetTickersInRange(100, 150);		// Tickers ranked between 100 and 150 (inclusive) without conversion.
	
	// Gets all tickers, abstracting the 100 tickers limit imposed by the API.
	cmcClient.GetAllTickers("EUR");		// With "EUR" (Euro) conversion.
	cmcClient.GetAllTickers();			// Without conversion.
	
	// Gets cryptocurrency global market data.
	cmcClient.GetGlobalData("EUR");		// With "EUR" (Euro) conversion.
	cmcClient.GetGlobalData();			// Without conversion.

