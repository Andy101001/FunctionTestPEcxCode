namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Models;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    /// <summary>
    /// Service to get reservation transaction for online booking
    /// </summary>
    public class OBSReservationTransactionsService : IOBSReservationTransactionsService
    {
        private readonly ILogger<OBSReservationTransactionsService> _logger;
        private readonly IDataAccessService _dataAccessService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OBSReservationTransactionsService"/> class.
        /// </summary>
        /// <param name="loggerFactory">logging</param>
        /// <param name="dataAccessService">data access</param>
        public OBSReservationTransactionsService(ILoggerFactory loggerFactory, IDataAccessService dataAccessService)
        {
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<OBSReservationTransactionsService>();
            _dataAccessService = dataAccessService;

            _logger.LogInformation($"Constructing {nameof(OBSReservationTransactionsService)}");
        }

        /// <summary>
        /// Get all the charging sessions and apply biz rules
        /// </summary>
        /// <returns>ActiveClosedEvChargingSession</returns>
        public async Task<ReservationTransactions> GetObsReservationTransactionsAsync()
        {
            _logger.LogInformation($"Getting OBS Reservation Transactions {nameof(GetObsReservationTransactionsAsync)}");

            List<ObsReservationTransactions> obsReservationTransactions = await _dataAccessService.GetReservationsTransactionsAsync().ConfigureAwait(false);

            return new ReservationTransactions { ObsReservationTransactions = obsReservationTransactions.SingleOrDefault() };
        }
    }
}