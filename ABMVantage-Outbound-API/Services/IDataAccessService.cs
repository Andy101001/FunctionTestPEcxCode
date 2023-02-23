namespace ABMVantage_Outbound_API.Services
{    
    using ABMVantage_Outbound_API.EntityModels;

    public interface IDataAccessService
    {
        Task<Reservation>? GetReservationsAsync(string id);
        Task<List<EvActiveSessions>>? GetActiveChargingSessionsAsync(string? id = null);
        Task<List<EvClosedSessions>>? GetClosedChargingSessionsAsync(string? id = null);
    }
}