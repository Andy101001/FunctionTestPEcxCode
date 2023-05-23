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

        public ODService(ILoggerFactory loggerFactory, IRepository repository, IDbContextFactory<CosmosDataContext> factory)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<ODService>();
            _repository = repository;
            _factory = factory;
        }

        public async Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(FilterParam filterParameters)
        {
            IEnumerable<OccRevenueByProduct> occRevenueByProductList = null;
            try
            {
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                var result = context.OD_TotalOccupancyRevenueData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)).ToList();

                //Group by Product Name 
                occRevenueByProductList = (List<OccRevenueByProduct>)result.GroupBy(x => new { x.ProductName}).Select(g =>
                 new OccRevenueByProduct
                 {
                     Product = g.Key.ProductName,
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
            List<OccWeeklyOccByDuration> occWeeklyOccByDuration = null;
            try
            {
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                var result = context.OD_AllData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                && (x.OccupancyEntryDate >= filterParameters!.FromDate && x.OccupancyEntryDate < filterParameters.ToDate)).ToList();

                //Group by Duration
                occWeeklyOccByDuration = (List<OccWeeklyOccByDuration>)result.GroupBy(x => new { x.Duration }).Select(g =>
                 new OccWeeklyOccByDuration
                 {
                     Duration = g.Key.Duration,
                     TotalWeeklyOccupancy = g.Count()
                 }).ToList();

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
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                var result = context.OD_AllData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                && (x.OccupancyEntryDate >= filterParameters!.FromDate && x.OccupancyEntryDate < filterParameters.ToDate)).ToList();

                var start = DateTime.Today;
                var clockQuery = from offset in Enumerable.Range(0, 24)
                                 select start.AddMinutes(60 * offset);
                foreach (var time in clockQuery)
                {
                    DateTime tempTime = time.AddHours(1);
                    var fResult = result.FindAll(x => TimeSpan.Compare(x.OccupancyEntryDate.TimeOfDay, time.TimeOfDay) > 0
                    && TimeSpan.Compare(x.OccupancyExitDate!.TimeOfDay, tempTime.TimeOfDay) < 0);

                    occCurrent.Add(new OccCurrent()
                    {
                        Time = time.ToString("hh:mm tt"),
                        NoOfOccupiedParking = (fResult != null && fResult.Count > 0) ? fResult.Count : 0, 
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
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                var result = context.OD_AllData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                && (x.OccupancyEntryDate >= filterParameters!.FromDate && x.OccupancyEntryDate < filterParameters.ToDate)).ToList();

                //Group by Duration,Year and Month
                List<OccVsDurationGroupedResult> gResult = result.GroupBy(x => new { x.Duration, x.OccupancyEntryDate.Year, x.OccupancyEntryDate.Month }).Select(g =>
                 new OccVsDurationGroupedResult
                 {
                     Duration = g.Key.Duration,
                     Year = g.Key.Year,
                     Month = g.Key.Month,
                     NoOfVehicles = g.Count()
                 }).ToList();

                List<string> durations = new List<string>() { "0 - 60 MINS", "1 - 3 HOURS", "4 - 8 HOURS", "9 - 12 HOURS", "GREATER THAN 12 HOURS" };

                DateTime dt = new DateTime(filterParameters!.ToDate.Year, 1, 1);
                for(var m = dt.Month; m <= 12; m++)
                {
                    foreach(var duration in durations)
                    {
                        var matchFound = gResult.FirstOrDefault(x => x.Year == dt.Year && x.Month == m && x.Duration == duration);
                        avgMonthlyOccVsDurationList.Add(new AvgMonthlyOccVsDuration {
                            Duration = duration, 
                            Year = dt.Year,
                            NoOfVehicles = matchFound != null ? matchFound.NoOfVehicles : 0,
                            Month = dt.ToString("MMM")
                        });
                    }
                    dt = dt.AddMonths(1);
                }
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
                using var context = _factory.CreateDbContext();

                var levels = filterParameters?.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = filterParameters?.Facilities.Select(x => x.Id).ToList();
                var products = filterParameters?.Products.Select(x => x.Id).ToList();

                var result = context.OD_AllData.Where(x => (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty) && facilities!.Contains(x.FacilityId!) && products!.Contains(x.ProductId)
                && (x.OccupancyEntryDate >= filterParameters!.FromDate && x.OccupancyEntryDate < filterParameters.ToDate)).ToList();

                //Group by Duration,Year and Month
                List<OccVsDurationGroupedResult> gResult = result.GroupBy(x => new { x.OccupancyEntryDate.Year, x.OccupancyEntryDate.Month }).Select(g =>
                 new OccVsDurationGroupedResult
                 {
                     Year = g.Key.Year,
                     Month = g.Key.Month,
                     NoOfVehicles = g.Count()
                 }).ToList();

                DateTime dt = new DateTime(filterParameters!.ToDate.Year, 1, 1);
                DateTime dtTo = dt.AddYears(-1);
                List<DateTime> fiscals = new List<DateTime>() { dt, dtTo };
                fiscals.ForEach(fiscalDate =>
                {
                    for (var m = fiscalDate.Month; m <= 12; m++)
                    {
                        var matchFound = gResult.FirstOrDefault(x => x.Year == fiscalDate.Year && x.Month == m);
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
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return yearlyOccupancy;
        }
    }
}