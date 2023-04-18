using ABMVantage.Data.Models;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Models;
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

        /// <summary>
        /// Gets the total number of transactions for each month, potentially filtered by facility, level and product.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="facilityId"></param>
        /// <param name="levelId"></param>
        /// <param name="parkingProductId"></param>
        /// <returns></returns>
        Task<IEnumerable<TransactionsByMonthAndProduct>> GetMonthlyTransactionCountsAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters);

        /// <summary>
        /// Gets the reservation count per hour, potentially filtered by facility, level and product.
        /// </summary>
        /// <param name="hourlyReservationParameters">parameters</param>
        /// <returns>IEnumerable<ReservationByHourData></returns>
        Task<IEnumerable<ReservationsForProductAndHour>> GetReservationByHourCountsAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters);


        Task<IEnumerable<MonthlyAverageTicketValue>> GetAverageTicketValuePerYearAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters);


        Task<decimal> GetDailyTotalRevenueAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters);

        Task<int> GetDailyTransactionCountAsync(DateTime? transactionDate, string? facilityId, string? levelId, string? parkingProductId);

        Task<DashboardDailyAverageOccupancy> GetDailyAverageOccupancy(DashboardFunctionDefaultDataAccessQueryParameters queryParameters);
        Task<IList<DashboardFunctionDayReservation>> GetDaysReservations(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId);
        Task<IList<DashboardFunctionDayRevenue>> GetRevnueByDay(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId);
        Task<IList<DashboardFunctionMonthRevenue>> GetRevnueByMonth(DateTime? startDate, DateTime? endDate, string? facilityId, string? levelId, string? parkingProductId);


        Task<IEnumerable<OccupancyByMonth>> GetMonthlyParkingOccupanciesAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters);
        Task<IEnumerable<RevenueAndBudgetForMonth>> GetMonthlyRevenueAndBudget(DashboardFunctionDefaultDataAccessQueryParameters queryParameters);



    }
}
