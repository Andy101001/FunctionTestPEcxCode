namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.DashboardFunctionModels;

    /// <summary>
    /// Service for all things to do with tickets
    /// </summary>
    public interface ITicketService
    {
        /// <summary>
        /// Get reservation data
        /// </summary>
        /// <param name="hourlyReservationParameters">params</param>
        /// <returns>ReservationDetails</returns>
        Task<List<DashboardMonthlyAverageTicketValue>> AverageTicketValuePerYear(TicketPerYearParameters ticketPerYearParameters);
    }
}