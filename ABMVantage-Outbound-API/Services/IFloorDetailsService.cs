using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public interface IFloorDetailsService
    {
        /// <summary>
        /// Get floor data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FloorDetails> GetFloorDetails(string id);

        //Task<decimal> GetDailyTotalRevenueAsync(DateTime calculationDate, string? facilityId, string? levelId, string? parkingProductId);
    }
}
