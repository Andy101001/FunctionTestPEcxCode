using ABMVantage_Outbound_API.DataAccess;
using ABMVantage_Outbound_API.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ABMVantage_Outbound_API.Services
{
    public class ObsReservationService : IObsReservationService
    {
        private readonly ILogger<ObsReservationService> _logger;

        /// <summary>
        /// Factory to generate <see cref="DocsContext"/> instances.
        /// </summary>
        private readonly IDbContextFactory<CosmosDataContext> _databaseFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObsReservationService"/> class.
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="databaseFactory">db context</param>
        public ObsReservationService(ILoggerFactory loggerFactory, IDbContextFactory<CosmosDataContext> databaseFactory)
        {
            ArgumentNullException.ThrowIfNull(databaseFactory);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<ObsReservationService>();
            _databaseFactory = databaseFactory;
        }

        public async Task<List<Booking>> GetAllReservationsAsync()
        {
            _logger.LogInformation("Getting all booking reservations");

            using var context = _databaseFactory.CreateDbContext();
            var bookings = await context.Booking.ToListAsync();

            _logger.LogInformation("Finished Getting all booking reservations");

            return bookings;
        }

        public async Task<Booking> GetReservationAsync(string id)
        {
            _logger.LogInformation($"Getting booking for Id:{id}");

            using var context = _databaseFactory.CreateDbContext();
            var booking = await context.Booking
                                    .WithPartitionKey(id)
                                    .SingleOrDefaultAsync(d => d.Id == id);

            _logger.LogInformation($"Finished Getting booking for Id:{id}");

            return booking;

        }
    }
}