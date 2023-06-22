﻿using ABMVantage.Data.Models;
using ABMVantage.Data.Models.DashboardModels;

namespace ABMVantage.Data.Interfaces
{
    public interface IInsightsService
    {
        Task<DailyAverageOccupancy> GetDailyAverageOccupancy(FilterParam? filterParameters);
        Task<Revenue> GetDailyTotalRevenueAsync(FilterParam filterParameters);
        Task<Transaction> GetDailyTransactiontCountAsync(FilterParam filterParameters);
        Task<DashboardDailyReservationCountByHour> GetHourlyReservationsByProduct(FilterParam filterParameters);
        Task<DashboardMonthlyRevenueAndBudget> GetMonthlyRevenueAndBudget(FilterParam filterParameters);
        Task<DashboardMonthlyParkingOccupancy> GetMonthlyParkingOccupancyAsync(FilterParam filterParameters);
        Task<DashboardMonthlyTransactionCount> GetMonthlyTransactionCountAsync(FilterParam filterParameters);
        Task<DashboardMonthlyAverageTicketValue> GetMonthlyAverageTicketValue(FilterParam filterParameter);
    }
}