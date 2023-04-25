﻿using System;
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

       
        public const string GetBudgetVsActualVriance = "[dbo].[GET_BUDGET_VARIANNCE]";
        public const string GetRevenueByProductByDays = "[dbo].[RevenueByProduct]";
        public const string GetRevenueVsBduget = "[dbo].[RevenueAndBudgetByMonth]";
        public const string GetTranacionByHours = "[dbo].[GET_TRANSACTION_BY_HOURS]";
        public const string GetTransactonByDays = "[dbo].[GET_TRANSACTION_BY_DAYS]";
        public const string GetTransactonMonths = "[dbo].[GET_TRANSACTION_MONTHS]";


        public const string GetRevenueByDay = "[dbo].[RevenueByDay]";
        public const string GetRevenueByMonth = "[dbo].[RevenueByMonth]";

    }
}
