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

    public class OccupancyRepository<T> : GenericRepository<T>, IOccupancyRepository where T : class
    {
        private readonly ILogger<OccupancyRepository<T>> _logger;

        #region Constructor

        public OccupancyRepository(ILoggerFactory loggerFactory, IDapperConnection context) : base(loggerFactory, context)
        {
            _logger = loggerFactory.CreateLogger<OccupancyRepository<T>>();
        }

        #endregion Constructor

        #region Public Methods

        public async Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(FilterParam inputFilter)
        {
            IEnumerable<OccRevenueByProduct>? totalOccRevenue = null;
            try
            {
                var dynamicParams = GetInputParam(inputFilter);
                //dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
                //dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

                totalOccRevenue = await SqlMapper.QueryAsync<OccRevenueByProduct>(
                        DapperConnection,
                        Utils.StoredProcs.GetTotalOccRevenue,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetTotalOccRevenue)} has an error! : {ex.Message}");
            }

            return totalOccRevenue;
        }

        public async Task<IEnumerable<OccWeeklyOccByDuration>> GetWeeklyOccByDuration(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<OccWeeklyOccByDuration>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<OccWeeklyOccByDuration>(
                       DapperConnection,
                       Utils.StoredProcs.GetWeeklyOccByDuration,
                       param: dynamicParams,
                       commandType: CommandType.StoredProcedure
                   );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetWeeklyOccByDuration)} has an error! : {ex.Message}");
            }

            return result;
        }

        public async Task<IEnumerable<OccCurrent>> GetOccCurrent(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<OccCurrent>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<OccCurrent>(
                       DapperConnection,
                       Utils.StoredProcs.GetOccCurrent,
                       param: dynamicParams,
                       commandType: CommandType.StoredProcedure
                   );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetOccCurrent)} has an error! : {ex.Message}");
            }

            return result;
        }

        public async Task<IEnumerable<AvgMonthlyOccVsDuration>> GetAvgMonthlyOccVsDuration(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<AvgMonthlyOccVsDuration>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<AvgMonthlyOccVsDuration>(
                       DapperConnection,
                       Utils.StoredProcs.GetAvgMonthlyOccVsDuration,
                       param: dynamicParams,
                       commandType: CommandType.StoredProcedure
                   );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetAvgMonthlyOccVsDuration)} has an error! : {ex.Message}");
            }

            return result;
        }

        public async Task<IEnumerable<YearlyOccupancy>> GetYearlyOccupancy(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<YearlyOccupancy>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<YearlyOccupancy>(
                       DapperConnection,
                       Utils.StoredProcs.GetYearlyOccupancy,
                       param: dynamicParams,
                       commandType: CommandType.StoredProcedure
                   );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetYearlyOccupancy)} has an error! : {ex.Message}");
            }
            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private DynamicParameters GetInputParam(FilterParam inputParam)
        {
            //var facilities = inputParam.Facilities != null ? inputParam.Facilities.ToList() : new List<FacilityFilter>();
            //var parkingLevels = inputParam.ParkingLevels != null ? inputParam.ParkingLevels.ToList() : new List<LevelFilter>();
            //var products = inputParam.Products != null ? inputParam.Products.ToList() : new List<ProductFilter>();

            var productIds = inputParam.Products != null ? string.Join(",", inputParam.Products.Select(x => x.Id)) : "";
            var parkingLevelIds = inputParam.ParkingLevels != null ? string.Join(",", inputParam.ParkingLevels.Select(x => x.Id)) : "";
            var facilityIds = inputParam.Facilities != null ? string.Join(",", inputParam.Facilities.Select(x => x.Id)) : "";

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", inputParam.UserId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", inputParam.CustomerId, DbType.Int32, ParameterDirection.Input);
            dynamicParams.Add("@FromDate", inputParam.FromDate, DbType.DateTime2, ParameterDirection.Input);
            dynamicParams.Add("@ToDate", inputParam.ToDate, DbType.DateTime2, ParameterDirection.Input);
            dynamicParams.Add("@Facilities", facilityIds, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@ParkingLevels", parkingLevelIds, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@Products", productIds, DbType.String, ParameterDirection.Input);

            return dynamicParams;
        }

        #endregion Private Methods
    }
}