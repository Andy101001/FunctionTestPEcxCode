using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Service
{
    public class Transaction_NewService : ServiceBase, ITransaction_NewService
    {
        public Transaction_NewService(IRepository repository) 
        {
            _repository = repository;
        }

        #region Public Methods
        public Task<IEnumerable<BudgetVariance>> GetBudgetVsActualVriance(FilterParam inputFilter)
            => _repository.TransactionRepository.GetBudgetVsActualVriance(inputFilter);

        public Task<IEnumerable<RevenueByDay>> GetRevenueByDays(FilterParam inputFilter)
            => _repository.TransactionRepository.GetRevenueByDays(inputFilter);

        public Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam inputFilter)
            => _repository.TransactionRepository.GetRevenueByMonths(inputFilter);

        public Task<IEnumerable<RevenueByProduct>> GetRevenueByProductByDays(FilterParam inputFilter)
            => _repository.TransactionRepository.GetRevenueByProductByDays(inputFilter);
        public Task<IEnumerable<RevenueBudget>> GetRevenueVsBduget(FilterParam inputFilter)
            => _repository.TransactionRepository.GetRevenueVsBduget(inputFilter);

        public Task<IEnumerable<CurrentTransaction>> GetTranacionByHours(FilterParam inputFilter)
             => _repository.TransactionRepository.GetTranactionByHours(inputFilter);

        public Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter)
            => _repository.TransactionRepository.GetTransactionByDays(inputFilter);

        public async Task<IEnumerable<CurrentAndPreviousYearMonthlyTransaction>> GetTransactonMonths(FilterParam inputFilter)
        {
            var currentYearFilter = inputFilter;
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
            var result = new List<CurrentAndPreviousYearMonthlyTransaction>();
            for (DateTime monthStart = inputFilter.FromDate; monthStart <= inputFilter.ToDate; monthStart = monthStart.AddMonths(1))
            {
                var data = new CurrentAndPreviousYearMonthlyTransaction();
                data.Month = monthStart.ToString("MMM");
                var currentYearResult = currentYearResults.FirstOrDefault(x => x.Year == monthStart.Year &&  x.MonthAsInt == monthStart.Month);
                var previousYearResult = previousYearResults.FirstOrDefault(x => x.Year == monthStart.Year - 1 && x.MonthAsInt == monthStart.Month);
                data.NoOfTransactions = currentYearResult?.NoOfTransactions ?? 0;
                data.PreviousYearNoOfTransactions = previousYearResult?.NoOfTransactions ?? 0;
                result.Add(data);
            }
            return result;
        }

        #endregion
    }
}
