using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    
    public class Location
    {
        [JsonProperty("LocationId")]
        public string? LocationId { get; set; }

        [JsonProperty("Bu")]
        public string? Bu { get; set; }

        [JsonProperty("Latitude")]
        public string? Latitude { get; set; }

        [JsonProperty("Longitude")]
        public string? Longitude { get; set; }

        [JsonProperty("LocationAddress")]
        public string? LocationAddress { get; set; }

        [JsonProperty("Type")]
        public string? Type { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("Facilities")]
        public List<Facilities> Facilities { get; set; } = new List<Facilities>();

        //[JsonProperty("Levels")]
        //public IList<Level> Levels { get; set; }

        //[JsonProperty("Products")]
        //public IList<Product> Products { get; set; }

        //[JsonProperty("FacilityTypes")]
        //public IList<FacilityType> FacilityTypes { get; set; }
    }
}
