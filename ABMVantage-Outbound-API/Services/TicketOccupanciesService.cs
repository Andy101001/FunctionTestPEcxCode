namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.DataAccess;
    using ABMVantage_Outbound_API.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class TicketOccupanciesService : ITicketOccupanciesService
    {
        private readonly ILogger<ObsReservationService> _logger;

        /// <summary>
        /// Factory to generate <see cref="DocsContext"/> instances.
        /// </summary>
        private readonly IDbContextFactory<CosmosDataContext> _factory;
        
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="factory">ef factory</param>
        public TicketOccupanciesService(ILoggerFactory loggerFactory, IDbContextFactory<CosmosDataContext> factory)
        {
            _logger = loggerFactory.CreateLogger<ObsReservationService>();
            _factory = factory;
        }

        /// <summary>
        /// Get all occupancies
        /// </summary>
        /// <returns><List<Occupancy></returns>
        public async Task<List<Occupancy>> GetOccupanciesAsync()
        {
            _logger.LogInformation("Getting all occupancies");

            using var context = _factory.CreateDbContext();
            var occupancies = await context.ParcsTickOccupanies.ToListAsync();

            _logger.LogInformation("Finished Getting all occupancies");

            return occupancies;
        }
    }
}