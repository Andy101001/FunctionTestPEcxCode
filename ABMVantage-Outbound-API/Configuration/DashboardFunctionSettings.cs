using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Configuration
{
    public class DashboardFunctionSettings
    {
        public DashboardFunctionSettings() 
        {
            MinimumValidCalculationDate = new DateTime(1900, 1, 1);
            MonthlyTransactionCountInterval = 13;
            MonthlyParkingOccupancyInterval = 6;
            MonthlyAverageTicketValueInterval = 13;
            DailyReservationCountByHourInteral = 1;
        }
        public int MonthlyTransactionCountInterval { get; set; }
        public DateTime MinimumValidCalculationDate { get; set; }
        public int MonthlyParkingOccupancyInterval { get; set; }
        public int MonthlyAverageTicketValueInterval { get; set; }
        public int DailyReservationCountByHourInteral { get; set; }
    }
}
