using ABMVantage.Data.Models;

namespace ABMVantage.Data.Interfaces
{
    public interface IRevenueAndTransactionService
    {
        Task<RevenueByProductList> GetRevenueByProductByDays(FilterParam inputFilter);

        Task<RevenueByDayList> GetRevenueByDays(FilterParam inputFilter);

        Task<RevenueBudgetList> GetRevenueVsBudget(FilterParam inputFilter);

        Task<BudgetVarianceList> GetBudgetVsActualVariance(FilterParam inputFilter);

        Task<MonthlyRevenueList> GetRevenueByMonths(FilterParam inputFilter);

        Task<CurrentTransactionList> GetTransacionByHours(FilterParam inputFilter);

        Task<DailyTransactionList> GetTransactonByDays(FilterParam inputFilter);

        Task<CurrentAndPreviousYearMonthlyTransactionList> GetTransactonMonths(FilterParam inputFilter);
    }
}