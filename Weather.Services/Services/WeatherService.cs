using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Weather.Services.ApiSchemas.ApiNinja;
using Weather.Services.Constants;
using Weather.Services.Interfaces;

namespace Weather.Services.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _client;
        private readonly ICityService _cityService;

        public WeatherService(IHttpClientFactory httpClientFactory, ICityService cityService)
        {
            _client = httpClientFactory.CreateClient("Rapid.WeatherApi");
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
        }

        public async Task<dynamic?> GetWeatherAsync(string projection, double latitude, double longitude)
        {
            using var response = await _client.GetAsync($"{projection}.json?q={latitude},{longitude}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<dynamic>(data);
        }

        public async Task<dynamic?> GetWeatherAsync(string projection, string name)
        {
            using var response = await _client.GetAsync($"{projection}.json?q={name}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<dynamic>(data);
        }

        public async Task<IEnumerable<dynamic>> GetWeatherByCityAsync(string projection, int limit = 10, string country = "us", int daysFromNow = 3)
        {
            var cities = await _cityService.GetCitiesAsync(limit, country);

            // execute API calls in parallel
            // by projecting each item into an awaitable task 
            var tasks = cities.Select(async x =>
            {
                string uri = "";
                switch (projection)
                {
                    case WeatherProjectionType.Current:
                        uri = $"current.json?q={x.Name}";
                        break;
                    case WeatherProjectionType.Forecast:
                        uri = $"forecast.json?q={x.Name}&days={daysFromNow}";
                        break;
                    default:
                        break;
                }

                using var response = await _client.GetAsync(uri);
                return await response.Content.ReadAsStringAsync();
            });

            var output = new List<dynamic>();

            // wait for all tasks to get a response
            await Task.WhenAll(tasks);
            foreach (var task in tasks)
            {
                var res = JsonSerializer.Deserialize<dynamic>(task.Result);
                output.Add(res);
            }

            return output;
        }
    }
}
