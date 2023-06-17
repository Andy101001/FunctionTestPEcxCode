namespace ABMVantage.Data.Service
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

                //Requirement is only to get last 13 Hours of DATA from Start Date
                //ADO Item: 4008
                parameters.ToDate = parameters.FromDate.AddHours(13);

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
                var fromDate = new DateTime(parameters.FromDate.Year, parameters.FromDate.Month, 1);
                var toDate = fromDate.AddMonths(13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();

                budgetVariance = sqlContext.RevenueAndBudgetSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                   && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                   && products!.Contains(x.ProductId) && x.FirstDayOfMonth >= fromDate && x.FirstDayOfMonth<= toDate)
                   .GroupBy(x => x.FirstDayOfMonth).Select(g =>
                       new BudgetVariance
                       {
                           FirstDayOfMonth = g.Key,
                           Month = g.Key.ToString("MMM"),
                           BgtVariance = ((g.Sum(x => x.Revenue)-g.Sum(x=>x.BudgetedRevenue))/ g.Sum(x => x.BudgetedRevenue)),
                       }
                       ).OrderBy(x => x.FirstDayOfMonth).ToList();



            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return budgetVariance;
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

                //ADO Item: 4003
                //13 months based on from date
                var fromDate = new DateTime(parameters.FromDate.Year, parameters.FromDate.Month, 1);
                var toDate = fromDate.AddMonths(13);
 
                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                revenueBudgetList = sqlContext.RevenueAndBudgetSQLData.Where(x => facilities!.Contains(x.FacilityId!)
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