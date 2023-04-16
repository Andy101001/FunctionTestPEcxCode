using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Utils
{
    public class StoredProcs
    {
        public const string GetTotalOccRevenue = "[BASE].[GET_TOTAL_OCCUPANCY_BY_REVENUE]";



        //Reservations
        
        public const string GetHourlyReservations = "[BASE].[GET_RESERVATIONS_HOURLY]";
        public const string GetDailyReservations = "[BASE].[GET_RESERVATIONS_DAILY]";
        public const string GetMonthlyReservations = "[BASE].[GET_RESERVATIONS_MONTHLY]";
        public const string GetReservationsAvgTkt = "[BASE].[GET_RESERVATIONS_AVGTKT]";
    }
}
