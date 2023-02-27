namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.DataAccess;
    using ABMVantage_Outbound_API.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    /// <summary>
    /// EF service for database reads
    /// </summary>
    public class DataAccessService : IDataAccessService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<DataAccessService> _logger;

        /// <summary>
        /// Factory to generate <see cref="DocsContext"/> instances.
        /// </summary>
        private readonly IDbContextFactory<CosmosDataContext> _dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessService"/> class.
        /// </summary>
        /// <param name="factory">The factory instance.</param>
        /// <param name="loggerFactory">Logger factory</param>
        public DataAccessService(IDbContextFactory<CosmosDataContext> dbContextFactory, ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(dbContextFactory);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<DataAccessService>();
            _dbContextFactory = dbContextFactory;

            _logger.LogInformation($"Constructing {nameof(DataAccessService)}");
        }

        /// <summary>
        /// Get active charging sessions
        /// </summary>
        /// <param name="id">optional id for a session</param>
        /// <returns>List<EvActiveSessions></returns>
        public async Task<List<EvActiveSessions>>? GetActiveChargingSessionsAsync(string? id = null)
        {
            _logger.LogInformation($"Getting {nameof(EvActiveSessions)}");

            using var context = _dbContextFactory.CreateDbContext();

            var evActiveSessions = await context.EvActiveSessions.ToListAsync();

            return evActiveSessions;
        }

        /// <summary>
        /// Get closed charging sessions
        /// </summary>
        /// <param name="id">Optional Id for Closed Sessions</param>
        /// <returns>List<EvClosedSessions></returns>
        public async Task<List<EvClosedSessions>>? GetClosedChargingSessionsAsync(string? id = null)
        {
            _logger.LogInformation($"Getting {nameof(EvClosedSessions)}");
            using var context = _dbContextFactory.CreateDbContext();

            var evClosedSessions = await context.EvClosedSessions.ToListAsync();

            return evClosedSessions;
        }

        /// <summary>
        /// Get Parcs Ticket Occupancies
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<Occupancy></returns>
        public async Task<List<Occupancy>>? GetParcsTicketOccupanciesAsync(string? id = null)
        {
            _logger.LogInformation($"Getting {nameof(Occupancy)}");
            using var context = _dbContextFactory.CreateDbContext();

            var parcsTicketOccupancies = await context.ParcsTickOccupanies.ToListAsync();

            return parcsTicketOccupancies;
        }

        /// <summary>
        /// Get PGS Ticket Occupancies
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<PgsOccupancy></returns>
        public async Task<List<PgsOccupancy>>? GetPgsTicketOccupanciesAsync(string? id = null)
        {
            _logger.LogInformation($"Getting {nameof(PgsOccupancy)}");

            using var context = _dbContextFactory.CreateDbContext();

            var pgsTicketOccupancies = await context.PgsTickOccupanies.ToListAsync();

            return pgsTicketOccupancies;
        }

        /// <summary>
        /// Returns a specific reservation for the dashboard
        /// </summary>
        /// <param name="id">reservation id to return</param>
        /// <returns>Reservation</returns>
        public async Task<Reservation> GetReservationsAsync(string id)
        {
            _logger.LogInformation($"Getting {nameof(Reservation)}");

            using var context = _dbContextFactory.CreateDbContext();

            var reservation = await context.Reservations
                            .WithPartitionKey(id)
                            .SingleOrDefaultAsync(d => d.Id == id);

            return reservation;
        }

        /// <summary>
        /// Get Reservation Transactions
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>List<ObsReservationTransactions></returns>
        public async Task<List<ObsReservationTransactions>>? GetReservationsTransactionsAsync(string? id = null)
        {
            _logger.LogInformation($"Getting {nameof(ObsReservationTransactions)}");

            using var context = _dbContextFactory.CreateDbContext();

            var obsReservationTransactions = await context.ReservationTransactions.ToListAsync();
            // Call a service method to do this biz for getting the transactions for the reservation

            return new List<ObsReservationTransactions> { new ObsReservationTransactions() };
        }

        /// <summary>
        /// Get Parcs Ticket Transactions
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>List<ParcsTicketsTransactions></returns>
        public async Task<List<ParcsTicketsTransactions>>? GetParcsTicketTransactionsAsync(string? id = null)
        {
            _logger.LogInformation($"Getting {nameof(ParcsTicketsTransactions)}");

            using var context = _dbContextFactory.CreateDbContext();

            var parcsTicketTransactions = await context.ParcsTicketsTransactions.ToListAsync();

            return parcsTicketTransactions;
        }
    }
}