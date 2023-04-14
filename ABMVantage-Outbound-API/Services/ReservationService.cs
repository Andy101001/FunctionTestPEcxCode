namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.DashboardFunctionModels;
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
        
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="dataAccessSqlService">sql</param>
        /// <param name="dataAccessService">cosmo</param>
        /// <param name="configuration">configuration</param>
        public ReservationService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService, IDataAccessService dataAccessService, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<ReservationService>();
            _dataAccessService = dataAccessService;
            _configuration = configuration;
            IsSqlDbConnectionOn = Convert.ToBoolean(_configuration.GetSection("SqlSettings")["IsSqlDbConnectionOn"]);

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
        public Task<List<ReservationByHour>> ReservationPerHour(HourlyReservationParameters hourlyReservationParameters)
        {
            //*************************************************************************
            // MOCK for now until I can connect to the database
            //*************************************************************************

            List<ReservationByHour> reservationByHour = new List<ReservationByHour>
            {
                new ReservationByHour
                {
                    ReservationTime = "05:00 AM",
                    Data = new List<ReservationByHourData>
                        {
                            new ReservationByHourData {Product = "EV",
                                NoOfReservations = 65 },
                            new ReservationByHourData {Product = "Valet",
                                NoOfReservations = 11 },
                            new ReservationByHourData {Product = "Premium",
                                NoOfReservations = 22 },
                            new ReservationByHourData {Product = "General",
                                NoOfReservations = 33 },
                            new ReservationByHourData {Product = "Monthly",
                                NoOfReservations = 44 }
                        }
                    },
                new ReservationByHour
            {
                ReservationTime = "06:00 AM",
                Data = new List<ReservationByHourData>
                        {
                            new ReservationByHourData {Product = "EV",
                                NoOfReservations = 1 },
                            new ReservationByHourData {Product = "Valet",
                                NoOfReservations = 3 },
                            new ReservationByHourData {Product = "Premium",
                                NoOfReservations = 6 },
                            new ReservationByHourData {Product = "General",
                                NoOfReservations = 7 },
                            new ReservationByHourData {Product = "Monthly",
                                NoOfReservations = 8 }
                        }
            }
            };
            
            return Task.FromResult(reservationByHour);
        }
    }
}