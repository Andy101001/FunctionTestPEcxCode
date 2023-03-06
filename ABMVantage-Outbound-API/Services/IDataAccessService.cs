namespace ABMVantage_Outbound_API.Services
{    
    using ABMVantage_Outbound_API.EntityModels;
    
    /// <summary>
    /// Data access readonly interface
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
        Task<List<ObsReservationTransactions>>? GetReservationsTransactionsAsync();

        /// <summary>
        /// Parcs Tickets Occupancies
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<Occupancy></returns>
        Task<List<Occupancy>>? GetParcsTicketOccupanciesAsync();

        /// <summary>
        /// PGS Ticket Occupancies
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<PgsOccupancy></returns>
        Task<List<PgsOccupancy>>? GetPgsTicketOccupanciesAsync();

        /// <summary>
        /// Parcs Ticket Transactions
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>List<ParcsTicketsTransactions></returns>
        Task<List<ParcsTicketsTransactions>>? GetParcsTicketTransactionsAsync();

        /// <summary>
        /// All OBS Reservations
        /// </summary>        
        /// <returns>List<List<Booking>></returns>
        Task<List<Booking>>? GetAllObsReservationsAsync();

        /// <summary>
        /// Get OBS Reservation
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Booking</returns>
        Task<Booking> GetReservationAsync(string id);
    }
}