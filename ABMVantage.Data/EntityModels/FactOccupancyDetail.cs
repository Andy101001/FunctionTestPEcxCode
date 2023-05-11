namespace ABMVantage.Data.EntityModels
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class FactOccupancyDetail
    {
        [Key]
        [JsonProperty("FactOccupancyDetailId")]
        public string FactOccupancyDetailId { get; set;}

        [JsonProperty("FacilityId")]
        public string? FacilityId { get; set; }

        [JsonProperty("LevelId")]
        public string? LevelId { get; set; }

        [JsonProperty("ProductId")]
        public string? ProductId { get; set; }

        [JsonProperty("BeginningOfHour")]
        public DateTime BeginningOfHour { get; set; }

        [JsonProperty("OccupancyForHour")]
        public string? OccupancyForHour { get; set; }

    }
}