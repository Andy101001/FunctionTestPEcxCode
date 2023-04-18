using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using ABMVantage.Data.Tools;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ABMVantage.Data.Repository
{
    public class FilterDataRepository<T> : GenericRepository<T> where T : class
    {
        #region Constructor
        public FilterDataRepository(IDapperConnection context) : base(context)
        {

        }
        #endregion

        #region Public Methods
        public async Task<FilterData> GetFiltersData(ServiceLocations custBus)
        {
            var result = new FilterData();

            var customerBus = custBus != null ? string.Join(",", custBus.BUs.Select(x => x.Bu)) : "";

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", custBus.UserId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", custBus.CustomerId, DbType.Int32, ParameterDirection.Input);
            dynamicParams.Add("@Bus", customerBus, DbType.String, ParameterDirection.Input);

            var rawData = await SqlMapper.QueryAsync<FilterRawData>(
                    DapperConnection,
                    Utils.StoredProcs.GetFiltersData,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            result.Facilities = rawData.Select(f => new FacilityData { Id = f.FacilityId, Name = f.FacilityName }).Distinct().ToList(); 
            result.Levels = rawData.Select( l => new LevelData { FacilityId = l.FacilityId, Id = l.LevelId, Level = l.Level} ).ToList();

            var productsData = await SqlMapper.QueryAsync<ProductData>(
                    DapperConnection,
                    Utils.StoredProcs.GetProductsData,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            result.Products = productsData;

            return result;
        }
        #endregion
    }
}
