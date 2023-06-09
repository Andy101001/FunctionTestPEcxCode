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
    using System;
    using System.Buffers.Text;
    using System.Threading.Tasks;
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
                        && (x.BeginningOfHour >= filterParameters!.FromDate && x.BeginningOfHour < filterParameters.ToDate));
                
                //Group by Product Name and Hour
                var gResult = result.GroupBy(x => new { x.ProductName, x.BeginningOfHour.TimeOfDay }).Select(g =>
                 new ReservationsForProductAndHour
                 {
                     Product = g.Key.ProductName,
                     Hour = g.Key.TimeOfDay,
                     ReservationCount = g.Sum(x => x.NoOfReservations)
                 }).OrderBy(x => x.Hour).ToList();
                
                //Group by Again for the UI Specifications
                var fResult = from ReservationsForProductAndHour res in gResult
                                group res by res.Hour into hourlyGroup 
                          select new HourlyReservationCount
                          {
                              ReservationDateTime =  new DateTime() + hourlyGroup.Key,
                              Data = hourlyGroup.Select(x => new ReservationsByProduct { NoOfReservations = x.ReservationCount, Product = x.Product! })
                          };
                //dashboardDailyReservationCountByHour.ReservationsByHour = fResult.OrderBy(x => x.ReservationDateTime).ToList();
                
                 dashboardDailyReservationCountByHour.ReservationsByHour = fResult;
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
                if ((filterParameters.ToDate.Subtract(filterParameters!.FromDate).TotalDays) > (12*30))
                {
                    //adding 11 month + 1 current date month
                    filterParameters.ToDate = filterParameters!.FromDate.AddMonths(11);
                }

                int monthInDateInetval = Convert.ToInt32((filterParameters.ToDate - filterParameters!.FromDate).TotalDays / 12);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueAndBudgetSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                       && (x.FirstDayOfMonth >= filterParameters!.FromDate && x.FirstDayOfMonth < filterParameters.ToDate));

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
                dashboardMonthlyRevenueAndBudget.MonthlyRevenueAndBudget = fResult.OrderBy(x => x.Date).ToList();

                foreach (var item in diff)
                {
                    if (!(dashboardMonthlyRevenueAndBudget.MonthlyRevenueAndBudget.Where(x => x.Date.Month == item.Date.Month).Count()>0))
                    {
                        dashboardMonthlyRevenueAndBudget.MonthlyRevenueAndBudget.Add(new RevenueAndBudget
                        {

                            Date = item.Date,
                            BudgetedRevenue=0,
                            Revenue=0
                        });
                    }
                }
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

                //should show data for 6 months based on the 'Start Date' filter. Full months of data should be shown regardless of date selected. 
                var fromDate = new DateTime(filterParameters!.FromDate.Year, filterParameters!.FromDate.Month, 1);
                var toDate = fromDate.AddMonths(6);
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
                filterParameters.ToDate= filterParameters!.FromDate.AddMonths(13);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.InsightsMonthlyTransactionsSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                      && (x.FirstDayOfMonth >= filterParameters!.FromDate && x.FirstDayOfMonth <= filterParameters.ToDate));

                var sql = result.ToQueryString();

                //Group by Product Name and Hour
                var gResult = result.GroupBy(x => new {x.FirstDayOfMonth.Year, x.FirstDayOfMonth.Month, x.ProductName }).Select(g =>
                 new TransactionsByMonthAndProduct
                 {
                     Year = g.Key.Year,
                     Month = g.Key.Month,
                     ParkingProduct = g.Key.ProductName,
                     TransactionCount = g.Sum( x => x.TransactionCount)
                 }).ToList();

                var fResults = from TransactionsByMonthAndProduct cnt in gResult
                              group cnt by new { cnt.Year, cnt.Month } into monthlyGroup
                              select new TransactionCountForMonth
                              {
                                  Date = new DateTime(monthlyGroup.Key.Year, monthlyGroup.Key.Month, 1),
                                  Data = monthlyGroup.Select(x => new TransactionsForProduct { NoOfTransactions = x.TransactionCount, Product = x.ParkingProduct })
                              };

             

                dashboardMonthlyTransactionCount.MonthlyTransactions = fResults.OrderBy(x => x.Date);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dashboardMonthlyTransactionCount;
        }
        public async Task<DashboardMonthlyAverageTicketValue> AverageTicketValuePerYear(FilterParam filterParameters)
        {
            DashboardMonthlyAverageTicketValue? dashboardMonthlyAverageTicketValue = new DashboardMonthlyAverageTicketValue();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.InsightsAverageMonthlyTicketValueSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                     && (x.FirstDayOfMonth >= filterParameters!.FromDate && x.FirstDayOfMonth < filterParameters.ToDate)).ToList();

                var fResult =  from InsightsAverageMonthlyTicketValueSQL data in result
                         group data by new { data.FirstDayOfMonth.Year , data.FirstDayOfMonth.Month } into monthlyGroup
                         select new AverageTicketValueForMonth
                         {
                             Date = new DateTime(monthlyGroup.Key.Year, monthlyGroup.Key.Month, 1), //monthlyGroup.Key.Year.ToString() + monthlyGroup.Key.Month.ToString(),
                             Data = monthlyGroup.Select(x => new TicketValueAverage { ParkingProduct = x.ProductName!, AverageTicketValue = Convert.ToInt32(x.AverageTicketValue) }).ToList()
                         };

                dashboardMonthlyAverageTicketValue.Response = fResult.OrderBy(x => x.Date);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dashboardMonthlyAverageTicketValue;
        }
    }
}