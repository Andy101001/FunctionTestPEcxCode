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
             => _repository.TransactionRepository.GetTranacionByHours(inputFilter);

        public Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter)
            => _repository.TransactionRepository.GetTransactonByDays(inputFilter);

        public Task<IEnumerable<MonthlyTransaction>> GetTransactonMonths(FilterParam inputFilter)
            => _repository.TransactionRepository.GetTransactonMonths(inputFilter);

        #endregion
    }
}
