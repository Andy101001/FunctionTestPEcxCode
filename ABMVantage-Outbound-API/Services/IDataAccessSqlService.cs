using ABMVantage_Outbound_API.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public interface IDataAccessSqlService
    {
        /// <summary>
        /// Get Products
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<DimProduct>> GetProductAsync(string id);

        /// <summary>
        /// Ge tLevels
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<DimLevel>> GetLevelAsync(string id);

        /// <summary>
        /// Get Facilites
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IList<DimFacility>> GetFacilityAsync(string id);

        Task<decimal> GetDailyTotalRevenueAsync(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId);

        Task<int> GetDailyTransactionCountAsync(DateTime? transactionDate, string? facilityId, string? levelId, string? parkingProductId);

        Task<int> GetDailyAverageOccupancy(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId);
    }
}
