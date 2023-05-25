using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Interfaces
{
    public interface IOccupancyService
    {
        Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(FilterParam inputFilter);
        Task<IEnumerable<OccWeeklyOccByDuration>> GetWeeklyOccByDuration(FilterParam inputFilter);
        Task<IEnumerable<OccCurrent>> GetOccCurrent(FilterParam inputFilter);
        Task<IEnumerable<AvgMonthlyOccVsDuration>> GetAvgMonthlyOccVsDuration(FilterParam inputFilter);
        Task<IEnumerable<YearlyOccupancy>> GetYearlyOccupancy(FilterParam inputFilter);
        Task <DailyAverageOccupancy> GetDailyAverageOccupancy(FilterParam? filterParameters);
    }
}
