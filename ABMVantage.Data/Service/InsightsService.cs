namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.Configuration;
    using ABMVantage.Data.DataAccess;
    using ABMVantage.Data.EntityModels.SQL;
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using ABMVantage.Data.Models.DashboardModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using StackExchange.Redis;
    using System;
    using System.Buffers.Text;
    using System.Threading.Tasks;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class InsightsService : ServiceBase, IInsightsService
    {
        private readonly ILogger<InsightsService> _logger;
        private readonly IDbContextFactory<CosmosDataContext> _factory;
        private readonly IDbContextFactory<SqlDataContextVTG> _sqlDataContextVTG;
        private readonly InsightsServiceSettings _insightsServiceSettings;

        public InsightsService(ILoggerFactory loggerFactory, IRepository repository, IDbContextFactory<CosmosDataContext> factory, IDbContextFactory<SqlDataContextVTG> sqlDataContextVTG, InsightsServiceSettings insightsServiceSettings)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<InsightsService>();
            _repository = repository;
            _factory = factory;
            _sqlDataContextVTG = sqlDataContextVTG;
            _insightsServiceSettings = insightsServiceSettings;
        }
        public async Task<DailyAverageOccupancy>? GetDailyAverageOccupancy(FilterParam? filterParameters)
        {
            DailyAverageOccupancy? dailyAverageOccupancy = new DailyAverageOccupancy() { AverageDailyOccupancyInteger = 0, AverageDailyOccupancyPercentage = 0 };

            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
                var fromDate = filterParameters!.FromDate.AddDays(-1);
                var toDate = filterParameters.FromDate;

                

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.InsightsAverageDialyOccupanySQLData.Where(x => facilities!.Contains(x.FacilityId!) 
                    && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                    && products!.Contains(x.ProductId)
                    && (x.Date >= fromDate && x.Date < toDate));

                var sql= result.ToQueryString();

                var data=result.ToList();

                int totalOccupiedParkingSpotHours = result.Sum(x => x.TotalOccupancy);
                
                int totalParkingSpaceCount = sqlContext.FacilityLevelProductSQLData.Where(x => facilities!.Contains(x.FacilityId!) 
                    && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) 
                    && products!.Contains(x.ProductId)).Sum(x => x.ParkingSpaceCount);
                TimeSpan filterRange = toDate - fromDate;
                int totalAvailableSpaceHours = totalParkingSpaceCount * filterRange.Days * 24;
                if (totalParkingSpaceCount > 0)
                {
                    //var avdt = totalOccupiedParkingSpotHours / totalParkingSpaceCount * 24;
                    dailyAverageOccupancy.AverageDailyOccupancyInteger = Convert.ToInt32((decimal) totalOccupiedParkingSpotHours / (decimal) totalAvailableSpaceHours * (decimal) totalParkingSpaceCount);
                    dailyAverageOccupancy.AverageDailyOccupancyPercentage = Convert.ToInt32((decimal) totalOccupiedParkingSpotHours / (decimal) totalAvailableSpaceHours * 100);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return dailyAverageOccupancy;
        }

        public async Task<double> GetDailyTotalRevenueAsync(FilterParam filterParameters)
        {
            double totalRevenue = 0;
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                //ADO Item: 3983 Dated: 06/09/2023
                //Description: Data on tile to reflect the previous 24 hours based on from date selected.
                //ex.From date = 11 / 1 / 2023 then the value on the card should be 10 / 31 / 2023(midnight to midnight)

                filterParameters!.ToDate = filterParameters!.FromDate.Date;
                filterParameters!.FromDate = filterParameters!.FromDate.Date.AddDays(-1);
               

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.InsightsTotalRevenueSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                         && (x.Day >= filterParameters!.FromDate && x.Day <= filterParameters.ToDate));

                totalRevenue = result.Sum(x => x.TotalRevenue);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return totalRevenue;

        }

        public async Task<int> GetDailyTransactiontCountAsync(FilterParam filterParameters)
        {
            int totalTransactionsCount = 0;
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                //Calculation is confirmed to be based on last 24 hours based on from date (looking backward)
                //ADO Item:3982
                filterParameters.ToDate = new DateTime(filterParameters.FromDate.Year, filterParameters.FromDate.Month, filterParameters.FromDate.Day);
                filterParameters.FromDate = filterParameters.ToDate.AddDays(-1);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                          && (x.TransactionDate >= filterParameters!.FromDate && x.TransactionDate < filterParameters!.ToDate));

                totalTransactionsCount = result.Count();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return totalTransactionsCount;
        }
        
        public async Task<DashboardDailyReservationCountByHour> GetHourlyReservationsByProduct(FilterParam filterParameters)
        {
            DashboardDailyReservationCountByHour? dashboardDailyReservationCountByHour = new DashboardDailyReservationCountByHour();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                //Requirement is only for 1 DAY
                filterParameters!.ToDate = filterParameters.FromDate.AddDays(1);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.ReservationsSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (x.LevelId == string.Empty || x.LevelId == null || levels!.Contains(x.LevelId!)) && products!.Contains(x.ProductId)
                        && (x.BeginningOfHour >= filterParameters!.FromDate && x.BeginningOfHour < filterParameters.ToDate)).ToList();


                //Create a nested grouping by time of day and product
                var resultWithNestedGroupingByTimeOfDayAndProduct = result.GroupBy(x => x.BeginningOfHour).Select(g =>
                new HourlyReservationCount
                {
                    ReservationDateTime = g.Key,
                    Data = g.GroupBy(x => x.ProductName).Select(sg => new ReservationsByProduct { Product = sg.Key ?? string.Empty, NoOfReservations = sg.Sum(x => x.NoOfReservations) })
                });

                var resultWithGroupingAndZerosForMissingData = new List<HourlyReservationCount>();
                var hoursInDay = Enumerable.Range(0, 24).Select(x => TimeSpan.FromHours(x));
                foreach (var hour in hoursInDay)
                {
                    var resultItem = resultWithNestedGroupingByTimeOfDayAndProduct.Where(x => x.ReservationDateTime.TimeOfDay == hour).FirstOrDefault();
                    if (resultItem == null)
                    {
                        resultItem = new HourlyReservationCount
                        {
                            ReservationDateTime = new DateTime() + hour,
                            Data = filterParameters.Products.Select(x => new ReservationsByProduct { NoOfReservations = 0, Product = x.Name }),
                        };
                    }
                    else
                    {
                        var reservationsByProduct = resultItem.Data.ToList();
                        foreach(var product in filterParameters.Products)
                        {
                            var reservationByProduct = reservationsByProduct.Where(x => x.Product == product.Name).FirstOrDefault();
                            if (reservationByProduct == null)
                            {
                                reservationsByProduct.Add(new ReservationsByProduct {  Product = product.Name, NoOfReservations = 0 });
                            }
                        }
                        resultItem.Data = reservationsByProduct;
                    }
                    resultWithGroupingAndZerosForMissingData.Add(resultItem);

                }

                dashboardDailyReservationCountByHour.ReservationsByHour = resultWithGroupingAndZerosForMissingData;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dashboardDailyReservationCountByHour;
        }

        public async Task<DashboardMonthlyRevenueAndBudget> GetMonthlyRevenueAndBudget(FilterParam filterParameters)
        {
            DashboardMonthlyRevenueAndBudget? dashboardMonthlyRevenueAndBudget = new DashboardMonthlyRevenueAndBudget();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                //Charan Dated:06/09/2023
                // Time frame should look at most 12 months based on date range selected.
                //If less than 12 months is selected then only show those months.
                var fromDate = new DateTime(filterParameters!.FromDate.Year, filterParameters!.FromDate.Month, 1);
                var toDate = new DateTime(filterParameters.ToDate.Year, filterParameters.ToDate.Month, 1).AddMonths(1);
                var monthlyInterval = (toDate.Year - fromDate.Year) * 12 + (toDate.Month - fromDate.Month);
                toDate = monthlyInterval < 12 ? toDate : fromDate.AddMonths(12);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueAndBudgetSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                       && (x.FirstDayOfMonth >= fromDate && x.FirstDayOfMonth < toDate)).ToList();

                var resultWithZerosForMissingData = new List<RevenueAndBudget>();
                for (DateTime monthStart = fromDate; monthStart < toDate;monthStart = monthStart.AddMonths(1))
                {
                    RevenueAndBudget monthlyRevenueAndBudget;
                    var monthlyData = result.Where(x => x.FirstDayOfMonth == monthStart);
                    if (!monthlyData.Any())
                    {
                        monthlyRevenueAndBudget = new RevenueAndBudget { Date = monthStart, BudgetedRevenue = 0, Revenue = 0 };
                    }
                    else
                    {
                        monthlyRevenueAndBudget = new RevenueAndBudget { Date = monthStart, Revenue = monthlyData.Sum(x => x.Revenue), BudgetedRevenue = monthlyData.Sum(x => x.BudgetedRevenue) };
                    }
                    resultWithZerosForMissingData.Add(monthlyRevenueAndBudget);
         
                }

                /*
                //Group by Year and Month
                var gResult = result.GroupBy(x => new { x.FirstDayOfMonth.Year, x.FirstDayOfMonth.Month }).Select(g =>
                 new RevenueAndBudgetForMonth
                 {
                     Year = g.Key.Year,
                     Month = g.Key.Month,
                     Revenue = g.Sum(x => x.Revenue),
                     BudgetedRevenue = g.Sum(x => x.BudgetedRevenue)
                 }).ToList();

                var diff = Enumerable.Range(0, Int32.MaxValue)
                     .Select(e => filterParameters!.FromDate.AddMonths(e))
                     .TakeWhile(e => e <= filterParameters.ToDate)
                     .Select(e => new { e.Date});

                var fResult = from RevenueAndBudgetForMonth rnb in gResult
                                select new RevenueAndBudget
                                {
                                    Date = new DateTime(rnb.Year, rnb.Month, 1),
                                    Revenue = rnb.Revenue,
                                    BudgetedRevenue = rnb.BudgetedRevenue
                                 };
                */
                dashboardMonthlyRevenueAndBudget.MonthlyRevenueAndBudget = resultWithZerosForMissingData;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dashboardMonthlyRevenueAndBudget;
        }

        public async Task<DashboardMonthlyParkingOccupancy> GetMonthlyParkingOccupancyAsync(FilterParam filterParameters)
        {
            DashboardMonthlyParkingOccupancy? dashboardMonthlyParkingOccupancy = new DashboardMonthlyParkingOccupancy();
            try
            {
                List<ParkingOccupancy> monthlyParkingOccupancyData = new List<ParkingOccupancy>();
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                //If more than a 6 month date range is given, then this api should return
                //Full months of data should be shown regardless of date selected. 
                var fromDate = new DateTime(filterParameters!.FromDate.Year, filterParameters!.FromDate.Month, 1);
                var toDate = new DateTime(filterParameters.ToDate.Year, filterParameters.ToDate.Month, 1).AddMonths(1);
                var monthlyInterval = (toDate.Year - fromDate.Year) * 12 + (toDate.Month - fromDate.Month);
                toDate = monthlyInterval < 6 ? toDate : fromDate.AddMonths(6);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();

                int totalParkingSpaceCount = sqlContext.FacilityLevelProductSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                && products!.Contains(x.ProductId)).Sum(x => x.ParkingSpaceCount);


                var result = sqlContext.InsightsMonthlyParkingOccupancySQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                       && (x.FirstDayOfMonth >= fromDate && x.FirstDayOfMonth < toDate)).ToList();

                //Group by Year and Month
                IEnumerable<OccupancyByMonth> gResult = new List<OccupancyByMonth>();
                if (result.Sum(x => x.ParkingSpaceCount) > 0) {
                    gResult  = result.GroupBy(x => new { x.FirstDayOfMonth.Year, x.FirstDayOfMonth.Month }).Select(g =>
                     new OccupancyByMonth
                     {
                         Year = g.Key.Year,
                         Month =  g.Key.Month,
                         OccupancyInteger =  Convert.ToInt32((decimal) g.Sum(x => x.TotalOccupancy) / ((decimal) (totalParkingSpaceCount * g.First().NoOFDaysInMonth * 24)) * ((decimal) totalParkingSpaceCount)),
                         OccupancyPercentage = (((decimal)g.Sum(x => x.TotalOccupancy)) / ((decimal) (totalParkingSpaceCount * g.First().NoOFDaysInMonth * 24)) * 100)
                     });
                }

                for (DateTime monthStart = fromDate; monthStart < toDate; monthStart = monthStart.AddMonths(1))
                {
                    var currentYearOccupancyByMonth = gResult.FirstOrDefault(x => x.Year == monthStart.Year && x.Month == monthStart.Month);
                    var priorYearOccupancyByMonth = gResult.FirstOrDefault(x => x.Year == monthStart.Year - 1 && x.Month == monthStart.Month);
                    var parkingOccupancy = new ParkingOccupancy();
                    parkingOccupancy.Month = new DateTime(monthStart.Year, monthStart.Month, 1).ToString("MMM"); // $"{monthStart.Month}{monthStart.Year}";
                    if (currentYearOccupancyByMonth != null)
                    {
                        parkingOccupancy.OccupancyInteger = currentYearOccupancyByMonth.OccupancyInteger;
                        parkingOccupancy.OccupancyPercentage = currentYearOccupancyByMonth.OccupancyPercentage;
                    }
                    else
                    {
                        parkingOccupancy.OccupancyInteger = 0;
                        parkingOccupancy.OccupancyPercentage = 0;
                    }
                    if (priorYearOccupancyByMonth != null)
                    {
                        parkingOccupancy.PreviousYearOccupancyInteger = priorYearOccupancyByMonth.OccupancyInteger;
                        parkingOccupancy.PreviousYearOccupancyPercentage = priorYearOccupancyByMonth.OccupancyPercentage;
                    }
                    else
                    {
                        parkingOccupancy.PreviousYearOccupancyInteger = 0;
                        parkingOccupancy.PreviousYearOccupancyPercentage = 0;
                    }
                    monthlyParkingOccupancyData.Add(parkingOccupancy);
                }
                dashboardMonthlyParkingOccupancy.MonthlyParkingOccupancy = monthlyParkingOccupancyData;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dashboardMonthlyParkingOccupancy;
        }
        public async Task<DashboardMonthlyTransactionCount> GetMonthlyTransactionCountAsync(FilterParam filterParameters)
        {
            DashboardMonthlyTransactionCount? dashboardMonthlyTransactionCount = new DashboardMonthlyTransactionCount();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
                filterParameters!.FromDate = new DateTime(filterParameters!.FromDate.Year, filterParameters!.FromDate.Month, 1);
                
                //ADO Item: 4018, 3993
                //Description: AC8: If the end user inputs a date range exceeding 13 months, the chart should display 13 months of data
                //starting from the selected start date. For example, if the user selects a date range of > 13 months,
                //the chart should only display data for the first 13 months of the selected range.
                //If the user selects a date range less than 13 months, then the lesser date range should be selected.

                var fromDate = new DateTime(filterParameters!.FromDate.Year, filterParameters!.FromDate.Month, 1);
                var toDate = new DateTime(filterParameters.ToDate.Year, filterParameters.ToDate.Month, 1).AddMonths(1);
                var monthlyInterval = (toDate.Year - fromDate.Year) * 12 + (toDate.Month - fromDate.Month);
                toDate = monthlyInterval < 13 ? toDate : fromDate.AddMonths(13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var transactionsByProductNameAndMonth = sqlContext.InsightsMonthlyTransactionsSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                      && (x.FirstDayOfMonth >= fromDate && x.FirstDayOfMonth <= toDate)).Select(g =>
                 new TransactionsByMonthAndProduct
                 {
                     Year = g.FirstDayOfMonth.Year,
                     Month = g.FirstDayOfMonth.Month,
                     ParkingProduct = g.ProductName,
                     TransactionCount = g.TransactionCount
                 }).ToList();


                var resultWithZerosForMissingData = new List<TransactionCountForMonth>();
                for (DateTime monthStart = fromDate; monthStart < toDate; monthStart = monthStart.AddMonths(1))
                {

                    var transactionCountForMonth = new TransactionCountForMonth { Date = new DateTime(monthStart.Year, monthStart.Month, 1) };
                    
                    var transactionsForMonth = transactionsByProductNameAndMonth.Where(x => x.Year == monthStart.Year && x.Month == monthStart.Month).OrderBy(x => x.ParkingProduct);
                    
                    if (!transactionsForMonth.Any())
                    {
                        transactionCountForMonth.Data = filterParameters.Products.Select(x => new TransactionsForProduct { Product = x.Name, NoOfTransactions = 0 }).ToList();
                    }
                    else
                    {
                        foreach (var product in filterParameters.Products)
                        {
                            var item = transactionsForMonth.Where(x => x.ParkingProduct == product.Name).Select(x => new TransactionsForProduct{ NoOfTransactions = x.TransactionCount, Product = product.Name  }).FirstOrDefault();
                            if (item == null)
                            {
                                item = new TransactionsForProduct { NoOfTransactions = 0, Product = product.Name  };

                            }
                            transactionCountForMonth.Data.Add(item);
                        }
                    }
                    resultWithZerosForMissingData.Add(transactionCountForMonth);
                }


                    /*var transactionsByMonth = from TransactionsByMonthAndProduct cnt in transactionsByProductNameAndMonth
                               group cnt by new { cnt.Year, cnt.Month } into monthlyGroup
                              select new TransactionCountForMonth
                              {
                                  Date = new DateTime(monthlyGroup.Key.Year, monthlyGroup.Key.Month, 1),
                                  Data = monthlyGroup.Select(x => new TransactionsForProduct { NoOfTransactions = x.TransactionCount, Product = x.ParkingProduct })
                              };*/



                dashboardMonthlyTransactionCount.MonthlyTransactions = resultWithZerosForMissingData;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dashboardMonthlyTransactionCount;
        }
        public async Task<DashboardMonthlyAverageTicketValue> GetMonthlyAverageTicketValue(FilterParam filterParameters)
        {
            DashboardMonthlyAverageTicketValue? dashboardMonthlyAverageTicketValue = new DashboardMonthlyAverageTicketValue();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                //ADO:3996 Insights - Avg Ticket Value
                //The visual will look back 13 months based on the month of the current day (inclusive of the current month).

               var toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1);
               var fromDate = toDate.AddMonths(-13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.InsightsAverageMonthlyTicketValueSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                     && (x.FirstDayOfMonth >= fromDate && x.FirstDayOfMonth < toDate)).ToList();

                var resultWithNestedMonthAndProductGrouping = from InsightsAverageMonthlyTicketValueSQL data in result
                                                              group data by new { data.FirstDayOfMonth.Year, data.FirstDayOfMonth.Month } into monthlyGroup
                                                              select new AverageTicketValueForMonth
                                                              {
                                                                  Date = new DateTime(monthlyGroup.Key.Year, monthlyGroup.Key.Month, 1), //monthlyGroup.Key.Year.ToString() + monthlyGroup.Key.Month.ToString(),
                                                                  Data = monthlyGroup.Select(x => new TicketValueAverage { ParkingProduct = x.ProductName!, AverageTicketValue = Convert.ToInt32(x.AverageTicketValue) }).ToList()
                                                              };

                var resultWithZerosForMissingData = new List<AverageTicketValueForMonth>();
                for (DateTime monthStart = fromDate; monthStart < toDate; monthStart = monthStart.AddMonths(1))
                {
                    var averageTicketValueForMonth = resultWithNestedMonthAndProductGrouping.Where(x => x.Date == monthStart).FirstOrDefault();
                    if (averageTicketValueForMonth == null)
                    {
                        averageTicketValueForMonth = new AverageTicketValueForMonth
                        {
                            Date = monthStart,
                            Data = filterParameters.Products.Select(x => new TicketValueAverage { ParkingProduct = x.Name, AverageTicketValue = 0 }).ToList()
                        };
                    }
                    else
                    {
                        foreach (var product in filterParameters.Products) 
                        {
                            var ticketValueAverage = averageTicketValueForMonth.Data.Where(x => x.ParkingProduct == product.Name).FirstOrDefault();
                            if (ticketValueAverage == null)
                            {
                                ticketValueAverage = new TicketValueAverage { ParkingProduct = product.Name, AverageTicketValue = 0 };
                                averageTicketValueForMonth.Data.Add(ticketValueAverage);
                            }
                        }
                    }
                    resultWithZerosForMissingData.Add(averageTicketValueForMonth);
                }

                dashboardMonthlyAverageTicketValue.Response = resultWithZerosForMissingData;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dashboardMonthlyAverageTicketValue;
        }
    }
}