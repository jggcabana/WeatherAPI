using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Weather.Services.ApiSchemas.ApiNinja;
using Weather.Services.Interfaces;

namespace Weather.Services.Services
{
    public class CityService : ICityService
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;
        
        public CityService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, ILogger<CityService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _client = httpClientFactory.CreateClient("ApiNinja.CityApi");
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(int limit = 10, string country = "us")
        {
            try
            {
                var cacheKey = $"CitiesByPop-{limit}-{country}";
                return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromDays(7);
                    return await GetCitiesAsync(false, limit, country);
                });
            }
            catch (HttpRequestException e)
            {
                // for some reason, api-ninjas is returning 502
                if(e.StatusCode == System.Net.HttpStatusCode.BadGateway)
                {
                    _logger.LogWarning("Cities API returns 502, using static city data instead...");
                    return GetCitiesStatic(limit);
                }

                throw;
            }
        }

        private async Task<IEnumerable<City>> GetCitiesAsync(bool isCached, int limit = 10, string country = "us")
        {
            if (isCached)
                return null;

            using var response = await _client.GetAsync($"v1/city?country={country}&limit={limit}");
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<City>>(stringResult);
        }

        private static List<City> GetCitiesStatic(int limit)
        {
            var cityList = new List<City>();

            cityList.Add(new City { Name = "New York" });
            cityList.Add(new City { Name = "Los Angeles" });
            cityList.Add(new City { Name = "Chicago" });
            cityList.Add(new City { Name = "Houston" });
            cityList.Add(new City { Name = "Phoenix" });
            cityList.Add(new City { Name = "Philadelphia" });
            cityList.Add(new City { Name = "San Antonio" });
            cityList.Add(new City { Name = "San Diego" });
            cityList.Add(new City { Name = "Dallas" });
            cityList.Add(new City { Name = "Austin" });

            return cityList.Take(limit).ToList();
        }
    }
}
