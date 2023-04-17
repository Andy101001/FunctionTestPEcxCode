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
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam inputFilter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RevenueByProduct>> GetRevenueByProductByDays(FilterParam inputFilter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RevenueBudget>> GetRevenueVsBduget(FilterParam inputFilter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CurrentTransaction>> GetTranacionByHours(FilterParam inputFilter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MonthlyTransaction>> GetTransactonMonths(FilterParam inputFilter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
