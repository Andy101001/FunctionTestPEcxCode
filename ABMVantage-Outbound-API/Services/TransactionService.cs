namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage.Data.Models;
    using ABMVantage_Outbound_API.Configuration;
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using ABMVantage_Outbound_API.EntityModels;
    using Microsoft.Extensions.Logging;

    public class TransactionService : ITransactionService
    {
        private DashboardFunctionSettings _settings;
        private readonly ILogger<TransactionService> _logger;
        private readonly IDataAccessSqlService _dataAccessSqlService;

        public TransactionService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService, DashboardFunctionSettings settings)
        {
            _logger = loggerFactory.CreateLogger<TransactionService>();

            _dataAccessSqlService = dataAccessSqlService;

            _settings = settings;

            _logger.LogInformation($"Constructing {nameof(TransactionService)}");
        }

        public async Task<int> GetDailyTransactiontCountAsync(FilterParam filterParameters)
        {
            int result = 0;

            if (filterParameters == null || filterParameters.FromDate < _settings.MinimumValidCalculationDate || filterParameters.ToDate < _settings.MinimumValidCalculationDate)
            {
                _logger.LogError($"{nameof(GetDailyTransactiontCountAsync)} Invalid or missing parameters");
                throw new ArgumentNullException("Invalid or missing parameters.");
            }
            try
            {
                var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
                result = await _dataAccessSqlService.GetDailyTransactionCountAsync(queryParameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetDailyTransactiontCountAsync)} has an error! : {ex.Message}");
            }
            return result;
        }

        public async Task<decimal> GetDailyTotalRevenueAsync(FilterParam filterParameters)
        {
            Decimal result  = 0;

            if (filterParameters == null || filterParameters.FromDate < _settings.MinimumValidCalculationDate || filterParameters.ToDate < _settings.MinimumValidCalculationDate)
            {
                _logger.LogError($"{nameof(GetDailyTotalRevenueAsync)} Parameter Object or to or from date missing!");
                throw new ArgumentException();
            }
            try
            {
                var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
                result = await _dataAccessSqlService.GetDailyTotalRevenueAsync(queryParameters);
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetDailyTotalRevenueAsync)} has an error! : {ex.Message}");                
            }
            return result;
        }

        public async Task<DashboardDailyAverageOccupancy> GetDailyAverageOccupancy(FilterParam filterParameters)
        {
            DashboardDailyAverageOccupancy? result = null;

            try
            {
                var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
                result = await _dataAccessSqlService.GetDailyAverageOccupancy(queryParameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetDailyAverageOccupancy)} has an error! : {ex.Message}");                
            }
            return result;
        }

        public async Task<DashboardMonthlyTransactionCount> GetMonthlyTransactionCountAsync(FilterParam filterParameters)
        {
            DashboardMonthlyTransactionCount? result = null;

            _logger.LogInformation($"Getting Dashboard Monthly Transaction Count {nameof(GetMonthlyTransactionCountAsync)}");
            if (filterParameters == null || filterParameters.FromDate < _settings.MinimumValidCalculationDate || filterParameters.ToDate < _settings.MinimumValidCalculationDate)
            {
                throw new ArgumentException($"Missing or Invalid parameters.");
            }

            try
            {
                var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
                var monthlyTransactionCounts = await _dataAccessSqlService.GetMonthlyTransactionCountsAsync(queryParameters);
                var results = from TransactionsByMonthAndProduct cnt in monthlyTransactionCounts
                              group cnt by new { cnt.Year, cnt.Month } into monthlyGroup
                              select new TransactionCountForMonth
                              {
                                  Month = monthlyGroup.Key.Year.ToString() + monthlyGroup.Key.Month.ToString(),
                                  Data = monthlyGroup.Select(x => new TransactionsForProduct { NoOfTransactions = x.TransactionCount, Product = x.ParkingProduct })
                              };
                result = new DashboardMonthlyTransactionCount { MonthlyTransactions = results };
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetMonthlyTransactionCountAsync)} has an error! : {ex.Message}");
            }
            return result;
        }

        public async Task<DashboardMonthlyRevenueAndBudget> GetMonthlyRevenueAndBudget(FilterParam filterParameters)
        {
            _logger.LogInformation($"Getting Dashboard Monthly Revenue and Budgeted Revenue {nameof(GetMonthlyRevenueAndBudget)}");
            IEnumerable<RevenueAndBudget>? results = null;

            if (filterParameters == null || filterParameters.FromDate < _settings.MinimumValidCalculationDate || filterParameters.ToDate < _settings.MinimumValidCalculationDate)
            {
                throw new ArgumentException($"Missing or Invalid parameters.");
            }
            try
            {
                var queryParameters = new DashboardFunctionDefaultDataAccessQueryParameters(filterParameters);
                var queryResults = await _dataAccessSqlService.GetMonthlyRevenueAndBudget(queryParameters);
                results = from RevenueAndBudgetForMonth rnb in queryResults
                              select new RevenueAndBudget
                              {
                                  Month = rnb.Year.ToString() + rnb.Month.ToString().PadLeft(2, '0'),
                                  Revenue = rnb.Revenue,
                                  BudgetedRevenue = rnb.BudgetedRevenue
                              };                
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetMonthlyRevenueAndBudget)} has an error! : {ex.Message}");                
            }

            return new DashboardMonthlyRevenueAndBudget { MonthlyRevenueAndBudget = results };
        }
    }
}