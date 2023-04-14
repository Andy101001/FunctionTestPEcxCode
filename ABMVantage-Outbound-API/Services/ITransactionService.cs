using ABMVantage_Outbound_API.DashboardFunctionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public interface ITransactionService
    {
        Task<DashboardMonthlyTransactionCount> GetMonthlyTransactionCount(DateTime calculationDate, string? facilityId, string? levelId, string? parkingProductId);
    }
}
