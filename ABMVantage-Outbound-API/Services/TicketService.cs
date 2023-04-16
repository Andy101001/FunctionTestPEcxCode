namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.Configuration;
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Service for all things to do with tickets
    /// </summary>
    public class TicketService : ITicketService
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
        public TicketService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService, IDataAccessService dataAccessService, IConfiguration configuration, DashboardFunctionSettings settings)
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
        /// This api will take in a calculation date, and possible filters for facility, level and product
        /// and will return the total transactions by month for 13 months after the start date.For example,
        /// if the calculation date is 2021-01-11, the api returns transactions by month for 2021/01/01 through 2022/01/31
        /// </summary>
        /// <param name="hourlyReservationParameters">Date, facilityId, levelId, and parkingProductId </param>
        /// <returns>DashboardMonthlyAverageTicketValue</returns>
        public async Task<List<DashboardMonthlyAverageTicketValue>> AverageTicketValuePerYear(TicketPerYearParameters ticketPerYearParameters)
        {
            _logger.LogInformation($"Getting Dashboard Hourly Reservation Count {nameof(AverageTicketValuePerYear)}");

            if (ticketPerYearParameters.StartDate < _settings.MinimumValidCalculationDate)
            {
                throw new ArgumentException($"Calculation date must be greater than {_settings.MinimumValidCalculationDate}");
            }
            var ticketValuesPerYear = await _dataAccessSqlService.GetAverageTicketValuePerYearAsync(ticketPerYearParameters);

            return ticketValuesPerYear.ToList();
        }
    }
}