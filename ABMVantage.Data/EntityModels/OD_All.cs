using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels
{
    public class OD_All
    {
        [Key]
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("FacilityId")]
        public string? FacilityId { get; set; }

        [JsonProperty("LevelId")]
        public string? LevelId { get; set; }

        [JsonProperty("ProductId")]
        public int? ProductId { get; set; }

        [JsonProperty("OccupancyId")]
        public string? OccupancyId { get; set; }

        [JsonProperty("Duration")]
        public string? Duration { get; set; }

        [JsonProperty("OccupancyEntryDateTimeUtc")]
        public DateTime? OccupancyEntryDateTimeUtc { get; set; }

        [JsonProperty("OccupancyExitDateTimeUtc")]
        public DateTime? OccupancyExitDateTimeUtc { get; set; }
    }
}
