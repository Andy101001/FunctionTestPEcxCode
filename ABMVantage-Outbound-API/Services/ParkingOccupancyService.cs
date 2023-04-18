using ABMVantage.Data.Models;
using ABMVantage_Outbound_API.Configuration;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace ABMVantage_Outbound_API.Services
{
    public class ParkingOccupancyService : IParkingOccupancyService
    {
        private readonly DashboardFunctionSettings _settings;
        private readonly ILogger<ParkingOccupancyService> _logger;
        private readonly IDataAccessSqlService _dataAccessService;

        public ParkingOccupancyService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService, DashboardFunctionSettings settings)
        {
            _logger = loggerFactory.CreateLogger<ParkingOccupancyService>();
            _dataAccessService = dataAccessSqlService;
            _settings = settings;
        }
        public async Task<DashboardMonthlyParkingOccupancy> GetMonthlyParkingOccupancyAsync(FilterParam filterParameters)
        {
            _logger.LogInformation($"Getting Dashboard Monthly Paring Occupancy{nameof(GetMonthlyParkingOccupancyAsync)})");
            if (filterParameters == null || filterParameters.FromDate < _settings.MinimumValidCalculationDate || filterParameters.ToDate < _settings.MinimumValidCalculationDate)
            {
                throw new ArgumentException($"Calculation data must be greater than {_settings.MinimumValidCalculationDate})");
            }
            var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
            var currentYearOccupanciesByMonth = await _dataAccessService.GetMonthlyParkingOccupanciesAsync(queryParameters);
            var priorYearQueryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
            priorYearQueryParameters.FromDate = queryParameters.FromDate.AddYears(-1);
            priorYearQueryParameters.ToDate = queryParameters.ToDate.AddYears(-1);
            var priorYearOccupanciesByMonth = await _dataAccessService.GetMonthlyParkingOccupanciesAsync(queryParameters);
            var monthlyParkingOccupancyData = new List<ParkingOccupancy>();
            for (DateTime monthStart = filterParameters.FromDate; monthStart < filterParameters.ToDate; monthStart = monthStart.AddMonths(1))
            {
                var currentYearOccupancyByMonth = currentYearOccupanciesByMonth.Where(x => x.Year == monthStart.Year && x.Month == monthStart.Month).FirstOrDefault();
                var priorYearOccupancyByMonth = priorYearOccupanciesByMonth.Where(x => x.Year == monthStart.Year - 1 && x.Month == monthStart.Month).FirstOrDefault();
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
            return new DashboardMonthlyParkingOccupancy { MonthlyParkingOccupancy = monthlyParkingOccupancyData };

        }
    }
}
