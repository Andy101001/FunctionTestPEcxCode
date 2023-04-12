using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    public class Facilities
    {
        [JsonProperty("FacilityId")]
        public string? FacilityId { get; set; }

        [JsonProperty("FacilityName")]
        public string? FacilityName { get; set; }

        [JsonProperty("FacilityType")]
        public string? FacilityType { get; set; }

        [JsonProperty("AreaType")]
        public string? AreaType { get; set; }

        [JsonProperty("IncludeBusinessUnitNumber")]
        public bool? IncludeBuNumber { get; set; }

        [JsonProperty("Latitude")]
        public string? Latitude { get; set; }

        [JsonProperty("Longitude")]
        public string? Longitude { get; set; }

        [JsonProperty("FacilityTypeId")]
        public string? FacilityTypeId { get; set; }

        [JsonProperty("TotalSpaces")]
        public string? TotalSpaces { get; set; }

    }
}
