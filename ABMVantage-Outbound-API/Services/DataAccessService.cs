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
        public async Task<List<Occupancy>>? GetParcsTicketOccupanciesAsync()
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
        public async Task<List<PgsOccupancy>>? GetPgsTicketOccupanciesAsync()
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
        public async Task<List<ObsReservationTransactions>>? GetReservationsTransactionsAsync()
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
        public async Task<List<ParcsTicketsTransactions>>? GetParcsTicketTransactionsAsync()
        {
            _logger.LogInformation($"Getting {nameof(ParcsTicketsTransactions)}");

            using var context = _dbContextFactory.CreateDbContext();

            var parcsTicketTransactions = await context.ParcsTicketsTransactions.ToListAsync();

            return parcsTicketTransactions;
        }

        /// <summary>
        /// Get OBS Reservations
        /// </summary>
        /// <returns>List<Booking></returns>
        public async Task<List<Booking>>? GetAllObsReservationsAsync()
        {
            _logger.LogInformation("Getting all booking reservations");

            using var context = _dbContextFactory.CreateDbContext();
            var bookings = await context.Booking.ToListAsync();

            _logger.LogInformation("Finished Getting all booking reservations");

            return bookings;
        }

        /// <summary>
        /// Get a OBS Reservation
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Booking</returns>
        public async Task<Booking> GetReservationAsync(string id)
        {
            _logger.LogInformation($"Getting booking for Id:{id}");

            using var context = _dbContextFactory.CreateDbContext();
            var booking = await context.Booking
                                    .WithPartitionKey(id)
                                    .SingleOrDefaultAsync(d => d.Id == id);

            _logger.LogInformation($"Finished Getting booking for Id:{id}");

            return booking;
        }

        public async Task<List<Product>> GetProductAsync(string id)
        {
            _logger.LogInformation($"Getting product for Id:{id}");

            var lstProduct=new List<Product>();
            lstProduct.Add(new Product
            {
                Id=101,
                LevelName="None EV"
            });
            lstProduct.Add(new Product
            {
                Id = 102,
                LevelName = "EV"
            });

            _logger.LogInformation($"Finished Getting product for Id:{id}");

            return await Task.FromResult<List<Product>>(lstProduct);
        }

        public async Task<List<Level>> GetLevelAsync(string id)
        {
            var lstLevel = new List<Level>();
            lstLevel.Add(new Level
            {
                Id = 101,
                LevelName = "Level 1"
            });
            lstLevel.Add(new Level
            {
                Id = 102,
                LevelName = "Level 2"
            });

            _logger.LogInformation($"Finished Getting product for Id:{id}");

            return await Task.FromResult<List<Level>>(lstLevel);
        }

        public async Task<List<Facility>> GetFacilityAsync(string id)
        {
            var lstFacility = new List<Facility>();
            lstFacility.Add(new Facility
            {
                Id = 101,
               FacilityName = "EWR"
            });
            lstFacility.Add(new Facility
            {
                Id = 102,
                FacilityName = "JFK"
            });

            _logger.LogInformation($"Finished Getting Facility for Id:{id}");

            return await Task.FromResult<List<Facility>>(lstFacility);
        }
    }
}