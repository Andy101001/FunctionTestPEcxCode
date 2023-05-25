using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels
{
    public class RevenueByProductByDay
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

        [JsonProperty("Product")]
        public string? Product { get; set; }

        [JsonProperty("Revenue")]
        public decimal? Revenue { get; set; }

        [JsonProperty("BudgetedRevenue")]
        public decimal? BudgetedRevenue { get; set; }

        [JsonProperty("TransactionDate")]
        public DateTime? TransactionDate { get; set; }

        [JsonProperty("TransactionId")]
        public DateTime? TransactionId { get; set; }
    }
}
