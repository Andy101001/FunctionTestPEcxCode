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
        public async Task<DashboardMonthlyParkingOccupancy> GetMonthlyParkingOccupancyAsync(DateTime calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            _logger.LogInformation($"Getting Dashboard Monthly Paring Occupancy{nameof(GetMonthlyParkingOccupancyAsync)})");
            if (calculationDate < _settings.MinimumValidCalculationDate)
            {
                throw new ArgumentException($"Calculation data must be greater than {_settings.MinimumValidCalculationDate})");
            }
            var startDate = new DateTime(calculationDate.Year, calculationDate.Month, 1);
            var endDate = startDate.AddMonths(_settings.MonthlyParkingOccupancyInterval).AddDays(-1);
            var currentYearOccupanciesByMonth = await _dataAccessService.GetMonthlyParkingOccupanciesAsync(startDate, endDate, facilityId, levelId, parkingProductId);
            var priorYearOccupanciesByMonth = await _dataAccessService.GetMonthlyParkingOccupanciesAsync(startDate.AddYears(-1), endDate.AddYears(-1), facilityId, levelId, parkingProductId);
            var monthlyParkingOccupancyData = new List<ParkingOccupancy>();
            for (DateTime monthStart = startDate; monthStart < endDate; monthStart = monthStart.AddMonths(1))
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
