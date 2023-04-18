using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.DashboardFunctionModels
{
    public class DashboardDailyReservationCountByHour
    {
        public DashboardDailyReservationCountByHour() 
        {
            ReservationsByHour = new List<HourlyReservationCount>();
        }
        public IEnumerable<HourlyReservationCount> ReservationsByHour { get; set; }
    }

    public class HourlyReservationCount
    {
        public string ReservationTime { get; set; }
        public IEnumerable<ReservationsByProduct> Data { get; set; }
    }

    public class ReservationsByProduct
    {
        public string Product{ get; set; }
        public int NoOfReservations { get; set; }
    }
}
