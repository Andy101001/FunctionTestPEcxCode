namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.EntityModels;

    /// <summary>
    /// Inteface for the parcs ticket transactions
    /// </summary>
    public interface IParcsTicketTransactionsService
    {
        /// <summary>
        /// Get all the parcs ticket transactions and apply biz rules
        /// </summary>
        /// <returns>Occupancy</returns>
        Task<List<ParcsTicketsTransactions>> GetParcsTicketTransactionsAsync();
    }
}