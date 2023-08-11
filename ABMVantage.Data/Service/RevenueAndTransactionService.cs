namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.DataAccess;
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using ABMVantage.Data.Models.DashboardModels;
    using Microsoft.Azure.Cosmos.Serialization.HybridRow;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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

        public async Task<DailyTransactionList> GetTransactonByDays(FilterParam parameters)
        {
            var trans = new DailyTransactionList();
            trans.Transactions = new List<DailyTransaction>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Requirement is to fetch 7 days of Data from Start Date
                parameters.ToDate = parameters.FromDate.AddDays(7);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                  && levels!.Contains(x.LevelId!)
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
                        
                    }
                    trans.Transactions.Add(item);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }


            //UI date rage display
            trans.FromDate = parameters!.FromDate;
            trans.ToDate = parameters!.ToDate;

            return trans;
        }

        public async Task<CurrentTransactionList> GetTransacionByHours(FilterParam parameters)
        {
            var transactions = new CurrentTransactionList();

            transactions.CurrentTransactions = new List<CurrentTransaction>();
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
                  && levels!.Contains(x.LevelId!)
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
                    transactions.CurrentTransactions.Add(item);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            //UI date rage display
            transactions.FromDate = parameters!.FromDate;
            transactions.ToDate = parameters!.ToDate;

            return transactions;
        }

        public async Task<BudgetVarianceList> GetBudgetVsActualVariance(FilterParam parameters)
        {
            var budgetAndVariance = new BudgetVarianceList();
            budgetAndVariance.Variances = new List<BudgetVariance>();
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
                   && levels!.Contains(x.LevelId!)
                   && products!.Contains(x.ProductId) && x.FirstDayOfMonth >= fromDate && x.FirstDayOfMonth <= toDate)
                   .GroupBy(x => x.FirstDayOfMonth).Select(g =>
                       new BudgetVariance
                       {
                           FirstDayOfMonth = g.Key,
                           Month = g.Key.ToString("MMM"),
                           BgtVariance = g.Sum(x => x.BudgetedRevenue) > 0 ? ((g.Sum(x => x.Revenue) - g.Sum(x => x.BudgetedRevenue)) / g.Sum(x => x.BudgetedRevenue)) : 0,
                       }
                       ).OrderBy(x => x.FirstDayOfMonth).ToArray();

                for (DateTime monthStart = fromDate; monthStart < toDate; monthStart = monthStart.AddMonths(1))
                {
                    var item = budgetVariance.Where(x => x.FirstDayOfMonth == monthStart).FirstOrDefault();
                    if (item == null)
                    {
                        item = new BudgetVariance { BgtVariance = 0, FirstDayOfMonth = monthStart, Month = monthStart.ToString("MMM") };
                    }
                    budgetAndVariance.Variances.Add(item);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            //UI date rage display
            budgetAndVariance.FromDate = parameters!.FromDate;
            budgetAndVariance.ToDate = parameters!.ToDate;


            return budgetAndVariance;
        }

        public async Task<RevenueByDayList> GetRevenueByDays(FilterParam parameters)
        {
            var revenuList = new RevenueByDayList();

            revenuList.RevenuList = new List<RevenueByDay>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Requirement: show 7 days data
                //Revenue & Transactions: Revenue: 3998 
                parameters.ToDate = parameters.FromDate.AddDays(7);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && levels!.Contains(x.LevelId!)
                    && products!.Contains(x.ProductId)
                     && x.RevenueDate >= parameters.FromDate && x.RevenueDate < parameters.ToDate).AsEnumerable();

                var revenueByDayList = result.GroupBy(x => new { Day = x.RevenueDate.Date, Product = x.ProductName }).Select(g =>
                          new RevenueByDayAndProduct
                          {
                              Product = g.Key.Product,
                              Day = g.Key.Day,
                              Revenue = g.Sum(x => x.Amount)
                          }).ToList();

                for (DateTime day = parameters.FromDate.Date; day < parameters.ToDate.Date; day = day.AddDays(1))
                {
                    var itemsForDay = revenueByDayList.Where(x => x.Day == day);
                    RevenueByDay revenueByDay = new RevenueByDay();
                    if (!itemsForDay.Any()) 
                    {
                        revenueByDay = new RevenueByDay { Day = day, WeekDay = day.DayOfWeek.ToString(), Data = parameters.Products.Select(x => new Data { Product  = x.Name, Revenue = 0}).ToList() };
                    }
                    else
                    {
                        revenueByDay.Day = day;
                        revenueByDay.WeekDay = day.DayOfWeek.ToString();
                        revenueByDay.Data = new List<Data>();
                        foreach (var product in parameters.Products)
                        {
                            var item = itemsForDay.Where(x => x.Product == product.Name).FirstOrDefault();
                            if (item == null)
                            {
                                revenueByDay.Data.Add(new Data { Product = product.Name, Revenue = 0 });
                            }
                            else
                            {
                                revenueByDay.Data.Add(new Data { Product = item.Product, Revenue = item.Revenue });
                            }
                        }
                    }

                    revenuList.RevenuList.Add(revenueByDay);


                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            //UI date rage display
            revenuList.FromDate = parameters!.FromDate;
            revenuList.ToDate = parameters!.ToDate;


            return revenuList;
        }

        public async Task<MonthlyRevenueList> GetRevenueByMonths(FilterParam parameters)
        {
            var monthsRevenue = new MonthlyRevenueList();

            monthsRevenue.MonthRevenues = new List<MonthlyRevenue>();
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
                var monthlyRevenue = sqlContext.RevenueSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                  && levels!.Contains(x.LevelId!)
                  && products!.Contains(x.ProductId)
                  && x.RevenueDate >= fromDate && x.RevenueDate < toDate)
                    .GroupBy(x => new { x.RevenueDate.Year, x.RevenueDate.Month }).Select(g =>
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
                    monthsRevenue.MonthRevenues.Add(item);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            //UI date rage display
            monthsRevenue.FromDate = parameters!.FromDate;
            monthsRevenue.ToDate = parameters!.ToDate;


            return monthsRevenue;
        }
          
        public async Task<RevenueByProductList> GetRevenueByProductByDays(FilterParam parameters)
        {
            var revenueProducts = new RevenueByProductList();
            revenueProducts.RevenueByProducts = new List<RevenueByProduct>();

            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();
               //Show only 24 hours of data
                parameters.ToDate = parameters.FromDate.AddHours(24);


                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                revenueProducts.RevenueByProducts = sqlContext.RevenueSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && levels!.Contains(x.LevelId!)
                    && products!.Contains(x.ProductId)
                    && x.RevenueDate >= parameters.FromDate && x.RevenueDate <= parameters.ToDate)
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

            //UI date rage display
            revenueProducts.FromDate = parameters!.FromDate;
            revenueProducts.ToDate = parameters!.ToDate;

            return revenueProducts;
        }

        public async Task<RevenueBudgetList> GetRevenueVsBudget(FilterParam parameters)    
        {
            var revenuBudget =new RevenueBudgetList();
            revenuBudget.RevenueBudgets = new List<RevenueBudget>();
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
                 && levels!.Contains(x.LevelId!)
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
                    revenuBudget.RevenueBudgets.Add(revenueBudget);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            //UI date rage display
            revenuBudget.FromDate = parameters!.FromDate;
            revenuBudget.ToDate = parameters!.ToDate;

            return revenuBudget;

        }

        public async Task<CurrentAndPreviousYearMonthlyTransactionList> GetTransactonMonths(FilterParam inputFilter)
        {
            var transactions = new CurrentAndPreviousYearMonthlyTransactionList();
            transactions.PreviousYearMonthly = new List<CurrentAndPreviousYearMonthlyTransaction>();
            
            try
            {
                var currentYearFilter = inputFilter;
                currentYearFilter.FromDate = new DateTime(inputFilter.FromDate.Year, inputFilter.FromDate.Month, 1);

                //https://dev.azure.com/abm-ss/ABMVantage/_workitems/edit/2693/
                //bug # 4018 Dated: 08/11/2023
                // show data only in selected date range, if month>13 then take only 13 months of data from fromDate.
                var monthlyInterval = (currentYearFilter.ToDate.Year - currentYearFilter.FromDate.Year) * 12 + (currentYearFilter.ToDate.Month - currentYearFilter.FromDate.Month);
                
                currentYearFilter.ToDate = monthlyInterval < 13 ? currentYearFilter.ToDate : currentYearFilter.FromDate.AddMonths(12);

                //currentYearFilter.ToDate = currentYearFilter.FromDate.AddMonths(13);

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

                for (DateTime monthStart = currentYearFilter.FromDate; monthStart <= currentYearFilter.ToDate; monthStart = monthStart.AddMonths(1))
                {
                    var data = new CurrentAndPreviousYearMonthlyTransaction();
                    data.Month = monthStart.ToString("MMM");
                    var currentYearResult = currentYearResults.FirstOrDefault(x => x.Year == monthStart.Year && x.MonthAsInt == monthStart.Month);
                    var previousYearResult = previousYearResults.FirstOrDefault(x => x.Year == monthStart.Year - 1 && x.MonthAsInt == monthStart.Month);
                    data.NoOfTransactions = currentYearResult?.NoOfTransactions ?? 0;
                    data.PreviousYearNoOfTransactions = previousYearResult?.NoOfTransactions ?? 0;
                    transactions.PreviousYearMonthly.Add(data);
                }


                //UI date rage display
                transactions.FromDate = currentYearFilter!.FromDate;
                transactions.ToDate = currentYearFilter!.ToDate;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetTransactonMonths)} has an error! : {ex.Message}");
            }

            return transactions;
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
                //parameters.ToDate = parameters.FromDate.AddMonths(13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                transactionsByMonth = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                 && levels!.Contains(x.LevelId!)
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



        #endregion Public Methods
    }
}