using ABMVantage_Outbound_API.DataAccess;
using ABMVantage_Outbound_API.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ABMVantage_Outbound_API.Services
{
    public class ObsReservationService : IObsReservationService
    {
        private readonly ILogger<ObsReservationService> _logger;
        private readonly IDataAccessService _dataAccessService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObsReservationService"/> class.
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="databaseFactory">db context</param>
        public ObsReservationService(ILoggerFactory loggerFactory, IDataAccessService dataAccessService)
        {
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<ObsReservationService>();
            _dataAccessService = dataAccessService;
        }

        public async Task<List<Booking>> GetAllReservationsAsync()
        {
            _logger.LogInformation("Getting all booking reservations");

            var bookings = await _dataAccessService.GetAllObsReservationsAsync();

            _logger.LogInformation("Finished Getting all booking reservations");

            return bookings;
        }

        public async Task<Booking> GetReservationAsync(string id)
        {
            _logger.LogInformation($"Getting booking for Id:{id}");
            
            var booking = await _dataAccessService.GetReservationAsync(id);

            _logger.LogInformation($"Finished Getting booking for Id:{id}");

            return booking;

        }
    }
}