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
        Task<IEnumerable<TransactionsByMonthAndProduct>> GetMonthlyTransactionCountsAsync(DateTime startDate, DateTime endDate, string? facilityId, string? levelId, string? parkingProductId);

        /// <summary>
        /// Gets the reservation count per hour, potentially filtered by facility, level and product.
        /// </summary>
        /// <param name="hourlyReservationParameters">parameters</param>
        /// <returns>IEnumerable<ReservationByHourData></returns>
        Task<IEnumerable<ReservationByHour>> GetReservationByHourCountsAsync(HourlyReservationParameters hourlyReservationParameters);

        /// <summary>
        /// Gets the average ticket value per year, potentially filtered by facility, level and product.
        /// </summary>
        /// <param name="hourlyReservationParameters">ticketValuesPerYear</param>
        /// <returns>IEnumerable<ReservationByHourData></returns>
        Task<IEnumerable<DashboardMonthlyAverageTicketValue>> GetAverageTicketValuePerYearAsync(TicketPerYearParameters ticketValuesPerYear);


        Task<decimal> GetDailyTotalRevenueAsync(DateTime calculationDate, string? facilityId, string? levelId, string? parkingProductId);
    }
}
