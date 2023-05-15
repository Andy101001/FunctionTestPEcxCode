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

    public class TransactionRepository<T> : GenericRepository<T>, ITransactionRepository where T : class
    {
        private readonly ILogger<TransactionRepository<T>> _logger;

        #region Constructor

        public TransactionRepository(ILoggerFactory loggerFactory, IDapperConnection context) : base(loggerFactory, context)
        {
            _logger = loggerFactory.CreateLogger<TransactionRepository<T>>();
        }

        #endregion Constructor

        #region Public Methods

        public async Task<IEnumerable<BudgetVariance>> GetBudgetVsActualVariance(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<BudgetVariance>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<BudgetVariance>(
                        DapperConnection,
                        Utils.StoredProcs.GetBudgetVsActualVriance,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetBudgetVsActualVariance)} has an error! : {ex.Message}");
            }
            return result;
        }

        public async Task<IEnumerable<RevenueByDay>> GetRevenueByDays(FilterParam inputFilter)
        {
            IEnumerable<RevenueByDay>? result = null;
            try
            {
                var dynamicParams = GetInputParam(inputFilter);

                result = await SqlMapper.QueryAsync<RevenueByDay>(
                        DapperConnection,
                        Utils.StoredProcs.GetRevenueByDay,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetRevenueByDays)} has an error! : {ex.Message}");
            }
            return result;
        }

        public async Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<MonthlyRevenue>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<MonthlyRevenue>(
                       DapperConnection,
                       Utils.StoredProcs.GetRevenueByMonth,
                       param: dynamicParams,
                       commandType: CommandType.StoredProcedure
                   );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetRevenueByMonths)} has an error! : {ex.Message}");
            }
            return result;
        }

        public async Task<IEnumerable<RevenueByProduct>> GetRevenueByProductByDays(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<RevenueByProduct>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<RevenueByProduct>(
                        DapperConnection,
                        Utils.StoredProcs.GetRevenueByProductByDays,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetRevenueByProductByDays)} has an error! : {ex.Message}");
            }
            return result;
        }

        public async Task<IEnumerable<RevenueBudget>> GetRevenueVsBudget(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<RevenueBudget>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<RevenueBudget>(
                       DapperConnection,
                       Utils.StoredProcs.GetRevenueVsBduget,
                       param: dynamicParams,
                       commandType: CommandType.StoredProcedure
                   );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetRevenueVsBudget)} has an error! : {ex.Message}");
            }
            return result;
        }

        public async Task<IEnumerable<CurrentTransaction>> GetTranactionByHours(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<CurrentTransaction>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<CurrentTransaction>(
                        DapperConnection,
                        Utils.StoredProcs.GetTranacionByHours,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetTranactionByHours)} has an error! : {ex.Message}");
            }
            return result;
        }

        //public async Task<IEnumerable<DailyTransaction>> GetTransactionByDays(FilterParam inputFilter)
        //{
        //    var dynamicParams = GetInputParam(inputFilter);
        //    IEnumerable<DailyTransaction>? result = null;
        //    try
        //    {
        //        result = await SqlMapper.QueryAsync<DailyTransaction>(
        //               DapperConnection,
        //               Utils.StoredProcs.GetTransactonByDays,
        //               param: dynamicParams,
        //               commandType: CommandType.StoredProcedure
        //           );
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"{nameof(GetTransactionByDays)} has an error! : {ex.Message}");
        //    }
        //    return result;
        //}

        public async Task<IEnumerable<StgDailyTransaction>> GetTransactionByDays(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<StgDailyTransaction>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<StgDailyTransaction>(
                       DapperConnection,
                       Utils.StoredProcs.GetCacheTransactonByDays,
                       commandType: CommandType.StoredProcedure
                   );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetTransactionByDays)} has an error! : {ex.Message}");
            }
            return result;
        }


        public async Task<IEnumerable<MonthlyTransaction>> GetTransactionMonths(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<MonthlyTransaction>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<MonthlyTransaction>(
                        DapperConnection,
                        Utils.StoredProcs.GetTransactonMonths,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetTransactionMonths)} has an error! : {ex.Message}");
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

        public static implicit operator TransactionRepository<T>(TransactionRepository<BudgetVariance> v)
        {
            throw new NotImplementedException();
        }

        #endregion Private Methods
    }
}