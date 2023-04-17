using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Interfaces
{
    public interface ITransaction_NewService
    {
        Task<IEnumerable<RevenueByProduct>> GetRevenueByProductByDays(FilterParam inputFilter);
        Task<IEnumerable<RevenueByDay>> GetRevenueByDays(FilterParam inputFilter);
        Task<IEnumerable<RevenueBudget>> GetRevenueVsBduget(FilterParam inputFilter);
        Task<IEnumerable<BudgetVariance>> GetBudgetVsActualVriance(FilterParam inputFilter);
        Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam inputFilter);
        Task<IEnumerable<CurrentTransaction>> GetTranacionByHours(FilterParam inputFilter);
        Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter);
        Task<IEnumerable<MonthlyTransaction>> GetTransactonMonths(FilterParam inputFilter);







    }
}
