using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Service
{
    public class FilterDataService : ServiceBase, IFilterDataService
    {
        #region Constructor
        private readonly IRedisCachingService _cache;
        public FilterDataService(IRepository repository, IRedisCachingService cache)
        {
            ArgumentNullException.ThrowIfNull(repository);
            _repository = repository;
            _cache = cache;
        }
        #endregion

        #region Public Methods

        //public Task<FilterData> GetFiltersData(ServiceLocations custBus)
        //    => _repository.FilterDataRepository.GetFiltersData(custBus);

        public async Task<FilterData> GetFiltersData(ServiceLocations custBus)
        {
            var result = new FilterData();
            //var rawData =  await _repository.FilterDataRepository.GetStsFiltersData();

            var rawData = await _cache.GetStgFilterData();

            var custBuses = custBus.BUs.Select(x => x.Bu).ToArray();

            result.Facilities = rawData.Where(x => x.CustomerId == Convert.ToString(custBus.CustomerId) && custBuses.Contains(x.BuCode)).GroupBy(g => new { g.FacilityId, g.FacilityName }).Select(f => new FacilityData { Id = f.Key.FacilityId, Name = f.Key.FacilityName }).ToList();
            result.Levels = rawData.Where(x => x.CustomerId == Convert.ToString(custBus.CustomerId) && custBuses.Contains(x.BuCode)).Select(l => new LevelData { FacilityId = l.FacilityId, FacilityName = l.FacilityName, Id = l.LevelId, Level = l.Level }).ToList();
            result.Products = rawData.Where(x => x.CustomerId == Convert.ToString(custBus.CustomerId) && custBuses.Contains(x.BuCode)).Select(l => new ProductData { FacilityId = l.FacilityId, FacilityName = l.FacilityName, LevelId = l.LevelId, Level = l.Level, Id = l.ProductId, Name = l.ProductName }).ToList().Distinct();

            return result;
        }
        #endregion

    }
}
