using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels
{
    public class InsightsMonthlyParkingOccupancy
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

        [JsonProperty("ParkingSpaceCount")]
        public int ParkingSpaceCount { get; set; }

        [JsonProperty("NoOFDaysInMonth")]
        public int NoOFDaysInMonth { get; set; }

        [JsonProperty("FirstDayOfMonth")]
        public DateTime FirstDayOfMonth { get; set; }

        [JsonProperty("TotalOccupancy")]
        public int TotalOccupancy { get; set; }

    }
}
