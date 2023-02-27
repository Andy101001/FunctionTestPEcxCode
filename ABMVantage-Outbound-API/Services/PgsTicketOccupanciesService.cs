namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.EntityModels;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// PGS Ticket Occupancies Service
    /// </summary>
    public class PgsTicketOccupanciesService : IPgsTicketOccupanciesService
    {
        private readonly ILogger<PgsTicketOccupanciesService> _logger;
        private readonly IDataAccessService _dataAccessService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PgsTicketOccupanciesService"/> class.
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="dataAccessService">Data Access</param>
        public PgsTicketOccupanciesService(ILoggerFactory loggerFactory, IDataAccessService dataAccessService)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);
            ArgumentNullException.ThrowIfNull(dataAccessService);

            _logger = loggerFactory.CreateLogger<PgsTicketOccupanciesService>();
            _dataAccessService = dataAccessService;

            _logger.LogInformation($"Constructing {nameof(PgsTicketOccupanciesService)}");
        }

        /// <summary>
        /// Get all occupancies
        /// </summary>
        /// <returns><List<Occupancy></returns>
        public async Task<List<PgsOccupancy>> GetOccupanciesAsync()
        {
            _logger.LogInformation("Getting all PGS occupancies");

            var occupancies = await _dataAccessService.GetPgsTicketOccupanciesAsync().ConfigureAwait(false);

            return occupancies;
        }
    }
}