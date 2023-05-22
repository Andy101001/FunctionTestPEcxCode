using ABMVantage.Data.Models;
using static ABMVantage.Data.Models.DashboardModels;

namespace ABMVantage.Data.Interfaces
{
    public interface IDashboardService
    {
        Task<DailyAverageOccupancy> GetDailyAverageOccupancy(FilterParam? filterParameters);
        Task<decimal> GetDailyTotalRevenueAsync(FilterParam filterParameters);
        Task<int> GetDailyTransactiontCountAsync(FilterParam filterParameters);
        Task<DashboardDailyReservationCountByHour> GetHourlyReservationsByProduct(FilterParam filterParameters);
        Task<DashboardMonthlyRevenueAndBudget> GetMonthlyRevenueAndBudget(FilterParam filterParameters);
        Task<DashboardMonthlyParkingOccupancy> GetMonthlyParkingOccupancyAsync(FilterParam filterParameters);
        Task<DashboardMonthlyTransactionCount> GetMonthlyTransactionCountAsync(FilterParam filterParameters);
        Task<DashboardMonthlyAverageTicketValue> AverageTicketValuePerYear(FilterParam filterParameter);
    }
}