﻿namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.DataAccess;
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using Microsoft.Azure.Cosmos.Serialization.HybridRow;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public class RevenueAndTransactionService : ServiceBase, IRevenueAndTransactionService
    {
        private readonly ILogger<RevenueAndTransactionService> _logger;
        private readonly IRedisCachingService _cache;
        private readonly IDbContextFactory<SqlDataContextVTG> _sqlDataContextVTG;
        

        public RevenueAndTransactionService(ILoggerFactory loggerFactory, IRepository repository, IRedisCachingService cache, IDbContextFactory<SqlDataContextVTG> sqlDataContextVTG)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<RevenueAndTransactionService>();
            _repository = repository;
            _sqlDataContextVTG = sqlDataContextVTG;
            _cache = cache;
        }

        #region Public Methods

        public async Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam parameters)
        {
            IList<DailyTransaction> dailyTransactionsList = new List<DailyTransaction>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Requirement is to fetch 7 days of Data from Start Date
                parameters.ToDate = parameters.FromDate.AddDays(7);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                  && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                  && products!.Contains(x.ProductId)
                  && x.TransactionDate >= parameters.FromDate && x.TransactionDate <= parameters.ToDate).AsEnumerable();

                dailyTransactionsList = result.GroupBy(x => new { x.TransactionDate.DayOfWeek, TransactionDate = x.TransactionDate.Date }).Select(g =>
                        new DailyTransaction
                        {
                            TransactionDate= g.Key.TransactionDate,
                            WeekDay = g.Key.DayOfWeek.ToString(),
                            NoOfTransactions = Convert.ToDecimal(g.Count())
                        }).OrderBy(x => x.TransactionDate).ToList();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dailyTransactionsList;
        }

        public async Task<IEnumerable<CurrentTransaction>> GetTransacionByHours(FilterParam parameters)
        {
            IList<CurrentTransaction> transactionsByHoursList = new List<CurrentTransaction>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Requirement is only to get last 24 Hours of DATA from Start Date
                parameters.ToDate = parameters.FromDate.AddDays(1);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                  && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                  && products!.Contains(x.ProductId)
                  && x.TransactionDate >= parameters.FromDate && x.TransactionDate <= parameters.ToDate).AsEnumerable();

                var result1 = result.GroupBy(x => new { Hour = new DateTime(x.TransactionDate.Year, x.TransactionDate.Month, x.TransactionDate.Day, x.TransactionDate.Hour, 0,0) }).Select(g =>
                          new CurrentTransaction
                          {
                              TimeOfDay = g.Key.Hour.TimeOfDay,
                              NoOfTransactions = Convert.ToDecimal(g.Count())

                          }).ToArray();

                transactionsByHoursList = result1.GroupBy(x => new { x.TimeOfDay }).Select(g =>
                      new CurrentTransaction
                      {
                          TimeOfDay = g.Key.TimeOfDay,
                          Time = DateTime.Today.Add(g.Key.TimeOfDay).ToString("hh:mm tt"),
                          NoOfTransactions = g.Sum(x => x.NoOfTransactions)
                      }).OrderBy(x=>x.TimeOfDay).ToList();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return transactionsByHoursList;
        }

        public async Task<IEnumerable<BudgetVariance>> GetBudgetVsActualVariance(FilterParam parameters)
        {
            IList<BudgetVariance> budgetVariance = new List<BudgetVariance>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();
                //Requirement: Show 13 month of data from todate
                var fromMonth= Convert.ToInt32(parameters.FromDate.ToString("yyyyMM"));
                var toMonth = Convert.ToInt32(parameters.FromDate.AddMonths(13).ToString("yyyyMM"));

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();

                var budgetVariance2 = sqlContext.RevenueBudgetVsActualVarianceSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                   && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                   && products!.Contains(x.ProductId) && x.MonthId >= fromMonth && x.MonthId <= toMonth)
                   .GroupBy(x => new { x.Month }).Select(g =>
                       new BudgetVariance
                       {
                           Month = g.Key.Month,
                           BgtVariance = g.Sum(x => x.Bgtvariance),
                       }
                       );

                budgetVariance = sqlContext.RevenueBudgetVsActualVarianceSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                    && products!.Contains(x.ProductId) && x.MonthId>=fromMonth && x.MonthId <= toMonth)
                    .GroupBy(x => new { x.Month }).Select(g =>
                        new BudgetVariance
                        {
                            Month = g.Key.Month,
                            BgtVariance = g.Sum(x => x.Bgtvariance),
                        }
                        ).ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return budgetVariance;
        }

        public async Task<IEnumerable<RevenueByDay>> GetRevenueByDays(FilterParam parameters)
        {
            IList<RevenueByDay> revenueByDayList = new List<RevenueByDay>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Requirement: show 7 days data
                parameters.ToDate = parameters.FromDate.AddDays(7);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                    && products!.Contains(x.ProductId)
                     && x.TransactionDate >= parameters.FromDate && x.TransactionDate < parameters.ToDate).AsEnumerable();

                revenueByDayList = result.GroupBy(x => new { Day = x.TransactionDate.Date }).Select(g =>
                          new RevenueByDay
                          {
                              Day = g.Key.Day,
                              WeekDay = g.Key.Day.DayOfWeek.ToString(),
                              Revenue = g.Sum(x => x.Amount)
                          }).OrderBy(x=>x.Day).ToList();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
             return revenueByDayList;
        }

        public async Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam parameters)
        {
            IList<MonthlyRevenue> monthlyRevenueList = new List<MonthlyRevenue>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Requirement is to fetch only 13 months of Data
                parameters.ToDate = parameters.FromDate.AddMonths(13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                monthlyRevenueList = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                  && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                  && products!.Contains(x.ProductId)
                  && x.TransactionDate >= parameters.FromDate && x.TransactionDate <= parameters.ToDate)
                    .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).Select(g =>
                      new MonthlyRevenue
                      {
                          FirstDayOfMonth = new DateTime(g.Key.Year, g.Key.Month, 1),
                          Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                          Revenue = g.Sum(x => x.Amount)
                      }).ToList();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return monthlyRevenueList.OrderBy(x => x.FirstDayOfMonth);
        }
          
        public async Task<IEnumerable<RevenueByProduct>> GetRevenueByProductByDays(FilterParam parameters)
        {
            IList<RevenueByProduct> revenueByProductList = new List<RevenueByProduct>();

            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();
               //Show only 24 hours of data
                parameters.ToDate = parameters.FromDate.AddHours(24);


                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                revenueByProductList = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                    && products!.Contains(x.ProductId)
                    && x.TransactionDate >= parameters.FromDate && x.TransactionDate <= parameters.ToDate)
                        .GroupBy(x => new { x.ProductName }).Select(g =>
                          new RevenueByProduct
                          {
                              Product = g.Key.ProductName,
                              Revenue = g.Sum(x => x.Amount)
                          }).ToList();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return revenueByProductList;
        }

        public async Task<IEnumerable<RevenueBudget>> GetRevenueVsBudget(FilterParam parameters)
        {
            IList<RevenueBudget> revenueBudgetList = new List<RevenueBudget>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Only filter Past 7 Months of Data
                parameters.ToDate = parameters.FromDate.AddMonths(7);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                revenueBudgetList = sqlContext.RevenueRevenueVsBudgetSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                 && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                 && products!.Contains(x.ProductId)
                 && x.TransactionDate >= parameters.FromDate && x.TransactionDate <= parameters.ToDate)
                    .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).Select(g =>
                          new RevenueBudget
                          {
                              Month = g.Key.Month.ToString(),
                              BudgetedRevenue = g.Sum(x => x.BudgetedRevenue),
                              Revenue = g.Sum(x => x.Revenue)
                          }).ToList();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return revenueBudgetList;

        }

        public async Task<IEnumerable<CurrentAndPreviousYearMonthlyTransaction>> GetTransactonMonths(FilterParam inputFilter)
        {
            var result = new List<CurrentAndPreviousYearMonthlyTransaction>();
            var currentYearFilter = inputFilter;
            try
            {
                var previousyearFilter = new FilterParam
                {
                    CustomerId = inputFilter.CustomerId,
                    UserId = inputFilter.UserId,
                    Facilities = inputFilter.Facilities,
                    FromDate = inputFilter.FromDate.AddYears(-1),
                    ToDate = inputFilter.ToDate.AddYears(-1),
                    ParkingLevels = inputFilter.ParkingLevels,
                    Products = inputFilter.Products
                };

                var currentYearResults = await GetTransactonByMonth(currentYearFilter);
                var previousYearResults = await GetTransactonByMonth(previousyearFilter);

                for (DateTime monthStart = inputFilter.FromDate; monthStart <= inputFilter.ToDate; monthStart = monthStart.AddMonths(1))
                {
                    var data = new CurrentAndPreviousYearMonthlyTransaction();
                    data.Month = monthStart.ToString("MMM");
                    var currentYearResult = currentYearResults.FirstOrDefault(x => x.Year == monthStart.Year && x.MonthAsInt == monthStart.Month);
                    var previousYearResult = previousYearResults.FirstOrDefault(x => x.Year == monthStart.Year - 1 && x.MonthAsInt == monthStart.Month);
                    data.NoOfTransactions = currentYearResult?.NoOfTransactions ?? 0;
                    data.PreviousYearNoOfTransactions = previousYearResult?.NoOfTransactions ?? 0;
                    result.Add(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetTransactonMonths)} has an error! : {ex.Message}");
            }
            return result;
        }

        private async Task<IList<MonthlyTransaction>> GetTransactonByMonth(FilterParam parameters)
        {
            IList<MonthlyTransaction> transactionsByMonth = new List<MonthlyTransaction>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                parameters.ToDate = parameters.FromDate.AddMonths(13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                transactionsByMonth = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                 && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                 && products!.Contains(x.ProductId)
                 && x.TransactionDate >= parameters.FromDate && x.TransactionDate <= parameters.ToDate)
                    .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).Select(g =>
                       new MonthlyTransaction
                       {
                           Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                           MonthAsInt = g.Key.Month,
                           Year = g.Key.Year,
                           NoOfTransactions = Convert.ToInt32(g.Count())

                       }).ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return transactionsByMonth;
        }

        /*private string GetHourAMPM(string hour)
        {
            string hourAMPM = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} {hour}:00:00.000";

            var dt = DateTime.Parse(hourAMPM);
            return dt.ToString("hh:mm tt");

        }*/

        #endregion Public Methods
    }
}