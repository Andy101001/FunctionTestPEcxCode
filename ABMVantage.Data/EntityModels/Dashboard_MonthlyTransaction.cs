using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels
{
    public class Dashboard_MonthlyTransaction
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

        [JsonProperty("ProductName")]
        public string? ProductName { get; set; }

        [JsonProperty("FirstDayofMonth")]
        public DateTime FirstDayofMonth { get; set; }

        [JsonProperty("TransactionCount")]
        public int TransactionCount { get; set; }
       
    }
}
