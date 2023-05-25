﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels
{
    public class InsightsMonthlyTransaction
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

        [JsonProperty("ProdutName")]
        public string? ProdutName { get; set; }

        [JsonProperty("FirstDayOfMonth")]
        public DateTime FirstDayOfMonth { get; set; }

        [JsonProperty("TransactionCount")]
        public int TransactionCount { get; set; }
       
    }
}
