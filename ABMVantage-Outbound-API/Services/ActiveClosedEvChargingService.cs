namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Models;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Active and Closed EV charging sessions service
    /// </summary>
    public class ActiveClosedEvChargingService : IActiveClosedEvChargingService
    {
        private readonly ILogger<ActiveClosedEvChargingService> _logger;
        private readonly IDataAccessService _dataAccessService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveClosedEvChargingService"/> class.
        /// </summary>
        /// <param name="loggerFactory">logging</param>
        /// <param name="dataAccessService">data access</param>
        public ActiveClosedEvChargingService(ILoggerFactory loggerFactory, IDataAccessService dataAccessService)
        {
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<ActiveClosedEvChargingService>();
            _dataAccessService = dataAccessService;
            
            _logger.LogInformation($"Constructing {nameof(ActiveClosedEvChargingService)}");
        }

        /// <summary>
        /// Get all the charging sessions and apply biz rules
        /// </summary>
        /// <returns>ActiveClosedEvChargingSession</returns>
        public async Task<ActiveClosedEvChargingSession> GetChargingSessionsAsync()
        {
            _logger.LogInformation($"Getting session data for {nameof(EvActiveSessions)} and {nameof(EvClosedSessions)}");

            var closed = await _dataAccessService.GetClosedChargingSessionsAsync().ConfigureAwait(false);
            var active = await _dataAccessService.GetActiveChargingSessionsAsync().ConfigureAwait(false);

            return new ActiveClosedEvChargingSession { ClosedSessions = closed, ActiveSessions = active };
        }
    }
}