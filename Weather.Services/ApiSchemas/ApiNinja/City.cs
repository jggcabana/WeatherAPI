using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Weather.Services.ApiSchemas.ApiNinja
{
    public class City
    {
        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Country { get; set; }

        public int Population { get; set; }

        [JsonPropertyName("is_capital")]
        public bool IsCapital { get; set; }

    }
}
