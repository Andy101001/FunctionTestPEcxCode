using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.EntityModels;

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

        /// <summary>
        /// Gets the total number of transactions for each month, potentially filtered by facility, level and product.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="facilityId"></param>
        /// <param name="levelId"></param>
        /// <param name="parkingProductId"></param>
        /// <returns></returns>
        Task<IEnumerable<TransactionsByMonthAndProduct>> GetMonthlyTransactionCountsAsync(DateTime startDate, DateTime endDate, string? facilityId, string? levelId, string? parkingProductId);


        Task<decimal> GetDailyTotalRevenueAsync(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId);

        Task<int> GetDailyTransactionCountAsync(DateTime? transactionDate, string? facilityId, string? levelId, string? parkingProductId);

        Task<DashboardDailyAverageOccupancy> GetDailyAverageOccupancy(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId);
        Task<IList<DashboardFuctionDayReservation>> GetDaysReservations(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId);
        Task<IList<DashboardFuctionDayRevenue>> GetRevnueByDay(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId);
        Task<IList<DashboardFuctionMonthRevenue>> GetRevnueByMonth(DateTime? startDate, DateTime? endDate, string? facilityId, string? levelId, string? parkingProductId);


        Task<IEnumerable<OccupancyByMonth>> GetMonthlyParkingOccupanciesAsync(DateTime startDate,DateTime endDate,string? facilityId,string? levelId,string? parkingProductId);
        Task<IList<RevenueAndBudget>> GetMonthlyRevenueAndBudget(DateTime? startDate, DateTime? endDate, string? facilityId, string? levelId, string? parkingProductId);



    }
}
