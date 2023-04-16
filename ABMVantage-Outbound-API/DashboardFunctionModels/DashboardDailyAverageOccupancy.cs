using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.DashboardFunctionModels
{
    public class DashboardDailyAverageOccupancy
    {
        [JsonProperty("averageDailyOccupancyInteger")]
        public int AverageDailyOccupancyInteger { get; set; }

        [JsonProperty("averageDailyOccupancyPercentage")]
        public int AverageDailyOccupancyPercentage { get; set; }
    }
}
