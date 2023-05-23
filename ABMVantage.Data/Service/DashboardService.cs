﻿namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.DataAccess;
    using ABMVantage.Data.EntityModels;
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using Microsoft.Azure.Cosmos.Serialization.HybridRow;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using ABMVantage.Data.Models.DashboardModels;

    public class DashboardService : ServiceBase, IDashboardService
    {
        private readonly ILogger<DashboardService> _logger;
        private readonly IDbContextFactory<CosmosDataContext> _factory;

        public DashboardService(ILoggerFactory loggerFactory, IRepository repository, IDbContextFactory<CosmosDataContext> factory)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardService>();
            _repository = repository;
            _factory = factory;
        }
        public async Task<DailyAverageOccupancy>? GetDailyAverageOccupancy(FilterParam? filterParameters)
        {
            DailyAverageOccupancy? dailyAverageOccupancy = new DailyAverageOccupancy() { AverageDailyOccupancyInteger = 0, AverageDailyOccupancyPercentage = 0 };

            try
            {
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
                
                var result = context.Dashboard_AverageDialyOccupanyData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                && (x.Day >= filterParameters!.FromDate && x.Day < filterParameters.ToDate));

                int totalOccupiedParkingSpotHours = result.Sum(x => x.TOTAL_OCCUPIED_PARKING_SPOT_HOURS_FOR_DAY);
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

        public async Task<decimal> GetDailyTotalRevenueAsync(FilterParam filterParameters)
        {
            decimal totalRevenue = 0;
            try
            {
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                var result = context.Dashboard_TotalRevenueData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
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
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                var result = context.Dashboard_TotalTransactionsData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                         && (x.Day >= filterParameters!.FromDate && x.Day < filterParameters.ToDate));

                totalTransactionsCount = result.Sum(x => x.TransactionCount);

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
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                var result = context.Dashboard_HourlyReservationsData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                         && (x.BeginingHour >= filterParameters!.FromDate && x.BeginingHour < filterParameters.ToDate)).ToList();

                //Group by Product Name and Hour
                var gResult = result.GroupBy(x => new { x.ProductName, x.BeginingHour }).Select(g =>
                 new ReservationsForProductAndHour
                 {
                     Product = g.Key.ProductName,
                     Hour = g.Key.BeginingHour,
                     ReservationCount = g.Sum(x => x.ReservationCount)
                 });

                //Group by Again for the UI Specifications
                var fResult = from ReservationsForProductAndHour res in gResult
                                group res by res.Hour into hourlyGroup
                          select new HourlyReservationCount
                          {
                              ReservationTime = hourlyGroup.Key.ToString("h:mm tt"),
                              Data = hourlyGroup.Select(x => new ReservationsByProduct { NoOfReservations = x.ReservationCount, Product = x.Product })
                          };

                dashboardDailyReservationCountByHour.ReservationsByHour = fResult.ToList();
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
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                var result = context.Dashboard_MonthlyRevenueAndBudgetData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                         && (x.FirstDayofMonth >= filterParameters!.FromDate && x.FirstDayofMonth < filterParameters.ToDate)).ToList();

                //Group by Year and Month
                var gResult = result.GroupBy(x => new { x.FirstDayofMonth.Year, x.FirstDayofMonth.Month }).Select(g =>
                 new RevenueAndBudgetForMonth
                 {
                     Year = g.Key.Year,
                     Month = g.Key.Month,
                     Revenue = g.Sum(x => x.Revenue),
                     BudgetedRevenue = g.Sum(x => x.BudgetedRevenue)
                 });

                dashboardMonthlyRevenueAndBudget.MonthlyRevenueAndBudget = from RevenueAndBudgetForMonth rnb in gResult
                                                                           select new RevenueAndBudget
                                                                           {
                                                                               Month = rnb.Year.ToString() + rnb.Month.ToString().PadLeft(2, '0'),
                                                                               Revenue = rnb.Revenue,
                                                                               BudgetedRevenue = rnb.BudgetedRevenue
                                                                           };
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
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                filterParameters!.FromDate = filterParameters.FromDate.AddYears(-1);
                filterParameters!.ToDate = filterParameters.ToDate.AddYears(-1);

                var result = context.Dashboard_MonthlyParkingOccupancyData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                         && (x.FirstDayofMonth >= filterParameters!.FromDate && x.FirstDayofMonth < filterParameters.ToDate)).ToList();

                //Group by Year and Month
                IEnumerable<OccupancyByMonth> gResult = new List<OccupancyByMonth>();
                if (result.Sum(x => x.ParkingSpaceCount) > 0) {
                    gResult  = result.GroupBy(x => new { x.FirstDayofMonth.Year, x.FirstDayofMonth.Month }).Select(g =>
                     new OccupancyByMonth
                     {
                         Year = g.Key.Year,
                         Month =  g.Key.Month,
                         OccupancyInteger =  ((g.Sum(x => x.TotalOccupiedParkingSpotHoursForMonth) / g.Sum(x => x.ParkingSpaceCount * x.NumberOfDaysInMonth * 24)) * g.Sum(x => x.ParkingSpaceCount)),
                         OccupancyPercentage = (g.Sum(x => x.TotalOccupiedParkingSpotHoursForMonth) / g.Sum(x => x.ParkingSpaceCount * x.NumberOfDaysInMonth * 24)) * 100,
                     });
                }

                for (DateTime monthStart = filterParameters.FromDate; monthStart < filterParameters.ToDate; monthStart = monthStart.AddMonths(1))
                {
                    var currentYearOccupancyByMonth = gResult.FirstOrDefault(x => x.Year == monthStart.Year && x.Month == monthStart.Month);
                    var priorYearOccupancyByMonth = gResult.FirstOrDefault(x => x.Year == monthStart.Year - 1 && x.Month == monthStart.Month);
                    var parkingOccupancy = new ParkingOccupancy();
                    parkingOccupancy.Month = $"{monthStart.Month}{monthStart.Year}";
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
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                var result = context.Dashboard_MonthlyTransactionsData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                        && (x.FirstDayofMonth >= filterParameters!.FromDate && x.FirstDayofMonth < filterParameters.ToDate)).ToList();

                //Group by Product Name and Hour
                var gResult = result.GroupBy(x => new {x.FirstDayofMonth.Year, x.FirstDayofMonth.Month, x.ProductName }).Select(g =>
                 new TransactionsByMonthAndProduct
                 {
                     Year = g.Key.Year,
                     Month = g.Key.Month,
                     ParkingProduct = g.Key.ProductName,
                     TransactionCount = g.Sum( x => x.TransactionCount)
                 });

                var fResults = from TransactionsByMonthAndProduct cnt in gResult
                              group cnt by new { cnt.Year, cnt.Month } into monthlyGroup
                              select new TransactionCountForMonth
                              {
                                  Month = monthlyGroup.Key.Year.ToString() + monthlyGroup.Key.Month.ToString(),
                                  Data = monthlyGroup.Select(x => new TransactionsForProduct { NoOfTransactions = x.TransactionCount, Product = x.ParkingProduct })
                              };
                dashboardMonthlyTransactionCount.MonthlyTransactions = fResults;

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
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();


                var result = context.Dashboard_AverageMonthlyTicketValueData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                      && (x.FirstDayofMonth >= filterParameters!.FromDate && x.FirstDayofMonth < filterParameters.ToDate)).ToList();


                var fResult = from Dashboard_AverageMonthlyTicketValue data in result
                         group data by new { data.FirstDayofMonth.Year , data.FirstDayofMonth.Month } into monthlyGroup
                         select new AverageTicketValueForMonth
                         {
                             Month = monthlyGroup.Key.Year.ToString() + monthlyGroup.Key.Month.ToString(),
                             Data = monthlyGroup.Select(x => new TicketValueAverage { ParkingProduct = x.ProductName, AverageTicketValue = Convert.ToInt32(x.AverageTicketValue) }).ToList()
                         };

                dashboardMonthlyAverageTicketValue.Response = fResult;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dashboardMonthlyAverageTicketValue;
        }
    }
}