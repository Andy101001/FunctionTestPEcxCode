using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    public class FacilityType
    {
        [JsonProperty("FacilityTypeId")]
        public string? FacilityTypeId { get; set; }

        [JsonProperty("DimFacilityFacilityId")]
        public string? DimFacilityFacilityId { get; set; }

        [JsonProperty("FacilityType")]
        public string? FacilitiesType { get; set; }

        [JsonProperty("FacilityTypeDescription")]
        public string? FacilityTypeDescription { get;  set; }
    }
}
