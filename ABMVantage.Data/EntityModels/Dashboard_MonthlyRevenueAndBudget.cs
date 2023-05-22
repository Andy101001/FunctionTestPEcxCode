using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels
{
    public class Dashboard_MonthlyRevenueAndBudget
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

        [JsonProperty("FirstDayofMonth")]
        public DateTime FirstDayofMonth { get; set; }

        [JsonProperty("Revenue")]
        public int Revenue { get; set; }

        [JsonProperty("BudgetedRevenue")]
        public int BudgetedRevenue { get; set; }

    }
}
