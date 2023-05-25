namespace ABMVantage.Data.Interfaces
{
    using ABMVantage.Data.Models;
    public interface ITransactionRepository
    {
        Task<IEnumerable<BudgetVariance>> GetBudgetVsActualVariance(FilterParam inputFilter);
        Task<IEnumerable<RevenueByDay>> GetRevenueByDays(FilterParam inputFilter);
        Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam inputFilter);
        Task<IEnumerable<RevenueByProduct>> GetRevenueByProductByDays(FilterParam inputFilter);
        Task<IEnumerable<RevenueBudget>> GetRevenueVsBudget(FilterParam inputFilter);
        Task<IEnumerable<CurrentTransaction>> GetTranactionByHours(FilterParam inputFilter);
        Task<IEnumerable<DailyTransaction>> GetTransactionByDays(FilterParam inputFilter);
        Task<IEnumerable<MonthlyTransaction>> GetTransactionMonths(FilterParam inputFilter);
    }
}