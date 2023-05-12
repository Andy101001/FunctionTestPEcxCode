namespace ABMVantage.Data.Repository
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using Dapper;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class FilterDataRepository<T> : GenericRepository<T>, IFilterDataRepository where T : class
    {
        private readonly ILogger<FilterDataRepository<T>> _logger;

        #region Constructor

        public FilterDataRepository(ILoggerFactory loggerFactory, IDapperConnection context) : base(loggerFactory, context)
        {
            _logger = loggerFactory.CreateLogger<FilterDataRepository<T>>();
        }

        #endregion Constructor

        #region Public Methods

        public async Task<FilterData> GetFiltersData(ServiceLocations custBus)
        {
            var result = new FilterData();
            try
            {
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

                result.Facilities = rawData.GroupBy(g => new { g.FacilityId, g.FacilityName }).Select(f => new FacilityData { Id = f.Key.FacilityId, Name = f.Key.FacilityName }).ToList();
                result.Levels = rawData.Select(l => new LevelData { FacilityId = l.FacilityId, FacilityName = l.FacilityName, Id = l.LevelId, Level = l.Level }).ToList();

                var productsData = await SqlMapper.QueryAsync<ProductData>(
                        DapperConnection,
                        Utils.StoredProcs.GetProductsData,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                result.Products = productsData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetFiltersData)} has an error! : {ex.Message}");
            }

            return result;
        }

        public async Task<IList<StgFilterData>> GetStsFiltersData()
        {
            var result = new List<StgFilterData>();

            try
            {
                var data = await SqlMapper.QueryAsync<StgFilterData>(
                        DapperConnection,
                        Utils.StoredProcs.GetStgFilterData,
                        commandType: CommandType.StoredProcedure
                    );
                result = data.ToList();


            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetStsFiltersData)} has an error! : {ex.Message}");
            }

            return result;
        }

        #endregion Public Methods
    }
}