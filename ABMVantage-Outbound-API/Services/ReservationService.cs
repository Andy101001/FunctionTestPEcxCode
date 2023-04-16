namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.Configuration;
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Service for all things to do with reservations
    /// </summary>
    public class ReservationService : IReservationService
    {
        private readonly ILogger<ReservationService> _logger;
        private readonly IDataAccessSqlService _dataAccessSqlService;
        private readonly IDataAccessService _dataAccessService;
        private readonly IConfiguration _configuration;
        private readonly bool IsSqlDbConnectionOn;
        private DashboardFunctionSettings _settings;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="dataAccessSqlService">sql</param>
        /// <param name="dataAccessService">cosmo</param>
        /// <param name="configuration">configuration</param>
        public ReservationService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService, IDataAccessService dataAccessService, IConfiguration configuration, DashboardFunctionSettings settings)
        {
            _logger = loggerFactory.CreateLogger<ReservationService>();
            _dataAccessService = dataAccessService;
            _configuration = configuration;
            IsSqlDbConnectionOn = Convert.ToBoolean(_configuration.GetSection("SqlSettings")["IsSqlDbConnectionOn"]);
            _settings = settings;
            _dataAccessSqlService = dataAccessSqlService;

            _logger.LogInformation($"Constructing {nameof(ReservationService)}");
        }

        /// <summary>
        /// The number of reservations for each hour of the day by hour.
        /// This is calculated as the number of reservations for which the reservation date and time range intersect the hour.
        /// For example, the reservations for hour 9:00 (9AM to 10AM) is the total number of reservations for which the start date/time is before 10AM,
        /// or the end date/time is after 9AM.
        /// </summary>
        /// <param name="hourlyReservationParameters">Date, facilityId, levelId, and parkingProductId </param>
        /// <returns>ReservationByHour</returns>
        public async Task<List<ReservationByHour>> ReservationPerHour(HourlyReservationParameters hourlyReservationParameters)
        {            
            _logger.LogInformation($"Getting Dashboard Hourly Reservation Count {nameof(ReservationPerHour)}");

            if (hourlyReservationParameters.calculationDate < _settings.MinimumValidCalculationDate)
            {
                throw new ArgumentException($"Calculation date must be greater than {_settings.MinimumValidCalculationDate}");
            }
            var reservationByHour = await _dataAccessSqlService.GetReservationByHourCountsAsync(hourlyReservationParameters);
            
            return reservationByHour.ToList();
        }
    }
}