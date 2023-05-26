namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using Microsoft.Azure.Cosmos.Serialization.HybridRow;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Transaction_NewService : ServiceBase, ITransaction_NewService
    {
        private readonly ILogger<Transaction_NewService> _logger;
        private readonly IRedisCachingService _cache;
        private readonly IDataCosmosAccessService _cosmosAccessService;

        public Transaction_NewService(ILoggerFactory loggerFactory, IRepository repository, IRedisCachingService cache, IDataCosmosAccessService cosmosAccessService)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<Transaction_NewService>();
            _repository = repository;
            _cosmosAccessService = cosmosAccessService;
            _cache = cache;
        }

        #region Public Methods

        #region old code

        public Task<IEnumerable<BudgetVariance>> GetBudgetVsActualVariance(FilterParam inputFilter)
            => _repository.TransactionRepository.GetBudgetVsActualVariance(inputFilter);
        public Task<IEnumerable<RevenueByDay>> GetRevenueByDays(FilterParam inputFilter)
            => _repository.TransactionRepository.GetRevenueByDays(inputFilter);
        public Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam inputFilter)
            => _repository.TransactionRepository.GetRevenueByMonths(inputFilter);
        public Task<IEnumerable<RevenueByProduct>> GetRevenueByProductByDays(FilterParam inputFilter)
            => _repository.TransactionRepository.GetRevenueByProductByDays(inputFilter);
        public Task<IEnumerable<RevenueBudget>> GetRevenueVsBudget(FilterParam inputFilter)
            => _repository.TransactionRepository.GetRevenueVsBudget(inputFilter);
        //public Task<IEnumerable<CurrentTransaction>> GetTranacionByHours(FilterParam inputFilter)
        //     => _repository.TransactionRepository.GetTranactionByHours(inputFilter);
        public Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter)
            => _repository.TransactionRepository.GetTransactionByDays(inputFilter);
        public async Task<IEnumerable<CurrentAndPreviousYearMonthlyTransaction>> GetTransactonMonths(FilterParam inputFilter)
        {
            var result = new List<CurrentAndPreviousYearMonthlyTransaction>();
            var currentYearFilter = inputFilter;
            try
            {
                var previousyearFilter = new FilterParam
                {
                    CustomerId = inputFilter.CustomerId,
                    UserId = inputFilter.UserId,
                    Facilities = inputFilter.Facilities,
                    FromDate = inputFilter.FromDate.AddYears(-1),
                    ToDate = inputFilter.ToDate.AddYears(-1),
                    ParkingLevels = inputFilter.ParkingLevels,
                    Products = inputFilter.Products
                };

                var currentYearResults = await _repository.TransactionRepository.GetTransactionMonths(currentYearFilter);
                var previousYearResults = await _repository.TransactionRepository.GetTransactionMonths(previousyearFilter);

                for (DateTime monthStart = inputFilter.FromDate; monthStart <= inputFilter.ToDate; monthStart = monthStart.AddMonths(1))
                {
                    var data = new CurrentAndPreviousYearMonthlyTransaction();
                    data.Month = monthStart.ToString("MMM");
                    var currentYearResult = currentYearResults.FirstOrDefault(x => x.Year == monthStart.Year && x.MonthAsInt == monthStart.Month);
                    var previousYearResult = previousYearResults.FirstOrDefault(x => x.Year == monthStart.Year - 1 && x.MonthAsInt == monthStart.Month);
                    data.NoOfTransactions = currentYearResult?.NoOfTransactions ?? 0;
                    data.PreviousYearNoOfTransactions = previousYearResult?.NoOfTransactions ?? 0;
                    result.Add(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetTransactonMonths)} has an error! : {ex.Message}");
            }

            return result;
        }



        public async Task<IEnumerable<CurrentTransaction>> GetTranacionByHours(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetTransactonByHours(inputFilter);

            return result;
        }
        //public async Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter)
        //{
        //    var result = await _cosmosAccessService.GetTransactonByDays(inputFilter);

        //    return result;
        //}


        #endregion

        #region cosmos code
        /*
        public async Task<IEnumerable<BudgetVariance>> GetBudgetVsActualVariance(FilterParam inputFilter)
        {
           var result = await _cosmosAccessService.GetBudgetVsActualVariance(inputFilter);

            return result;
        }

        public async Task<IEnumerable<RevenueByDay>> GetRevenueByDays(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetRevenueByDays(inputFilter);

            return result;
        }

        public async Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetRevenueByMonths(inputFilter);

            return result;
        }
          
        public async Task<IEnumerable<RevenueByProduct>> GetRevenueByProductByDays(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetRevenueByProductByDays(inputFilter);

            return result;
        }

        public async Task<IEnumerable<RevenueBudget>> GetRevenueVsBudget(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetRevenueVsBudget(inputFilter);

            return result;

        }

        public async Task<IEnumerable<CurrentTransaction>> GetTranacionByHours(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetTransactonByHours(inputFilter);

            return result;
        }

        public async Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetTransactonByDays(inputFilter);

            return result;
        }

        public async Task<IEnumerable<CurrentAndPreviousYearMonthlyTransaction>> GetTransactonMonths(FilterParam inputFilter)
        {
            var result = new List<CurrentAndPreviousYearMonthlyTransaction>();
            var currentYearFilter = inputFilter;
            try
            {
                var previousyearFilter = new FilterParam
                {
                    CustomerId = inputFilter.CustomerId,
                    UserId = inputFilter.UserId,
                    Facilities = inputFilter.Facilities,
                    FromDate = inputFilter.FromDate.AddYears(-1),
                    ToDate = inputFilter.ToDate.AddYears(-1),
                    ParkingLevels = inputFilter.ParkingLevels,
                    Products = inputFilter.Products
                };



                var currentYearResults = await _cosmosAccessService.GetTransactonByMonth(currentYearFilter);
                
                var data2 = currentYearResults.ToList();

                var previousYearResults = await _cosmosAccessService.GetTransactonByMonth(previousyearFilter);

                for (DateTime monthStart = inputFilter.FromDate; monthStart <= inputFilter.ToDate; monthStart = monthStart.AddMonths(1))
                {
                    var data = new CurrentAndPreviousYearMonthlyTransaction();
                    data.Month = monthStart.ToString("MMM");
                    var currentYearResult = currentYearResults.FirstOrDefault(x => x.Year == monthStart.Year && x.MonthAsInt == monthStart.Month);
                    var previousYearResult = previousYearResults.FirstOrDefault(x => x.Year == monthStart.Year - 1 && x.MonthAsInt == monthStart.Month);
                    data.NoOfTransactions = currentYearResult?.NoOfTransactions ?? 0;
                    data.PreviousYearNoOfTransactions = previousYearResult?.NoOfTransactions ?? 0;
                    result.Add(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetTransactonMonths)} has an error! : {ex.Message}");
            }

            return result;
        }
        */
        #endregion

        //TODO: DO NOT DELTE BELOW CODE



        //public Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter)
        //    => _repository.TransactionRepository.GetTransactionByDays(inputFilter);


        //public async Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter)
        //{
        //    IEnumerable<DailyTransaction>? result = null;
        //    result = await _cache.GetStgTransactonByDays(inputFilter);

        //    return result;
        //}








        //GetTransactonByMonth

        #endregion Public Methods
    }
}