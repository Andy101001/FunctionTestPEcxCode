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
        public int ProductId { get; set; }

        [JsonProperty("occupancyId")]
        public string? occupancyId { get; set; }

        [JsonProperty("Duration")]
        public string? Duration { get; set; }

        [JsonProperty("OccupancyEntryDate")]
        public DateTime OccupancyEntryDate { get; set; }

        [JsonProperty("OccupancyExitDate")]
        public DateTime OccupancyExitDate { get; set; }
    }
}
