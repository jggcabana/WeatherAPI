# WeatherAPI - Summary of Submission
- A simple .NET 7 WebAPI application that serves weather data.
- Uses the following 3rd Party APIs to get data:
	- [Rapid Weather API](https://rapidapi.com/weatherapi/api/weatherapi-com/)
	- [City API - API Ninjas](https://api-ninjas.com/api/city)
- The app uses both APIs in tandem to first request for cities, then to request weather data per city.
- ***IMPORTANT***: Do not use the default Swagger page for requests. Swagger is very inefficient in loading large datasets (weather data can grow very large). Use Postman instead.

### Features
- App uses `OutputCaching` for caching the server responses, and the in-built `IMemoryCache` to cache 3rd party API responses.
- Parallelization of API calls, when 3rd party API endpoints don't support requests with ranged parameters.
- OpenAPI documentation (See swagger page).

### Local Development Setup
- In order to run the API, you will need API keys for our 3rd APIs. Create an account and subscribe to the free tier to obtain API keys for the following:
	- 	 [Rapid Weather API](https://rapidapi.com/weatherapi/api/weatherapi-com/)
	- [City API - API Ninjas](https://api-ninjas.com/api/city)
- Due to security reasons, storing API keys in the solution is discouraged, so we need to manage they keys as secrets.
- In the root folder of the application, configure user secrets using the following command(s):

```
dotnet user-secrets clear --project "./Weather.WebAPI"
dotnet user-secrets init --project "./Weather.WebAPI"
dotnet user-secrets set "RapidWeatherApiKey" "{your-rapid-weather-api-key}" --project "./Weather.WebAPI"
dotnet user-secrets set "ApiNinjaCityApiKey" "{your-api-ninja-api-key}" --project "./Weather.WebAPI"
```

- Be sure to replace the values for the API keys you obtained from the 3rd party APIs.

### Final Developer Notes
- Due to time constraints (as well as competing deadlines), the solution has still a lot of room to improve. Given more time, I would have seen to the following implementations:
	- Structured logging (preferrably via serilog)
	- Better caching implementation
	- Unit tests (although most of the code has minimal business logic, since it is simply made up of calls to 3rd party APIs).