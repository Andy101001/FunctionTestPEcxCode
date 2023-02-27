namespace ABMVantage_Outbound_API.Services
{    
    using ABMVantage_Outbound_API.EntityModels;
    
    /// <summary>
    /// Dataaccess readonly interface
    /// </summary>
    public interface IDataAccessService
    {
        /// <summary>
        /// Get Reservations
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Reservation</returns>
        Task<Reservation>? GetReservationsAsync(string id);

        /// <summary>
        /// Active Charging Sessions
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<EvActiveSessions></returns>
        Task<List<EvActiveSessions>>? GetActiveChargingSessionsAsync(string? id = null);

        /// <summary>
        /// Closed Charging Sessions
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<EvClosedSessions></returns>
        Task<List<EvClosedSessions>>? GetClosedChargingSessionsAsync(string? id = null);

        /// <summary>
        /// Reservation Transactions
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<ObsReservationTransactions></returns>
        Task<List<ObsReservationTransactions>>? GetReservationsTransactionsAsync(string? id = null);

        /// <summary>
        /// Parcs Tickets Occupancies
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<Occupancy></returns>
        Task<List<Occupancy>>? GetParcsTicketOccupanciesAsync(string? id = null);

        /// <summary>
        /// PGS Ticket Occupancies
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<PgsOccupancy></returns>
        Task<List<PgsOccupancy>>? GetPgsTicketOccupanciesAsync(string? id = null);

        /// <summary>
        /// Parcs Ticket Transactions
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<ParcsTicketsTransactions></returns>
        Task<List<ParcsTicketsTransactions>>? GetParcsTicketTransactionsAsync(string? id = null);
    }
}