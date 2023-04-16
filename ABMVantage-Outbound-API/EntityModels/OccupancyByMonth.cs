using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    public class OccupancyByMonth
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int OccupancyInteger { get; set; }
        public decimal OccupancyPercentage { get; set; }

    }
}
