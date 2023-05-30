using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Configuration
{
    public class InsightsServiceSettings
    {
        public InsightsServiceSettings() 
        {
            MonthlyTransactionCountDataRange= 13;
            MonthlyParkingOccupancyDataRange = 6;
            MonthlyAverageTicketValueDataRange = 13;
            DailyReservationCountByHourDataRange = 1;
            MonthlyOccupancyDataRange = 6;
        }

        public int MonthlyTransactionCountDataRange { get; set; }
        public int MonthlyParkingOccupancyDataRange { get; set; }
        public int MonthlyAverageTicketValueDataRange { get; set; }
        public int DailyReservationCountByHourDataRange { get; set; }
        public int MonthlyOccupancyDataRange { get; set; }
    }


}
