using ABMVantage_Outbound_API.DashboardFunctionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public interface IRevenueService
    {

        Task<IList<DashboardFunctionDayRevenue>> GetRevenueByDay(DateTime? tranactionDate, string? facilityId, string? levelId, string? parkingProductId);

        Task<IList<DashboardFunctionMonthRevenue>> GetRevnueByMonth(DateTime? startDate, DateTime? endDate, string? facilityId, string? levelId, string? parkingProductId);
    }
}
