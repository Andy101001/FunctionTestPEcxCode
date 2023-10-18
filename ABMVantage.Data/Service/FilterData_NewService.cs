using ABMVantage.Data.DataAccess;
using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Microsoft.EntityFrameworkCore;
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
    public class FilterDataService_New : ServiceBase, IFilterDataService_New
    {
        #region Constructor
        private readonly IDbContextFactory<SqlDataContextVTG> _sqlDataContextVTG;
        public FilterDataService_New(IRepository repository, IDbContextFactory<SqlDataContextVTG> sqlDataContextVTG)
        {
            ArgumentNullException.ThrowIfNull(repository);
            _sqlDataContextVTG = sqlDataContextVTG;
            _repository = repository;
        }
        #endregion

        #region Public Methods
        public async Task<FilterData> GetFiltersData(ServiceLocations request)
        {
            var result = new FilterData();
            try
            {
                var custBuList = request.BUs.Select(x => x.Bu).ToArray();

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var rawData = sqlContext.filterDataSQLData;
                result.Facilities = rawData.Where(x => x.CustomerId == request.CustomerId && custBuList.Contains(x.BuCode)).GroupBy(g => new { g.FacilityId, g.FacilityName }).Select(f => new FacilityData { Id = f.Key.FacilityId, Name = f.Key.FacilityName }).ToList().Distinct();
                result.Levels = rawData.Where(x => x.CustomerId == request.CustomerId && custBuList.Contains(x.BuCode) /*&& x.LevelId !=null*/).Select(l => new LevelData { FacilityId = l.FacilityId, FacilityName = l.FacilityName, Id = l.LevelId, Level = l.Level }).Distinct().ToList();
                //var firstFacility = result.Facilities.FirstOrDefault();
                //levels.Insert(0, new LevelData { FacilityId = firstFacility.Id, FacilityName = string.Empty, Id = null, Level = "Show items with no associated Level" });
                result.Products = rawData.Where(x => x.CustomerId == request.CustomerId && custBuList.Contains(x.BuCode) && x.ProductId != null).Select(l => new ProductData { FacilityId = l.FacilityId, FacilityName = l.FacilityName, LevelId = l.LevelId, Level = l.Level, Id = l.ProductId, Name = l.ProductName!}).Distinct().ToList();
                
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return result;
        }
        #endregion

    }
}
