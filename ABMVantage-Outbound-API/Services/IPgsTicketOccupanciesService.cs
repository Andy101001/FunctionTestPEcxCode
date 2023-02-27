namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.EntityModels;

    /// <summary>
    /// Inteface for the parcs ticket occupancies
    /// </summary>
    public interface IPgsTicketOccupanciesService
    {
        /// <summary>
        /// Get all the parcs ticket occupancies and apply biz rules
        /// </summary>
        /// <returns>List<PgsOccupancy></returns>
        Task<List<PgsOccupancy>> GetOccupanciesAsync();
    }
}