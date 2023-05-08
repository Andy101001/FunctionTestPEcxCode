using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Service
{
    public class FilterDataService : ServiceBase, IFilterDataService
    {
        #region Constructor
        public FilterDataService(IRepository repository)
        {
            ArgumentNullException.ThrowIfNull(repository);
            _repository = repository;
        }
        #endregion

        #region Public Methods
        
        public Task<FilterData> GetFiltersData(ServiceLocations custBus)
            => _repository.FilterDataRepository.GetFiltersData(custBus);
        #endregion

    }
}
