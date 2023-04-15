using ABMVantage_Outbound_API.DashboardFunctionModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{

 
  
    public class TransactionService : ITransactionService
    {

        private readonly ILogger<TransactionService> _logger;
        private readonly IDataAccessSqlService _dataAccessSqlService;
        private readonly IConfiguration _configuration;
        public TransactionService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService)
        {
            _logger = loggerFactory.CreateLogger<TransactionService>();

            _dataAccessSqlService = dataAccessSqlService;

            _logger.LogInformation($"Constructing {nameof(TransactionService)}");
        }

       

       
        public async Task<int> GetDailyTransactiontCountAsync(DateTime? tranactionDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            var result = await _dataAccessSqlService.GetDailyTransactionCountAsync(tranactionDate, facilityId, levelId, parkingProductId);

            return result;
        }

        public async Task<decimal> GetDailyTotalRevenueAsync(DateTime? tranactionDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            var result = await _dataAccessSqlService.GetDailyTotalRevenueAsync(tranactionDate, facilityId, levelId, parkingProductId);

            return result;
        }

        public async Task<DashboardDailyAverageOccupancy> GetDailyAverageOccupancy(DateTime? tranactionDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            var result = await _dataAccessSqlService.GetDailyAverageOccupancy(tranactionDate, facilityId, levelId, parkingProductId);

            return result;
        }
    }
}
