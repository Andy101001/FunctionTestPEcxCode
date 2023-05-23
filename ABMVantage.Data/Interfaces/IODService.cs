using ABMVantage.Data.Models;
using ABMVantage.Data.Models.DashboardModels;

namespace ABMVantage.Data.Interfaces
{
    public interface IODService
    {
        Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(FilterParam inputFilter);
        Task<IEnumerable<OccWeeklyOccByDuration>> GetWeeklyOccByDuration(FilterParam inputFilter);
        Task<IEnumerable<OccCurrent>> GetOccCurrent(FilterParam inputFilter);
        Task<IEnumerable<AvgMonthlyOccVsDuration>> GetAvgMonthlyOccVsDuration(FilterParam inputFilter);
        Task<IEnumerable<YearlyOccupancy>> GetYearlyOccupancy(FilterParam inputFilter);
    }
}