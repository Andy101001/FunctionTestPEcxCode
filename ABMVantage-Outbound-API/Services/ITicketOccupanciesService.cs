namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.EntityModels;

    /// <summary>
    /// Inteface for the parcs ticket occupancies
    /// </summary>
    public interface ITicketOccupanciesService
    {
        /// <summary>
        /// Get all the parcs ticket occupancies and apply biz rules
        /// </summary>
        /// <returns>Occupancy</returns>
        Task<List<Occupancy>> GetOccupanciesAsync();
    }
}