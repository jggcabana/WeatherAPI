using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Weather.Services.Interfaces;
using Weather.WebAPI.ViewModels.Response;

namespace Weather.WebAPI.Controllers
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
        }

        /// <summary>
        /// Gets the weather for the given coordinates.
        /// </summary>
        /// <param name="projection">The projection of the weather ('current' or 'forecast').</param>
        /// <param name="latitude">Latitude coordinate of the location.</param>
        /// <param name="longitude">Longitude coordinate of the location.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{projection}")]
        [OutputCache]
        public async Task<IActionResult> Test(string projection, [FromQuery] double latitude, [FromQuery] double longitude)
        {
            var result = await _weatherService.GetWeatherAsync(projection, latitude, longitude);
            return Ok(new BaseResponse
            {
                Data = result,
            });
        }

        /// <summary>
        /// Gets the projected weather data for the cities under a country, ranked by an arbitrary attribute.
        /// </summary>
        /// <param name="projection">The projection of the weather ('current' or 'forecast').</param>
        /// <param name="country">The country to query cities from.</param>
        /// <param name="toRank">Number of cities to include (i.e., to rank 7, 10, etc).</param>
        /// <param name="daysFromNow">Used for forecasts. Number of days to include the forecast for.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{projection}/{country}/cities/top/by-population")]
        [OutputCache]
        public async Task<IActionResult> GetCurrentWeatherByCity(string projection, string country, [FromQuery] int toRank = 3, [FromQuery] int daysFromNow = 1)
        {
            var result = await _weatherService.GetWeatherByCityAsync(projection, toRank, country, daysFromNow);
            return Ok(new BaseResponse { Data = result, });
        }
    }
}
