using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.DashboardFunctionModels
{
    public class DashboardMonthlyParkingOccupancy
    {
        public IEnumerable<ParkingOccupancy> MonthlyParkingOccupancy { get; set; }
    }

    public class ParkingOccupancy
    {
        public string Month { get; set; }
        public int OccupancyInteger { get; set; }
        public decimal OccupancyPercentage { get; set; }
        public int PreviousYearOccupancyInteger { get; set; }
        public decimal PreviousYearOccupancyPercentage { get; set; }
    }
}
