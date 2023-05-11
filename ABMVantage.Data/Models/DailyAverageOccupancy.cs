namespace ABMVantage.Data.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;


    public class DailyAverageOccupancy
    {
        [JsonProperty("averageDailyOccupancyInteger")]
        public int AverageDailyOccupancyInteger { get; set; }

        [JsonProperty("averageDailyOccupancyPercentage")]
        public int AverageDailyOccupancyPercentage { get; set; }
    }
}
