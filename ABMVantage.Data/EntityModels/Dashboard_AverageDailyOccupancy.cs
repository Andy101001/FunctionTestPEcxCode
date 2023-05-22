using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels
{
    public class Dashboard_AverageDailyOccupancy
    {
        [Key]
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("FacilityId")]
        public string? FacilityId { get; set; }

        [JsonProperty("LevelId")]
        public string? LevelId { get; set; }

        [JsonProperty("ProductId")]
        public int ProductId { get; set; }

        [JsonProperty("TOTAL_OCCUPIED_PARKING_SPOT_HOURS_FOR_DAY")]
        public int TOTAL_OCCUPIED_PARKING_SPOT_HOURS_FOR_DAY { get; set; }

        [JsonProperty("Day")]
        public DateTime Day { get; set; }

        [JsonProperty("ParkingSpaceCount")]
        public int ParkingSpaceCount { get; set; }
        

    }
}
