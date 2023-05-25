namespace ABMVantage.Data.Utils
{
    public class StoredProcs
    {
        //Filter
        public const string GetFiltersData = "[BASE].[GET_FILTERS_BY_BUS]";
        public const string GetProductsData = "[BASE].[GET_PRODUCTS]";
        public const string GetStgFilterData = "[DBO].[GET_FILTER_DATA]";

        

        //Insights
        public const string GetDailyAverageOccupancy = "[DBO].[GET_OCCUPANCY_AVERAGE_FOR_DAY]";//done
        public const string GetReservationsByHour = "[DBO].[GET_RESERVATIONS_BY_HOUR]";//done
        public const string GetMonthlyOccupancy = "[DBO].[GET_OCCUPANCY_BY_MONTH]";//done
        public const string GetMonthlyAverageTicketValue = "[DBO].[GET_TICKET_AVERAGE_VALUE_BY_MONTH_AND_PRODUCT]";//done
        public const string GetDailyTransactionCount = "[DBO].[GET_TRANSACTIONS_FOR_DAY]";//done
        public const string GetMonthlyRevenueAndBudget = "[DBO].[GET_REVENUE_AND_BUDGET_BY_MONTH]";//done
        public const string GetDailyTotalRevenue = "[DBO].[GET_REVENUE_FOR_DAY]"; //done
        public const string GetMonthlyTransactions = "[DBO].[GET_TRANSACTIONS_BY_MONTH_AND_PRODUCT]";//done



        //Occupancy
        public const string GetTotalOccRevenue = "[BASE].[GET_TOTAL_OCCUPANCY_BY_REVENUE]"; //done
        public const string GetWeeklyOccByDuration = "[BASE].[GET_WEEKLY_OCCUPANCY_BY_DURATION]";//done
        public const string GetOccCurrent = "[BASE].[GET_CURRENT_OCCUPANCY]";//done
        public const string GetAvgMonthlyOccVsDuration = "[BASE].[GET_AVG_MONTHLY_OCC_VS_DURATION]";//done
        public const string GetYearlyOccupancy = "[BASE].[GET_YEARLY_OCCUPANCY]";//done

        //Reservations//Done

        public const string GetHourlyReservations = "[BASE].[GET_RESERVATIONS_HOURLY]"; //done
        public const string GetDailyReservations = "[BASE].[GET_RESERVATIONS_DAILY]"; //done
        public const string GetMonthlyReservations = "[BASE].[GET_RESERVATIONS_MONTHLY]";//done
        public const string GetReservationsAvgTkt = "[BASE].[GET_RESERVATIONS_AVGTKT]";//done

        //Transaction

       
        public const string GetBudgetVsActualVriance = "[dbo].[GET_BUDGET_VARIANNCE]"; //done
        public const string GetRevenueByProductByDays = "[dbo].[RevenueByProduct]"; //done
        public const string GetRevenueVsBduget = "[dbo].[RevenueAndBudgetByMonth]";//done
        public const string GetTranacionByHours = "[dbo].[GET_TRANSACTION_BY_HOURS]"; //done
        public const string GetTransactonByDays = "[dbo].[GET_TRANSACTION_BY_DAYS]"; //done
        public const string GetTransactonMonths = "[dbo].[GET_TRANSACTION_MONTHS]"; //done

        //Cache
        
        public const string GetCacheTransactonByDays = "[DBO].[CACHE_GET_TRANSACTION_BY_DAYS]";


        public const string GetRevenueByDay = "[dbo].[RevenueByDay]"; //done
        public const string GetRevenueByMonth = "[dbo].[RevenueByMonth]"; //done

    }
}
