using Weather.Services.ApiSchemas.ApiNinja;

namespace Weather.Services.Interfaces
{
    public interface ICityService
    {
        public Task<IEnumerable<City>> GetCitiesAsync(int limit = 10, string country = "us");
    }
}