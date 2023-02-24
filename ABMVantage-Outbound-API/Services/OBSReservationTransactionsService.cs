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
        private readonly ILogger<ObsReservationService> _logger;
        private readonly IDataAccessService _dataAccessService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="loggerFactory">logging</param>
        /// <param name="dataAccessService">data access</param>
        public OBSReservationTransactionsService(ILoggerFactory loggerFactory, IDataAccessService dataAccessService)
        {
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<ObsReservationService>();
            _dataAccessService = dataAccessService;
        }

        /// <summary>
        /// Get all the charging sessions and apply biz rules
        /// </summary>
        /// <returns>ActiveClosedEvChargingSession</returns>
        public async Task<ReservationTransactions> GetObsReservationTransactions()
        {
            List<ObsReservationTransactions> obsReservationTransactions = await _dataAccessService.GetReservationsTransactionsAsync().ConfigureAwait(false);    

            return new ReservationTransactions {ObsReservationTransactions = obsReservationTransactions.SingleOrDefault() };
        }
    }
}