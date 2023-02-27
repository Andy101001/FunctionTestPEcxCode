namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.EntityModels;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Parcs Ticket Occupancies Service
    /// </summary>
    public class TicketOccupanciesService : ITicketOccupanciesService
    {
        private readonly ILogger<TicketOccupanciesService> _logger;
        private readonly IDataAccessService _dataAccessService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PgsTicketOccupanciesService"/> class.
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="factory">ef factory</param>
        public TicketOccupanciesService(ILoggerFactory loggerFactory, IDataAccessService dataAccessService)
        {
            _logger = loggerFactory.CreateLogger<TicketOccupanciesService>();
            _dataAccessService = dataAccessService;

            _logger.LogInformation($"Constructing {nameof(TicketOccupanciesService)}");
        }

        /// <summary>
        /// Get all occupancies
        /// </summary>
        /// <returns><List<Occupancy></returns>
        public async Task<List<Occupancy>> GetOccupanciesAsync()
        {
            _logger.LogInformation("Getting all occupancies");

            var occupancies = await _dataAccessService.GetParcsTicketOccupanciesAsync().ConfigureAwait(false);

            return occupancies;
        }
    }
}