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
    using System.Drawing;
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
            var dailyTransactionsWithZerosWhereThereIsNoData = new List<DailyTransaction>();
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

                var dailyTransactionsList = result.GroupBy(x => new { x.TransactionDate.DayOfWeek, TransactionDate = x.TransactionDate.Date }).Select(g =>
                        new DailyTransaction
                        {
                            TransactionDate= g.Key.TransactionDate,
                            WeekDay = g.Key.DayOfWeek.ToString(),
                            NoOfTransactions = Convert.ToDecimal(g.Count())
                        }).ToList();

                for (DateTime date = parameters.FromDate; date < parameters.ToDate; date = date.AddDays(1))
                {
                    var item = dailyTransactionsList.Where(x => x.TransactionDate == date.Date).FirstOrDefault();
                    if (item == null)
                    {
                        item = new DailyTransaction
                        {
                            TransactionDate = date.Date,
                            WeekDay = date.DayOfWeek.ToString(),
                            NoOfTransactions = 0
                        };
                        dailyTransactionsWithZerosWhereThereIsNoData.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dailyTransactionsWithZerosWhereThereIsNoData;
        }

        public async Task<IEnumerable<CurrentTransaction>> GetTransacionByHours(FilterParam parameters)
        {
            var transactionsByHourWithZerosWhereThereIsNoData = new List<CurrentTransaction>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Requirement is only to get last 13 Hours of DATA from Start Date
                //ADO Item: 4008
                parameters.ToDate = parameters.FromDate.AddHours(13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                  && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                  && products!.Contains(x.ProductId)
                  && x.TransactionDate >= parameters.FromDate && x.TransactionDate <= parameters.ToDate).AsEnumerable();

                var transactionsByHoursList = result.GroupBy(x => new DateTime(x.TransactionDate.Year, x.TransactionDate.Month, x.TransactionDate.Day, x.TransactionDate.Hour, 0, 0)).Select(g =>
                          new CurrentTransaction
                          {
                              TimeOfDay = g.Key.TimeOfDay,
                              NoOfTransactions = Convert.ToDecimal(g.Count()),
                              Time = g.Key.ToString("hh:mm tt")

                          }).ToArray();

                for (DateTime beginningOfHour = parameters.FromDate; beginningOfHour < parameters.ToDate; beginningOfHour = beginningOfHour.AddHours(1))
                {
                    var item = transactionsByHoursList.Where(x => x.TimeOfDay == beginningOfHour.TimeOfDay).FirstOrDefault();
                    if (item == null)
                    {
                        item = new CurrentTransaction
                        {
                            TimeOfDay = beginningOfHour.TimeOfDay,
                            Time = beginningOfHour.ToString("hh:mm tt"),
                            NoOfTransactions = 0
                        };
                    }
                    transactionsByHourWithZerosWhereThereIsNoData.Add(item);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return transactionsByHourWithZerosWhereThereIsNoData;
        }

        public async Task<IEnumerable<BudgetVariance>> GetBudgetVsActualVariance(FilterParam parameters)
        {
            var budgetVarianceWithZerosForNoData = new List<BudgetVariance>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();
                //Requirement: Show 13 month of data from todate
                var fromDate = new DateTime(parameters.FromDate.Year, parameters.FromDate.Month, 1);
                var toDate = fromDate.AddMonths(13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();

                var budgetVariance = sqlContext.RevenueAndBudgetSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                   && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                   && products!.Contains(x.ProductId) && x.FirstDayOfMonth >= fromDate && x.FirstDayOfMonth <= toDate)
                   .GroupBy(x => x.FirstDayOfMonth).Select(g =>
                       new BudgetVariance
                       {
                           FirstDayOfMonth = g.Key,
                           Month = g.Key.ToString("MMM"),
                           BgtVariance = ((g.Sum(x => x.Revenue) - g.Sum(x => x.BudgetedRevenue)) / g.Sum(x => x.BudgetedRevenue)),
                       }
                       ).OrderBy(x => x.FirstDayOfMonth).ToArray();

                for (DateTime monthStart = fromDate; monthStart < toDate; monthStart = monthStart.AddMonths(1))
                {
                    var item = budgetVariance.Where(x => x.FirstDayOfMonth == monthStart).FirstOrDefault();
                    if (item == null)
                    {
                        item = new BudgetVariance { BgtVariance = 0, FirstDayOfMonth = monthStart, Month = monthStart.ToString("MMM") };
                    }
                    budgetVarianceWithZerosForNoData.Add(item);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return budgetVarianceWithZerosForNoData;
        }

        public async Task<IEnumerable<RevenueByDay>> GetRevenueByDays(FilterParam parameters)
        {

            List<RevenueByDay> revenueByDayWithZerosWhereThereIsNoData= new List<RevenueByDay>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Requirement: show 7 days data
                //Revenue & Transactions: Revenue: 3998 
                parameters.ToDate = parameters.FromDate.AddDays(7);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                    && products!.Contains(x.ProductId)
                     && x.TransactionDate >= parameters.FromDate && x.TransactionDate < parameters.ToDate).AsEnumerable();

                var revenueByDayList = result.GroupBy(x => x.TransactionDate.Date).Select(g =>
                          new RevenueByDay
                          {
                              Day = g.Key,
                              WeekDay = g.Key.DayOfWeek.ToString(),
                              Revenue = g.Sum(x => x.Amount)
                          }).OrderBy(x=>x.Day).ToList();

                for (DateTime day = parameters.FromDate.Date; day < parameters.ToDate.Date; day = day.AddDays(1))
                {
                    var revenueByDay = revenueByDayList.Where(x => x.Day == day).FirstOrDefault();
                    if (revenueByDay == null) 
                    {
                        revenueByDay = new RevenueByDay { Day = day, Revenue = 0, WeekDay = day.DayOfWeek.ToString() };
                    }
                    revenueByDayWithZerosWhereThereIsNoData.Add(revenueByDay);


                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
             return revenueByDayWithZerosWhereThereIsNoData;
        }

        public async Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam parameters)
        {
            var monthlyRevenueWithZerosWhereThereIsNoData = new List<MonthlyRevenue>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Requirement is to fetch only 13 months of Data
                var fromDate = new DateTime(parameters!.FromDate.Year, parameters!.FromDate.Month, 1);
                var toDate = new DateTime(parameters.ToDate.Year, parameters.ToDate.Month, 1).AddMonths(1);
                var monthlyInterval = (toDate.Year - fromDate.Year) * 12 + (toDate.Month - fromDate.Month);
                toDate = monthlyInterval < 13 ? toDate : fromDate.AddMonths(13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var monthlyRevenue = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                  && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                  && products!.Contains(x.ProductId)
                  && x.TransactionDate >= fromDate && x.TransactionDate < toDate)
                    .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).Select(g =>
                      new MonthlyRevenue
                      {
                          FirstDayOfMonth = new DateTime(g.Key.Year, g.Key.Month, 1),
                          Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                          Revenue = g.Sum(x => x.Amount)
                      }).ToList();
                for (DateTime monthStart = fromDate; monthStart < toDate; monthStart = monthStart.AddMonths(1))
                {
                    var item = monthlyRevenue.FirstOrDefault(x => x.FirstDayOfMonth == monthStart);
                    if (item == null)
                    {
                        item = new MonthlyRevenue { FirstDayOfMonth = monthStart, PreviousYearRevenue = null, Revenue = 0, Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthStart.Month) };
                    }
                    monthlyRevenueWithZerosWhereThereIsNoData.Add(item);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return monthlyRevenueWithZerosWhereThereIsNoData;
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
            var revenueBudgetWithZerosForNoDataMonths = new List<RevenueBudget>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //ADO Item: 4003
                //13 months based on from date
                var fromDate = new DateTime(parameters.FromDate.Year, parameters.FromDate.Month, 1);
                var toDate = fromDate.AddMonths(13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var revenueBudgetList = sqlContext.RevenueAndBudgetSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                 && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                 && products!.Contains(x.ProductId)
                 && x.FirstDayOfMonth >= fromDate && x.FirstDayOfMonth <= toDate)
                    .GroupBy(x => x.FirstDayOfMonth).Select(g =>
                          new RevenueBudget
                          {
                              FirstDayOfMonth = g.Key,
                              Month = g.Key.ToString("MMM"),
                              BudgetedRevenue = g.Sum(x => x.BudgetedRevenue),
                              Revenue = g.Sum(x => x.Revenue)
                          }).OrderBy(x => x.FirstDayOfMonth).ToList();
                
                for (DateTime monthStart = fromDate; monthStart < toDate; monthStart = monthStart.AddMonths(1))
                {
                    var revenueBudget = revenueBudgetList.Where(x => x.FirstDayOfMonth == monthStart).FirstOrDefault();
                    if (revenueBudget == null)
                    {
                        revenueBudget = new RevenueBudget { FirstDayOfMonth = monthStart, Month = monthStart.ToString("MMM"), BudgetedRevenue = 0, Revenue = 0 };
                    }
                    revenueBudgetWithZerosForNoDataMonths.Add(revenueBudget);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return revenueBudgetWithZerosForNoDataMonths;

        }

        public async Task<IEnumerable<CurrentAndPreviousYearMonthlyTransaction>> GetTransactonMonths(FilterParam inputFilter)
        {
            var result = new List<CurrentAndPreviousYearMonthlyTransaction>();
            
            try
            {
                var currentYearFilter = inputFilter;
                currentYearFilter.FromDate = new DateTime(inputFilter.FromDate.Year, inputFilter.FromDate.Month, 1);
                currentYearFilter.ToDate = currentYearFilter.FromDate.AddMonths(13);
                var previousyearFilter = new FilterParam
                {
                    CustomerId = currentYearFilter.CustomerId,
                    UserId = currentYearFilter.UserId,
                    Facilities = currentYearFilter.Facilities,
                    FromDate = currentYearFilter.FromDate.AddYears(-1),
                    ToDate = currentYearFilter.ToDate.AddYears(-1),
                    ParkingLevels = currentYearFilter.ParkingLevels,
                    Products = currentYearFilter.Products
                };

                var currentYearResults = await GetTransactonByMonth(currentYearFilter);
                var previousYearResults = await GetTransactonByMonth(previousyearFilter);

                for (DateTime monthStart = currentYearFilter.FromDate; monthStart < currentYearFilter.ToDate; monthStart = monthStart.AddMonths(1))
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
                //to show 13 month of data, it 12 month to from date and 1 current month is alrady included.
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