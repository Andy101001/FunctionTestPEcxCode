namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.DataAccess;
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
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

        public async Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(FilterParam filterParameters)
        {
            IEnumerable<OccRevenueByProduct> occRevenueByProductList = new List<OccRevenueByProduct>();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
                var toDate = filterParameters!.FromDate;
                var fromDate = toDate.AddDays(-1);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                occRevenueByProductList = sqlContext.OccupancyRevenueSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                      && (x.LevelId == string.Empty || x.LevelId == null || levels!.Contains(x.LevelId!)) &&
                            (x.ProductId == null || products!.Contains(x.ProductId.Value!)))
                            .GroupBy(x => new { x.ProductName }).Select(g =>
                                 new OccRevenueByProduct
                                 {
                                     Product = g.Key.ProductName!,
                                     Revenue = g.Sum(x => x.Amount)
                                 }).ToList();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return occRevenueByProductList;
        }

        public async Task<IEnumerable<OccWeeklyOccByDuration>> GetWeeklyOccByDuration(FilterParam filterParameters)
        {
            List<OccWeeklyOccByDuration> occWeeklyOccByDuration = new List<OccWeeklyOccByDuration>();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
                var toDate = filterParameters!.FromDate;
                var fromDate = toDate.AddDays(-1);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                occWeeklyOccByDuration = sqlContext.OccupancyVsDurationSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                      && (x.OccupancyEntryDateTimeUtc >= fromDate && x.OccupancyExitDateTimeUtc != null &&
                      x.OccupancyEntryDateTimeUtc < toDate
                      )).GroupBy(x => new { x.Duration }).Select(g =>
                 new OccWeeklyOccByDuration
                 {
                     Duration = g.Key.Duration!,
                     TotalWeeklyOccupancy = g.Count()
                 }).OrderBy(x => x.Duration).ToList();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return occWeeklyOccByDuration;
        }

        public async Task<IEnumerable<OccCurrent>> GetOccCurrent(FilterParam filterParameters)
        {
            List<OccCurrent> occCurrent = new List<OccCurrent>();
            try
            {
                //Requirement expressed by Arjun and captured in story 2976 the data is from the 24 hours preceding the start date
                var toDate = filterParameters.FromDate;
                var fromDate = toDate.AddDays(-1);
                    
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.OccupancyVsDurationSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                      && (x.OccupancyEntryDateTimeUtc >= fromDate && x.OccupancyExitDateTimeUtc != null &&
                      x.OccupancyEntryDateTimeUtc < toDate
                      )).ToList();
             
                var start = fromDate;
                var clockQuery = from offset in Enumerable.Range(0, 24)
                                 select start.AddMinutes(60 * offset);
                foreach (var time in clockQuery)
                {
                    DateTime tempTime = time.AddHours(1);
                    var fResult = result.FindAll(x => TimeSpan.Compare(x.OccupancyEntryDateTimeUtc!.Value.TimeOfDay, time.TimeOfDay) > 0
                    && TimeSpan.Compare(x.OccupancyExitDateTimeUtc!.Value.TimeOfDay, tempTime.TimeOfDay) < 0);

                    occCurrent.Add(new OccCurrent()
                    {
                        Time = time.ToString("hh:mm tt"),
                        NoOfOccupiedParking = (fResult != null && fResult.Count() > 0) ? fResult.Count() : 0, 
                    });
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return occCurrent;
        }

        public async Task<IEnumerable<AvgMonthlyOccVsDuration>> GetAvgMonthlyOccVsDuration(FilterParam filterParameters)
        {
            List<AvgMonthlyOccVsDuration> avgMonthlyOccVsDurationList = new List<AvgMonthlyOccVsDuration>();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
                var toDate = filterParameters!.FromDate;
                var fromDate = toDate.AddMonths(-13); //13 Months of data going back from start date -story 2977
                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.OccupancyVsDurationSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                      && (x.OccupancyEntryDateTimeUtc >= fromDate && x.OccupancyExitDateTimeUtc != null &&
                      x.OccupancyEntryDateTimeUtc < toDate
                      )).GroupBy(x => new { x.Duration, x.OccupancyEntryDateTimeUtc!.Value.Year, x.OccupancyEntryDateTimeUtc.Value.Month }).Select(g =>
                         new OccVsDurationGroupedResult
                         {
                             Duration = g.Key.Duration,
                             Year = g.Key.Year,
                             Month = new DateTime (g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                             NoOfVehicles = g.Count()
                         });

                avgMonthlyOccVsDurationList = result.Select(x => new AvgMonthlyOccVsDuration { Duration = x.Duration, Month = x.Month, NoOfVehicles = x.NoOfVehicles, Year = x.Year  }).ToList();

                /*List<string> durations = new List<string>() { "0 - 60 MINS", "1 - 3 HOURS", "4 - 8 HOURS", "9 - 12 HOURS", "GREATER THAN 12 HOURS" };
                DateTime dt = new DateTime(filterParameters!.ToDate.Year, 1, 1);
                for(var m = dt.Month; m <= 12; m++)
                {
                    foreach(var duration in durations)
                    {
                        var matchFound = result.FirstOrDefault(x => x.Year == dt.Year && x.Month == m && x.Duration == duration);
                        avgMonthlyOccVsDurationList.Add(new AvgMonthlyOccVsDuration {
                            Duration = duration, 
                            Year = dt.Year,
                            NoOfVehicles = matchFound != null ? matchFound.NoOfVehicles : 0,
                            Month = dt.ToString("MMM")
                        });
                    }
                    dt = dt.AddMonths(1);
                }*/
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return avgMonthlyOccVsDurationList;
        }

        public async Task<IEnumerable<YearlyOccupancy>> GetYearlyOccupancy(FilterParam filterParameters)
        {
            List<YearlyOccupancy> yearlyOccupancy = new List<YearlyOccupancy>();
            try
            {
                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();
                var toDate = filterParameters!.FromDate;
                var fromDate = toDate.AddMonths(-13); //13 Months of data going back from start date -story 2978
                
                using var sqlContext = _sqlDataContextVTG.CreateDbContext();


                var currentYearResult = sqlContext.OccupancyVsDurationSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                      && (x.OccupancyEntryDateTimeUtc >= fromDate && x.OccupancyExitDateTimeUtc != null &&
                      x.OccupancyEntryDateTimeUtc < toDate
                      )).GroupBy(x => new { x.OccupancyEntryDateTimeUtc!.Value.Year, x.OccupancyEntryDateTimeUtc!.Value.Month }).Select(g =>
                         new YearlyOccupancyGroupedResult
                         {
                             FirstDayOfMonth = new DateTime(g.Key.Year, g.Key.Month, 1),
                             Occupancy = g.Count()
                         }).ToArray();

                var previousYearResults = sqlContext.OccupancyVsDurationSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                            && (x.OccupancyEntryDateTimeUtc >= fromDate.AddYears(-1) && x.OccupancyExitDateTimeUtc != null &&
                            x.OccupancyEntryDateTimeUtc < toDate.AddYears(-1)         
                         )).GroupBy(x => new { x.OccupancyEntryDateTimeUtc!.Value.Year, x.OccupancyEntryDateTimeUtc!.Value.Month }).Select(g =>
                         new YearlyOccupancyGroupedResult
                         {
                             FirstDayOfMonth = new DateTime(g.Key.Year, g.Key.Month, 1),
                             Occupancy = g.Count()
                         }).ToArray();

                yearlyOccupancy = currentYearResult.Select(x => new YearlyOccupancy { Year = x.FirstDayOfMonth.Year, Month = x.FirstDayOfMonth.ToString("MMM"), Occupancy = x.Occupancy, Fiscal = "CURRENT" }).ToList();
                yearlyOccupancy.AddRange(previousYearResults.Select(x => new YearlyOccupancy { Year = x.FirstDayOfMonth.Year, Month = x.FirstDayOfMonth.ToString("MMM"), Occupancy = x.Occupancy, Fiscal = "PREVIOUS" }).ToList());


                /*
                DateTime dt = new DateTime(filterParameters!.ToDate.Year, 1, 1);
                DateTime dtTo = dt.AddYears(-1);
                List<DateTime> fiscals = new List<DateTime>() { dt, dtTo };
                fiscals.ForEach(fiscalDate =>
                {
                    for (var m = fiscalDate.Month; m <= 12; m++)
                    {
                        var matchFound = result.FirstOrDefault(x => x.Year == fiscalDate.Year && x.Month == m);
                        yearlyOccupancy.Add(new YearlyOccupancy
                        {
                            Fiscal = fiscalDate.Year == filterParameters!.ToDate.Year ? "CURRENT" : "PREVIOUS",
                            Year = fiscalDate.Year,
                            Occupancy = matchFound != null ? matchFound.NoOfVehicles : 0,
                            Month = fiscalDate.ToString("MMM")
                        });
                        fiscalDate = fiscalDate.AddMonths(1);
                    }
                });
                */
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return yearlyOccupancy;
        }
    }
}