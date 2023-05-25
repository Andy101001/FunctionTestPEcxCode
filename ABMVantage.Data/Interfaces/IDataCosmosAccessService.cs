using ABMVantage.Data.EntityModels;
using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Interfaces
{
    public interface  IDataCosmosAccessService
    {
        // Revenue and Trasaction       
        Task<IList<DailyTransaction>> GetTransactonByDays(FilterParam parameters);
        Task<IList<CurrentTransaction>> GetTransactonByHours(FilterParam parameters);
        Task<IList<MonthlyTransaction>> GetTransactonByMonth(FilterParam parameters);
        Task<IList<RevenueBudget>> GetRevenueVsBudget(FilterParam parameters);
        Task<IList<RevenueByProduct>> GetRevenueByProductByDays(FilterParam parameters);
        Task<IList<BudgetVariance>> GetBudgetVsActualVariance(FilterParam parameters);
        Task<IList<RevenueByDay>> GetRevenueByDays(FilterParam parameters);
        Task<IList<MonthlyRevenue>> GetRevenueByMonths(FilterParam parameters);

        //Revenue
        Task<IList<ReservationsByHour>> GetHourlyReservations(FilterParam parameters);
        Task<IList<ReservationsByDay>> GetDailyReservations(FilterParam parameters);
        Task<IList<ReservationsByMonth>> GetMonthlyReservations(FilterParam parameters);
        Task<IList<ResAvgTicketValue>> GetReservationsAvgTkt(FilterParam parameters);
    }
}
