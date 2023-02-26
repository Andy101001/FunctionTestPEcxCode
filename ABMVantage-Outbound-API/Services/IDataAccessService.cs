namespace ABMVantage_Outbound_API.Services
{    
    using ABMVantage_Outbound_API.EntityModels;

    public interface IDataAccessService
    {
        Task<Reservation>? GetReservationsAsync(string id);
        Task<List<EvActiveSessions>>? GetActiveChargingSessionsAsync(string? id = null);
        Task<List<EvClosedSessions>>? GetClosedChargingSessionsAsync(string? id = null);        
        Task<List<ObsReservationTransactions>>? GetReservationsTransactionsAsync(string? id = null);
        Task<List<Occupancy>>? GetParcsTicketOccupanciesAsync(string? id = null);
        Task<List<PgsOccupancy>>? GetPgsTicketOccupanciesAsync(string? id = null);
    }
}