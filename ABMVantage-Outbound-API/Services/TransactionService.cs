using ABMVantage.Data.Models;
using ABMVantage_Outbound_API.Configuration;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Functions;
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



        private DashboardFunctionSettings _settings;
        private readonly ILogger<TransactionService> _logger;
        private readonly IDataAccessSqlService _dataAccessSqlService;
        private readonly IConfiguration _configuration;
        //private IDataAccessSqlService _dataAccessService;
        public TransactionService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService, DashboardFunctionSettings settings)
        {
            _logger = loggerFactory.CreateLogger<TransactionService>();

            _dataAccessSqlService = dataAccessSqlService;

            _settings = settings;

            _logger.LogInformation($"Constructing {nameof(TransactionService)}");
        }




        public async Task<int> GetDailyTransactiontCountAsync(DateTime? tranactionDate, string? facilityId, string? levelId, string? parkingProductId)
        {
           /* if (filterParameters == null || filterParameters.FromDate < _settings.MinimumValidCalculationDate || filterParameters.ToDate < _settings.MinimumValidCalculationDate)
            {
                _logger.LogError($"{nameof(DashboardFunctionDailyAverageOccupancy)} Query string  parametr customerId is EMPTY OR not supplied!");
                throw new ArgumentNullException("parkingProductId");
            }*/

            var result = await _dataAccessSqlService.GetDailyTransactionCountAsync(tranactionDate, facilityId, levelId, parkingProductId);

            return result;
        }

        public async Task<decimal> GetDailyTotalRevenueAsync(FilterParam filterParameters)
        {
            if (filterParameters == null || filterParameters.FromDate < _settings.MinimumValidCalculationDate || filterParameters.ToDate < _settings.MinimumValidCalculationDate)
            {
                _logger.LogError($"{nameof(GetDailyTotalRevenueAsync)} Parameter Object or to or from date missing!");
                throw new ArgumentException();
            }
            var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
            var result = await _dataAccessSqlService.GetDailyTotalRevenueAsync(queryParameters);

            return result;
        }

        public async Task<DashboardDailyAverageOccupancy> GetDailyAverageOccupancy(FilterParam filterParameters)
        {
            var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
            var result = await _dataAccessSqlService.GetDailyAverageOccupancy(queryParameters);

            return result;
        }

        public async Task<DashboardMonthlyTransactionCount> GetMonthlyTransactionCountAsync(FilterParam filterParameters)
        {
            _logger.LogInformation($"Getting Dashboard Monthly Transaction Count {nameof(GetMonthlyTransactionCountAsync)}");
            if (filterParameters == null || filterParameters.FromDate < _settings.MinimumValidCalculationDate || filterParameters.ToDate < _settings.MinimumValidCalculationDate)
            {
                throw new ArgumentException($"Missing or Invalid parameters.");
            }

            var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
            var monthlyTransactionCounts = await _dataAccessSqlService.GetMonthlyTransactionCountsAsync(queryParameters);
            var results = from TransactionsByMonthAndProduct cnt in monthlyTransactionCounts
                          group cnt by new { cnt.Year, cnt.Month } into monthlyGroup
                          select new TransactionCountForMonth
                          {
                              Month = monthlyGroup.Key.Year.ToString() + monthlyGroup.Key.Month.ToString(),
                              Data = monthlyGroup.Select(x => new TransactionsForProduct { NoOfTransactions = x.TransactionCount, Product = x.ParkingProduct })
                          };
            var result = new DashboardMonthlyTransactionCount { MonthlyTransactions = results };
            return result;



        }

        public async Task<DashboardMonthlyRevenueAndBudget> GetMonthlyRevenueAndBudget(FilterParam filterParameters)
        {
            _logger.LogInformation($"Getting Dashboard Monthly Revenue and Budgeted Revenue {nameof(GetMonthlyRevenueAndBudget)}");
            if (filterParameters == null || filterParameters.FromDate < _settings.MinimumValidCalculationDate || filterParameters.ToDate < _settings.MinimumValidCalculationDate)
            {
                throw new ArgumentException($"Missing or Invalid parameters.");
            }
            var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
            var queryResults = await _dataAccessSqlService.GetMonthlyRevenueAndBudget(queryParameters);
            var results = from RevenueAndBudgetForMonth rnb in queryResults
                          select new RevenueAndBudget
                          {
                              Month = rnb.Year.ToString() + rnb.Month.ToString().PadLeft(2, '0'),
                              Revenue = rnb.Revenue,
                              BudgetedRevenue = rnb.BudgetedRevenue
                          };

            return new DashboardMonthlyRevenueAndBudget { MonthlyRevenueAndBudget = results };
        }
    }

}