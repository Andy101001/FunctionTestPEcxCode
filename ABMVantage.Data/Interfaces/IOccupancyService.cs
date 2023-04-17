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
        Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(string userId, int CustomerId);
        Task<IEnumerable<OccWeeklyOccByDuration>> GetWeeklyOccByDuration(string userId, int CustomerId);
        Task<IEnumerable<OccCurrent>> GetOccCurrent(string userId, int CustomerId);
        Task<IEnumerable<AvgMonthlyOccVsDuration>> GetAvgMonthlyOccVsDuration(string userId, int CustomerId);
        Task<IEnumerable<YearlyOccupancy>> GetYearlyOccupancy(string userId, int CustomerId);
    }
}
