namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using ABMVantage_Outbound_API.Models;
    /// <summary>
    /// Service for all things to do with reservations
    /// </summary>
    public interface IReservationService
    {
        /// <summary>
        /// Get reservation data
        /// </summary>
        /// <param name="hourlyReservationParameters">params</param>
        /// <returns>ReservationDetails</returns>
        Task<List<ReservationByHour>> ReservationPerHour(HourlyReservationParameters hourlyReservationParameters);
    }
}