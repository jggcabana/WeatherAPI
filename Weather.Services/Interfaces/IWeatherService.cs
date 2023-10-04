using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Services.ApiSchemas.ApiNinja;

namespace Weather.Services.Interfaces
{
    public interface IWeatherService
    {
        public Task<dynamic> GetWeatherAsync(string projection, double latitude, double longitude);

        public Task<dynamic> GetWeatherAsync(string projection, string name);

        public Task<IEnumerable<dynamic>> GetWeatherByCityAsync(string projection, int limit = 10, string country = "us", int daysFromNow = 3);
    }
}
