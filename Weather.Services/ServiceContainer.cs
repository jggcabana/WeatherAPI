using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Services.Interfaces;
using Weather.Services.Services;

namespace Weather.Services
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddWeatherServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddScoped<ICityService, CityService>();

            var rapidApiKey = config["RapidWeatherApiKey"];
            if(string.IsNullOrEmpty(rapidApiKey))
                throw new ArgumentNullException(nameof(rapidApiKey));

            services.AddHttpClient("Rapid.WeatherApi", client =>
            {
                // TODO: Move API Key into secrets.json
                client.BaseAddress = new Uri("https://weatherapi-com.p.rapidapi.com");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", rapidApiKey);
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com");
            });

            var apiNinjaApiKey = config["ApiNinjaCityApiKey"];
            if (string.IsNullOrEmpty(apiNinjaApiKey))
                throw new ArgumentNullException(nameof(apiNinjaApiKey));

            services.AddHttpClient("ApiNinja.CityApi", client =>
            {
                // TODO: Move API Key into secrets.json
                client.BaseAddress = new Uri("https://api.api-ninjas.com");
                client.DefaultRequestHeaders.Add("X-Api-Key", apiNinjaApiKey);
            });

            return services;
        }
    }
}
