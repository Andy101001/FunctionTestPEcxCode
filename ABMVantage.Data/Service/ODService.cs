namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.DataAccess;
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using ABMVantage.Data.Models.DashboardModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    public class ODService : ServiceBase, IODService
    {
        private readonly ILogger<ODService> _logger;
        private readonly IDbContextFactory<CosmosDataContext> _factory;
        private readonly IDbContextFactory<SqlDataContextVTG> _sqlDataContextVTG;
        public ODService(ILoggerFactory loggerFactory, IRepository repository, IDbContextFactory<SqlDataContextVTG> sqlDataContextVTG, IDbContextFactory<CosmosDataContext> factory)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<ODService>();
            _repository = repository;
            _factory = factory;
            _sqlDataContextVTG = sqlDataContextVTG;
        }

        public async Task<OccRevenueByProductList> GetTotalOccRevenue(FilterParam filterParameters)
        {
            throw new NotImplementedException("This needs to be reimplemented if needed as the dependent table is not in sql anymore.");

            /*
            var occRevenueByProductList = new OccRevenueByProductList();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
                var toDate = filterParameters!.FromDate;
                var fromDate = toDate.AddDays(-1);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                occRevenueByProductList.OccRevenueByProduc = sqlContext.OccupancyRevenueSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                      && levels!.Contains(x.LevelId!) &&
                            products!.Contains(x.ProductId.Value!))
                            .GroupBy(x => new { x.ProductName }).Select(g =>
                                 new OccRevenueByProduct
                                 {
                                     Product = g.Key.ProductName!,
                                     Revenue = g.Sum(x => x.Amount)
                                 }).ToList();

                //UI display text for date range
                occRevenueByProductList.ToDate = toDate;
                occRevenueByProductList.FromDate = fromDate;

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

           

            
            return occRevenueByProductList;
            */
        }

        public async Task<OccWeeklyOccByDurationList> GetWeeklyOccByDuration(FilterParam filterParameters)
        {
            var occWeeklyOccByDuration = new OccWeeklyOccByDurationList() { IsDataForOneDate = true };
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
                var toDate = filterParameters!.FromDate;
                var fromDate = toDate.AddDays(-1);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                occWeeklyOccByDuration.OccWeeklyOccByDurations = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && levels!.Contains(x.LevelId!)
                      && products!.Contains(x.ProductId!)
                      && (x.TransactionDate>= fromDate && x.TransactionDate != null &&
                      x.TransactionDate < toDate && x.Duration != null
                      )).GroupBy(x => new { x.Duration }).Select(g =>
                 new OccWeeklyOccByDuration
                 {
                     Duration = g.Key.Duration!,
                     TotalWeeklyOccupancy = g.Count()
                 }).OrderBy(x => x.Duration).ToList();

                //UI display text for date range
                occWeeklyOccByDuration.ToDate = toDate;
                occWeeklyOccByDuration.FromDate = fromDate;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return occWeeklyOccByDuration;
        }

        public async Task<OccCurrentList> GetOccCurrent(FilterParam filterParameters)
        {
            var resultByHourWithZeroes = new OccCurrentList() { IsDataForOneDate = true };
            try
            {
                //Requirement expressed by Arjun and captured in story 2976 the data is from the 24 hours preceding the start date
                var toDate = filterParameters.FromDate;
                var fromDate = toDate.AddDays(-1);

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();

                //bug:5723 dated: 08/30/2023
                var result = sqlContext.OccupancyDetailSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && levels!.Contains(x.LevelId!)
                    && products!.Contains(x.ProductId!)
                      && x.BeginningOfHour >= fromDate && x.BeginningOfHour != null &&
                      x.BeginningOfHour <= toDate
                      ).ToList();

                int totalParkingSpaceCount = sqlContext.FacilityLevelProductSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                && levels!.Contains(x.LevelId!)
                && products!.Contains(x.ProductId)).Sum(x => x.ParkingSpaceCount);

                //Group by Hour
                var resultByHour = result.GroupBy(x => new DateTime(x.BeginningOfHour.Year, x.BeginningOfHour.Month, x.BeginningOfHour.Day, x.BeginningOfHour.Hour, 0,0)).Select(g =>
                 new OccCurrent
                 {
                     MonthInt = g.Key.Hour,
                     Time = g.Key.ToString("hh:mm tt"),
                     NoOfOccupiedParking = Math.Round(g.Sum(p => p.OccupiedMinutesForHour) / (60 * (decimal) totalParkingSpaceCount) * totalParkingSpaceCount,2)

                 }).ToList();

                for (DateTime beginningOfHour = fromDate; beginningOfHour < toDate; beginningOfHour = beginningOfHour.AddHours(1))
                {
                    var resultItem = resultByHour.FirstOrDefault(x => x.MonthInt == beginningOfHour.Hour);
                    if (resultItem  == null)
                    {
                        resultItem = new OccCurrent { MonthInt = beginningOfHour.Hour, Time = beginningOfHour.ToString("hh:mm tt"), NoOfOccupiedParking = 0};
                    }
                    resultByHourWithZeroes.OccCurrents.Add(resultItem);
                }

                //UI display text for date range
                resultByHourWithZeroes.ToDate = toDate;
                resultByHourWithZeroes.FromDate = fromDate;

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return resultByHourWithZeroes;
        }

        public async Task<AvgMonthlyOccVsDurationList> GetAvgMonthlyOccVsDuration(FilterParam filterParameters)
        {
            var avgMonthlyOccVsDurationWithZerosWhereThereIsNoData = new AvgMonthlyOccVsDurationList();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
                var toDate = new DateTime(filterParameters!.FromDate.Year, filterParameters!.FromDate.Month, 1); ;
                var fromDate = toDate.AddMonths(-13); //13 Months of data going back from start date -story 2977
                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.RevenueTransactionSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && levels!.Contains(x.LevelId!)
                    //&& products!.Contains(x.ProductId!.Value)
                      && (x.TransactionDate >= fromDate && x.TransactionDate != null &&
                      x.TransactionDate < toDate
                      )).GroupBy(x => new { x.Duration, x.TransactionDate!.Year, x.TransactionDate.Month }).Select(g =>
                         new OccVsDurationGroupedResult
                         {
                             FirstDayOfMonth = new DateTime(g.Key.Year, g.Key.Month, 1),
                             Duration = g.Key.Duration.Trim(),
                             Year = g.Key.Year,
                             Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                             NoOfVehicles =g.Count()
                         }).ToList();

                var avgMonthlyOccVsDuration = result.Select(x => new AvgMonthlyOccVsDuration { FirstDayOfMonth = x.FirstDayOfMonth, Duration = x.Duration, Month = x.Month, NoOfVehicles = x.NoOfVehicles, Year = x.Year }).ToList();

                //Add Zeros where there is no data

                string[] durations = new string[] { "0 - 60 MINS", "1 - 3 HOURS", "4 - 8 HOURS", "9 - 12 HOURS", "GREATER THAN 12 HOURS" };

                for (DateTime monthStart = fromDate; monthStart < toDate; monthStart = monthStart.AddMonths(1))
                {
                    foreach (var duration in durations)
                    {
                        var item = avgMonthlyOccVsDuration.FirstOrDefault(x => x.FirstDayOfMonth == monthStart && x.Duration == duration);
                        if (item == null)
                        {
                            item = new AvgMonthlyOccVsDuration { FirstDayOfMonth = monthStart, Duration = duration, Year = monthStart.Year, Month = monthStart.ToString("MMM"), NoOfVehicles = 0 };
                        }
                        avgMonthlyOccVsDurationWithZerosWhereThereIsNoData.AvgMonthlyOccVsDurations.Add(item);
                    }
                }
                //UI display text for date range
                avgMonthlyOccVsDurationWithZerosWhereThereIsNoData.ToDate = toDate;
                avgMonthlyOccVsDurationWithZerosWhereThereIsNoData.FromDate = fromDate;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            avgMonthlyOccVsDurationWithZerosWhereThereIsNoData.AvgMonthlyOccVsDurations.OrderBy(x => x.FirstDayOfMonth);
            return avgMonthlyOccVsDurationWithZerosWhereThereIsNoData;
        }

        public async Task<YearlyOccupancyList> GetYearlyOccupancy(FilterParam filterParameters)
        {
           var yearlyOccupancyWithZerosWhereThereIsNoData = new YearlyOccupancyList { YearlyOccupancies = new List<YearlyOccupancy>()};
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
               
                //Bug: 5395 Dated: 08/17/2022
                var toDate = new DateTime(filterParameters!.FromDate.Year, filterParameters!.FromDate.Month, 1);
                var fromDate = toDate.AddMonths(-13); //13 Months of data going back from start date -story 2978

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();

                var test = sqlContext.InsightsMonthlyParkingOccupancySQLData.Where(x => facilities!.Contains(x.FacilityId!)
                      && x.FirstDayOfMonth >= fromDate && x.FirstDayOfMonth < toDate
                      && levels.Contains(x.LevelId!)
                      && products.Contains(x.ProductId)).ToList();

                var currentYearResult = sqlContext.InsightsMonthlyParkingOccupancySQLData.Where(x => facilities!.Contains(x.FacilityId!)
                      && x.FirstDayOfMonth >= fromDate && x.FirstDayOfMonth < toDate
                      && levels.Contains(x.LevelId!)
                      && products.Contains(x.ProductId)
                      ).GroupBy(x => x.FirstDayOfMonth
                      ).Select(g =>
                         new YearlyOccupancy
                         {
                             FirstDayOfMonth = g.Key,
                             Occupancy = Math.Round((decimal)g.Sum(x => x.TotalOccupancyInMinutes) * (decimal)g.Sum(x => x.ParkingSpaceCount) / ((decimal)g.Sum(x => x.ParkingSpaceCount) * g.Min(x => x.NumberOFDaysInMonth) * 24 * 60), 2),
                             OccupancyPercentage = Math.Round((decimal)g.Sum(x => x.TotalOccupancyInMinutes) / ((decimal)g.Sum(x => x.ParkingSpaceCount) * g.Min(x => x.NumberOFDaysInMonth) * 24 * 60) * 100,2),
                             Fiscal = "CURRENT",
                             Year = g.Key.Year,
                             Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM")
                         }).ToArray();

                var previousYearResults = sqlContext.InsightsMonthlyParkingOccupancySQLData.Where(x => facilities!.Contains(x.FacilityId!)
                      && x.FirstDayOfMonth >= fromDate.AddYears(-1) && x.FirstDayOfMonth < toDate.AddYears(-1)
                      && levels.Contains(x.LevelId!)
                      && products.Contains(x.ProductId)
                      ).GroupBy(x => x.FirstDayOfMonth
                     ).Select(g =>
                        new YearlyOccupancy
                        {
                            FirstDayOfMonth = g.Key,
                            Occupancy = Math.Round((decimal)g.Sum(x => x.TotalOccupancyInMinutes) * (decimal)g.Sum(x => x.ParkingSpaceCount) / ((decimal)g.Sum(x => x.ParkingSpaceCount) * g.Min(x => x.NumberOFDaysInMonth) * 24 * 60),2),
                            OccupancyPercentage = Math.Round((decimal)g.Sum(x => x.TotalOccupancyInMinutes) / ((decimal)g.Sum(x => x.ParkingSpaceCount) * g.Min(x => x.NumberOFDaysInMonth) * 24 * 60) * 100, 2),
                            Fiscal = "PREVIOUS",
                            Year = g.Key.Year,
                            Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM")
                         }).ToArray();

                for (DateTime monthStart = fromDate; monthStart < toDate; monthStart = monthStart.AddMonths(1))
                {
                    var currentYearOccupancy = currentYearResult.FirstOrDefault(x => x.FirstDayOfMonth == monthStart);
                    if (currentYearOccupancy == null)
                    {
                        currentYearOccupancy = new YearlyOccupancy { FirstDayOfMonth = monthStart, Occupancy = 0, Fiscal = "CURRENT", Year = monthStart.Year, Month = monthStart.ToString("MMM") };
                    }
                    yearlyOccupancyWithZerosWhereThereIsNoData.YearlyOccupancies.Add(currentYearOccupancy);
                }
            
                for (DateTime monthStart = fromDate.AddYears(-1); monthStart < toDate.AddYears(-1); monthStart = monthStart.AddMonths(1))
                {
                    var previousYearOccupancy = previousYearResults.FirstOrDefault(x => x.FirstDayOfMonth == monthStart);
                    if (previousYearOccupancy == null)
                    {
                        previousYearOccupancy = new YearlyOccupancy { FirstDayOfMonth = monthStart, Occupancy = 0, Fiscal = "PREVIOUS", Year = monthStart.Year, Month = monthStart.ToString("MMM") };
                    }
                    yearlyOccupancyWithZerosWhereThereIsNoData.YearlyOccupancies.Add(previousYearOccupancy);
                }

                //UI display text for date range
                yearlyOccupancyWithZerosWhereThereIsNoData.ToDate = toDate;
                yearlyOccupancyWithZerosWhereThereIsNoData.FromDate = fromDate;


            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return yearlyOccupancyWithZerosWhereThereIsNoData;
        }
        private string GetHourAMPM(int hour)
        {
            string hourAMPM = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} {hour}:00:00.000";

            var dt = DateTime.Parse(hourAMPM);
            return dt.ToString("hh:mm tt");
        }
    }
}