using ABMVantage.Data.Models;

namespace ABMVantage.Data.Interfaces
{
    public interface IRevenueAndTransactionService
    {
        Task<IEnumerable<RevenueByProduct>> GetRevenueByProductByDays(FilterParam inputFilter);

        Task<IEnumerable<RevenueByDay>> GetRevenueByDays(FilterParam inputFilter);

        Task<IEnumerable<RevenueBudget>> GetRevenueVsBudget(FilterParam inputFilter);

        Task<IEnumerable<BudgetVariance>> GetBudgetVsActualVariance(FilterParam inputFilter);

        Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam inputFilter);

        Task<IEnumerable<CurrentTransaction>> GetTransacionByHours(FilterParam inputFilter);

        Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter);

        Task<IEnumerable<CurrentAndPreviousYearMonthlyTransaction>> GetTransactonMonths(FilterParam inputFilter);
    }
}