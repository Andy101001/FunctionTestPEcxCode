namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.Models;

    /// <summary>
    /// Inteface for the ActiveClosed EV sessions
    /// </summary>
    public interface IActiveClosedEvChargingService
    {
        /// <summary>
        /// Get all the charging sessions and apply biz rules
        /// </summary>
        /// <returns>ActiveClosedEvChargingSession</returns>
        Task<ActiveClosedEvChargingSession> GetChargingSessionsAsync();
    }
}