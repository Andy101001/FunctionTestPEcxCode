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
        private readonly IDbContextFactory<CosmosDataContext> _factory;

        public ObsReservationService(ILoggerFactory loggerFactory, IDbContextFactory<CosmosDataContext> factory)
        {
            _logger = loggerFactory.CreateLogger<ObsReservationService>();
            _factory = factory;
        }

        public async Task<List<Booking>> GetAllReservationsAsync()
        {
            _logger.LogInformation("Getting all booking reservations");

            using var context = _factory.CreateDbContext();
            var bookings = await context.Booking.ToListAsync();

            _logger.LogInformation("Finished Getting all booking reservations");

            return bookings;
        }

        public async Task<Booking> GetReservationAsync(string id)
        {
            _logger.LogInformation($"Getting booking for Id:{id}");

            using var context = _factory.CreateDbContext();
            var booking = await context.Booking
                                    .WithPartitionKey(id)
                                    .SingleOrDefaultAsync(d => d.Id == id);

            _logger.LogInformation($"Finished Getting booking for Id:{id}");

            return booking;

        }
    }
}