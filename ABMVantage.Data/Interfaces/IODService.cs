using ABMVantage.Data.Models;
using ABMVantage.Data.Models.DashboardModels;

namespace ABMVantage.Data.Interfaces
{
    public interface IODService
    {
        Task<OccRevenueByProductList> GetTotalOccRevenue(FilterParam inputFilter);
        Task<OccWeeklyOccByDurationList> GetWeeklyOccByDuration(FilterParam inputFilter);
        Task<OccCurrentList> GetOccCurrent(FilterParam inputFilter);
        Task<AvgMonthlyOccVsDurationList> GetAvgMonthlyOccVsDuration(FilterParam inputFilter);
        Task<YearlyOccupancyList> GetYearlyOccupancy(FilterParam inputFilter);
    }
}