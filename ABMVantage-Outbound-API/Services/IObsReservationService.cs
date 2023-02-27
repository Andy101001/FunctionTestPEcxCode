namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.EntityModels;
    /// <summary>
    /// Readonly Interface for OBS Reservations
    /// </summary>
    public interface IObsReservationService
    {
        /// <summary>
        /// Get all Reservations
        /// </summary>
        /// <returns>List<Booking></returns>
        Task<List<Booking>> GetAllReservationsAsync();
        
        /// <summary>
        /// Get a reservation
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Booking</returns>
        Task<Booking> GetReservationAsync(string id);
    }
}