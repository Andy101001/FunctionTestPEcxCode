namespace ABMVantage_Outbound_API.Models
{
    using ABMVantage_Outbound_API.EntityModels;
    /// <summary>
    /// Ev sessions model
    /// </summary>
    public class ActiveClosedEvChargingSession
    {
        /// <summary>
        /// Active Sessions
        /// </summary>
        public List<EvActiveSessions>? ActiveSessions { get; set; }

        /// <summary>
        /// Closed Sessions
        /// </summary>
        public List<EvClosedSessions>? ClosedSessions { get; set; }
    }
}