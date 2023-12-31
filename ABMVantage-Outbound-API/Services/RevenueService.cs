﻿namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using Microsoft.Extensions.Logging;

    public class RevenueService : IRevenueService
    {
        private readonly ILogger<RevenueService> _logger;
        private readonly IDataAccessSqlService _dataAccessSqlService;

        public RevenueService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService)
        {
            _logger = loggerFactory.CreateLogger<RevenueService>();

            _dataAccessSqlService = dataAccessSqlService;

            _logger.LogInformation($"Constructing {nameof(RevenueService)}");
        }

        public async Task<IList<DashboardFunctionDayRevenue>> GetRevenueByDay(DateTime? tranactionDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            IList<DashboardFunctionDayRevenue>? revenues = null;
            try
            {
                revenues = await _dataAccessSqlService.GetRevnueByDay(tranactionDate, facilityId, levelId, parkingProductId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DashboardFunctionDayRevenue)} has an error! : {ex.Message}");
            }

            return revenues;
        }

        public async Task<IList<DashboardFunctionMonthRevenue>> GetRevnueByMonth(DateTime? startDate, DateTime? endDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            IList<DashboardFunctionMonthRevenue>? revenues = null;
            try
            {
                revenues = await _dataAccessSqlService.GetRevnueByMonth(startDate, endDate, facilityId, levelId, parkingProductId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DashboardFunctionMonthRevenue)} has an error! : {ex.Message}");
            }

            return revenues;
        }
    }
}