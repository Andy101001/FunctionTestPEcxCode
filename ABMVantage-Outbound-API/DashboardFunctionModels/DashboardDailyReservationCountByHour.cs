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
        public string Hour { get; set; }
        public int Reservations { get; set; }
    }

    /// <summary>
    /// Input Parameters for hourly reservation
    /// </summary>
    public class HourlyReservationParameters
    {
        public DateTime? calculationDate { get; set; }
        public string facilityId { get; set; }
        public string levelId { get; set; }
        public string parkingProductId { get; set; }
        
    }
}
