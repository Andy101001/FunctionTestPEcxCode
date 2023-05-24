using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels
{
    public class InsightsTotalRevenue
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

        [JsonProperty("Day")]
        public DateTime Day { get; set; }

        [JsonProperty("TotalRevenue")]
        public decimal TotalRevenue { get; set; }

    }
}
