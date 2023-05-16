using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels
{
    public class DimParkingSpaceCount
    {
        [Key]
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("FacilityId")]
        public string FacilityId { get; set; }
        [JsonProperty("LevelId")]
        public string LevelId { get; set; }
        [JsonProperty("ParkingProductId")]
        public int ParkingProductId { get; set; }
        [JsonProperty("ProductName")]
        public string ProductName { get; set; }
        [JsonProperty("ParkingSpaceCount")]
        public int ParkingSpaceCount { get; set; }
    }
}
