using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Utils
{
    public class StoredProcs
    {
        //Filter
        public const string GetFiltersData = "[BASE].[GET_FILTERS_BY_BUS]";
        public const string GetProductsData = "[BASE].[GET_PRODUCTS]";

        public const string GetDailyAverageOccupancy = "[DBO].[GET_OCCUPANCY_AVERAGE_FOR_DAY]";



        //Occupancy
        public const string GetTotalOccRevenue = "[BASE].[GET_TOTAL_OCCUPANCY_BY_REVENUE]";
        public const string GetWeeklyOccByDuration = "[BASE].[GET_WEEKLY_OCCUPANCY_BY_DURATION]";
        public const string GetOccCurrent = "[BASE].[GET_CURRENT_OCCUPANCY]";
        public const string GetAvgMonthlyOccVsDuration = "[BASE].[GET_AVG_MONTHLY_OCC_VS_DURATION]";
        public const string GetYearlyOccupancy = "[BASE].[GET_YEARLY_OCCUPANCY]";

        //Reservations

        public const string GetHourlyReservations = "[BASE].[GET_RESERVATIONS_HOURLY]";
        public const string GetDailyReservations = "[BASE].[GET_RESERVATIONS_DAILY]";
        public const string GetMonthlyReservations = "[BASE].[GET_RESERVATIONS_MONTHLY]";
        public const string GetReservationsAvgTkt = "[BASE].[GET_RESERVATIONS_AVGTKT]";

        //Transaction

        //public const string GetBudgetVariance = "[BASE].[GET_BUDGET_VARIANNCE]"; //dupliate
        public const string GetBudgetVsActualVriance = "[BASE].[GET_BUDGET_VARIANNCE]"; //done
        public const string GetRevenueByProductByDays = "[BASE].[GET_REVENUE_BY_PRODUCT]"; //done
        public const string GetRevenueVsBduget = "[BASE].[GET_REVENUE_VS_BUDGET]"; //done
        public const string GetTranacionByHours = "[BASE].[GET_TRANSACTION_BY_HOURS]"; //done
        public const string GetTransactonByDays = "[BASE].[GET_TRANSACTION_BY_DAYS]"; //done
        public const string GetTransactonMonths = "[BASE].[GET_TRANSACTION_MONTHS]"; //done


        public const string GetRevenueByDay = "[BASE].[RevenueByDay]";//done
        public const string GetRevenueByMonth = "[BASE].[RevenueByMonth]";//done 

    }
}
