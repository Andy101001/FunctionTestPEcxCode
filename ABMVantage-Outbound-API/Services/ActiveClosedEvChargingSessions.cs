namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.Models;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class ActiveClosedEvChargingSessions : IActiveClosedEvChargingSessions
    {
        private readonly ILogger<ObsReservationService> _logger;
        private readonly IDataAccessService _dataAccessService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="loggerFactory">logging</param>
        /// <param name="dataAccessService">data access</param>
        public ActiveClosedEvChargingSessions(ILoggerFactory loggerFactory, IDataAccessService dataAccessService)
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
        public async Task<ActiveClosedEvChargingSession> GetChargingSessionsAsync()
        {
            var closed = await _dataAccessService.GetClosedChargingSessionsAsync().ConfigureAwait(false);
            var active = await _dataAccessService.GetActiveChargingSessionsAsync().ConfigureAwait(false);

            return new ActiveClosedEvChargingSession { ClosedSessions = closed, ActiveSessions = active };
        }
    }
}