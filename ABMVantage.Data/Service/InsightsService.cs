namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.DataAccess;
    using ABMVantage.Data.EntityModels.SQL;
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using ABMVantage.Data.Models.DashboardModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class InsightsService : ServiceBase, IInsightsService
    {
        private readonly ILogger<InsightsService> _logger;
        private readonly IDbContextFactory<CosmosDataContext> _factory;
        private readonly IDbContextFactory<SqlDataContextVTG> _sqlDataContextVTG;

        public InsightsService(ILoggerFactory loggerFactory, IRepository repository, IDbContextFactory<CosmosDataContext> factory, IDbContextFactory<SqlDataContextVTG> sqlDataContextVTG)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<InsightsService>();
            _repository = repository;
            _factory = factory;
            _sqlDataContextVTG = sqlDataContextVTG;
        }
        public async Task<DailyAverageOccupancy>? GetDailyAverageOccupancy(FilterParam? filterParameters)
        {
            DailyAverageOccupancy? dailyAverageOccupancy = new DailyAverageOccupancy() { AverageDailyOccupancyInteger = 0, AverageDailyOccupancyPercentage = 0 };

            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.InsightsAverageDialyOccupanySQLData.Where(x => facilities!.Contains(x.FacilityId!) 
                    && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                    && products!.Contains(x.ProductId)
                    && (x.Date >= filterParameters!.FromDate && x.Date < filterParameters.ToDate));

                int totalOccupiedParkingSpotHours = result.Sum(x => x.TotalOccupancy);
                int totalParkingSpaceCount = result.Sum(x => x.ParkingSpaceCount);

                if (totalParkingSpaceCount > 0)
                {
                    dailyAverageOccupancy.AverageDailyOccupancyInteger = totalOccupiedParkingSpotHours / (totalParkingSpaceCount * 24) * totalParkingSpaceCount;
                    dailyAverageOccupancy.AverageDailyOccupancyPercentage = totalOccupiedParkingSpotHours / (totalParkingSpaceCount * 24) * 100;
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
                         && (x.Day >= filterParameters!.FromDate && x.Day < filterParameters.ToDate));

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

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                          && (x.TransactionDate >= filterParameters!.FromDate && x.TransactionDate < filterParameters.ToDate));

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
                filterParameters.ToDate = filterParameters.FromDate.AddDays(1);

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

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.InsightsMonthlyRevenueAndBudgetSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
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

                var fResult = from RevenueAndBudgetForMonth rnb in gResult
                                select new RevenueAndBudget
                                {
                                    Date = new DateTime(rnb.Year, rnb.Month, 1),
                                    Revenue = rnb.Revenue,
                                    BudgetedRevenue = rnb.BudgetedRevenue
                                    };
                dashboardMonthlyRevenueAndBudget.MonthlyRevenueAndBudget = fResult.OrderBy(x => x.Date);
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

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.InsightsMonthlyParkingOccupancySQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                       && (x.FirstDayOfMonth >= filterParameters!.FromDate && x.FirstDayOfMonth < filterParameters.ToDate));

                //Group by Year and Month
                IEnumerable<OccupancyByMonth> gResult = new List<OccupancyByMonth>();
                if (result.Sum(x => x.ParkingSpaceCount) > 0) {
                    gResult  = result.GroupBy(x => new { x.FirstDayOfMonth.Year, x.FirstDayOfMonth.Month }).Select(g =>
                     new OccupancyByMonth
                     {
                         Year = g.Key.Year,
                         Month =  g.Key.Month,
                         OccupancyInteger =  ((g.Sum(x => x.TotalOccupancy) / g.Sum(x => x.ParkingSpaceCount * x.NoOFDaysInMonth * 24)) * g.Sum(x => x.ParkingSpaceCount)),
                         OccupancyPercentage = (g.Sum(x => x.TotalOccupancy) / g.Sum(x => x.ParkingSpaceCount * x.NoOFDaysInMonth * 24)) * 100,
                     });
                }

                for (DateTime monthStart = filterParameters!.FromDate; monthStart < filterParameters.ToDate; monthStart = monthStart.AddMonths(1))
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

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.InsightsMonthlyTransactionsSQLData.Where(x => facilities!.Contains(x.FacilityId!) && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null) && products!.Contains(x.ProductId)
                      && (x.FirstDayOfMonth >= filterParameters!.FromDate && x.FirstDayOfMonth < filterParameters.ToDate));

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