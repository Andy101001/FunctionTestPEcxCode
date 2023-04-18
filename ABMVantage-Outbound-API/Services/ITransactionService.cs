﻿using ABMVantage.Data.Models;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public interface ITransactionService
    {
        Task<int> GetDailyTransactiontCountAsync(DateTime? tranactionDate, string? facilityId, string? levelId, string? parkingProductId);
        Task<decimal> GetDailyTotalRevenueAsync(FilterParam filterParameters);
        Task<DashboardDailyAverageOccupancy> GetDailyAverageOccupancy(FilterParam filterParameters);
        Task<DashboardMonthlyTransactionCount> GetMonthlyTransactionCountAsync(FilterParam filterParameters);
        Task<IList<RevenueAndBudget>> GetMonthlyRevenueAndBudget(DateTime? startDate, DateTime? endDate, string? facilityId, string? levelId, string parkingProductId);
    }
}
