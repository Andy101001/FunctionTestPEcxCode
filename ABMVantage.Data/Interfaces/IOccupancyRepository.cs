using ABMVantage.Data.Models;

namespace ABMVantage.Data.Interfaces
{
    public interface IOccupancyRepository
    {
        Task<IEnumerable<AvgMonthlyOccVsDuration>> GetAvgMonthlyOccVsDuration(FilterParam inputFilter);
        Task<IEnumerable<OccCurrent>> GetOccCurrent(FilterParam inputFilter);
        Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(FilterParam inputFilter);
        Task<IEnumerable<OccWeeklyOccByDuration>> GetWeeklyOccByDuration(FilterParam inputFilter);
        Task<IEnumerable<YearlyOccupancy>> GetYearlyOccupancy(FilterParam inputFilter);
    }
}