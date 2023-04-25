namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage.Data.Models;
    using ABMVantage_Outbound_API.Configuration;
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<DashboardDailyReservationCountByHour> GetHourlyReservationsByProduct(FilterParam filterParameters)
        {
            if (filterParameters == null || filterParameters.FromDate < _settings.MinimumValidCalculationDate || filterParameters.ToDate < _settings.MinimumValidCalculationDate)
            {
                throw new ArgumentException("Missing or invalid filter parameters.");
            }
            var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
            var queryResults = await _dataAccessSqlService.GetReservationByHourCountsAsync(queryParameters);
            var results = from ReservationsForProductAndHour res in queryResults
                          group res by res.Hour into hourlyGroup
                          select new HourlyReservationCount
                          {
                              ReservationTime = hourlyGroup.Key,
                              Data = hourlyGroup.Select(x => new ReservationsByProduct { NoOfReservations = x.ReservationCount, Product = x.Product })
                          };



            /*var results = from ReservationsForProductAndHour res in queryResults
                          group res by new { res.Year, res.Month, res.Day, res.Hour } into hourlyGroup
                          select new HourlyReservationCount
                          {
                              ReservationTime = hourlyGroup.Key.Hour.ToString() + ":00",
                              Data = hourlyGroup.Select(x => new ReservationsByProduct { NoOfReservations = x.ReservationCount, Product = x.Product })
                          };*/
            return new DashboardDailyReservationCountByHour { ReservationsByHour = results };
        }
    }
}