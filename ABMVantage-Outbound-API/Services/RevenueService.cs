using ABMVantage_Outbound_API.DashboardFunctionModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public class RevenueService : IRevenueService
    {
        private readonly ILogger<RevenueService> _logger;
        private readonly IDataAccessSqlService _dataAccessSqlService;
        private readonly IConfiguration _configuration;

        public RevenueService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService)
        {
            _logger = loggerFactory.CreateLogger<RevenueService>();

            _dataAccessSqlService = dataAccessSqlService;

            _logger.LogInformation($"Constructing {nameof(RevenueService)}");
        }
        public async Task<IList<DashboardFuctionDayRevenue>> GetRevenueByDay(DateTime? tranactionDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            var revenues = await _dataAccessSqlService.GetRevnueByDay(tranactionDate, facilityId, levelId, parkingProductId);

            return revenues;
        }

        public async Task<IList<DashboardFuctionMonthRevenue>> GetRevnueByMonth(DateTime? startDate, DateTime? endDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            var revenues = await _dataAccessSqlService.GetRevnueByMonth(startDate, endDate, facilityId, levelId, parkingProductId);

            return revenues;
        }
    }
}
