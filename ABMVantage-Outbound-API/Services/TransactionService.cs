using ABMVantage_Outbound_API.Functions;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABMVantage_Outbound_API.Configuration;
using Microsoft.Extensions.Logging;
using ABMVantage_Outbound_API.EntityModels;

namespace ABMVantage_Outbound_API.Services
{
    public class TransactionService : ITransactionService
    {
        private DashboardFunctionSettings _settings;
        private ILogger<TransactionService> _logger;
        private IDataAccessSqlService _dataAccessService;

        public TransactionService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAcessService, DashboardFunctionSettings settings)
        {
            _logger = loggerFactory.CreateLogger<TransactionService>();
            _dataAccessService = dataAcessService;
            _settings = settings;
        }

        public async Task<DashboardMonthlyTransactionCount> GetMonthlyTransactionCount(DateTime calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            _logger.LogInformation($"Getting Dashboard Monthly Transaction Count {nameof(GetMonthlyTransactionCount)}");
            if (calculationDate < _settings.MinimumValidCalculationDate)
            {
                throw new ArgumentException($"Calculation date must be greater than {_settings.MinimumValidCalculationDate}");
            }
            var startDate = new DateTime(calculationDate.Year, calculationDate.Month, 1);
            var endDate = startDate.AddMonths(_settings.MonthlyTransactionCountInterval).AddDays(-1);
            var monthlyTransactionCounts = await _dataAccessService.GetMonthlyTransactionCountsAsync(startDate, endDate, facilityId, levelId, parkingProductId);
            var results = from TransactionsByMonthAndProduct cnt in monthlyTransactionCounts
                               group cnt by new {cnt.Year, cnt.Month} into monthlyGroup
                          select new TransactionCountForMonth
                          {
                              Month = monthlyGroup.Key.Year.ToString() +  monthlyGroup.Key.Month.ToString(),
                              Data = monthlyGroup.Select(x => new TransactionsForProduct { NoOfTransactions = x.TransactionCount, Product = x.ParkingProduct })
                          };
            var result = new DashboardMonthlyTransactionCount { MonthlyTransactions = results };
            return result;

            

        }
    }
}
