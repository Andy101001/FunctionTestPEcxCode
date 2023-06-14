using ABMVantage.Data.Models;

namespace ABMVantage.Data.Interfaces
{
    public interface ISingleTicketEVChargesService
    {
        /// <summary>
        /// Get EVCharges 
        /// </summary>
        /// <param name="evRequest"></param>
        /// <returns></returns>
        Task<SingleTicketEVChargesResponse> GetEVCharges(SingleTicketEVChargesRequest evRequest);
    }
}
