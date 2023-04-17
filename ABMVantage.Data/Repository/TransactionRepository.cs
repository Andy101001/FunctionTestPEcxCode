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
    public class TransactionRepository<T> : GenericRepository<T> where T : class
    {
        #region Constructor
        public TransactionRepository(IDapperConnection context) : base(context)
        {

        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<BudgetVariance>> GetBudgetVsActualVriance(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<BudgetVariance>(
                    DapperConnection,
                    Utils.StoredProcs.GetHourlyReservations,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<RevenueByDay>> GetRevenueByDays(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<RevenueByDay>(
                    DapperConnection,
                    Utils.StoredProcs.GetDailyReservations,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<MonthlyRevenue>> GetRevenueByMonths(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<MonthlyRevenue>(
                    DapperConnection,
                    Utils.StoredProcs.GetMonthlyReservations,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<RevenueByProduct>> GetRevenueByProductByDays(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<RevenueByProduct>(
                    DapperConnection,
                    Utils.StoredProcs.GetReservationsAvgTkt,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<RevenueBudget>> GetRevenueVsBduget(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<RevenueBudget>(
                    DapperConnection,
                    Utils.StoredProcs.GetReservationsAvgTkt,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<CurrentTransaction>> GetTranacionByHours(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<CurrentTransaction>(
                    DapperConnection,
                    Utils.StoredProcs.GetReservationsAvgTkt,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<DailyTransaction>> GetTransactonByDays(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<DailyTransaction>(
                    DapperConnection,
                    Utils.StoredProcs.GetReservationsAvgTkt,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<MonthlyTransaction>> GetTransactonMonths(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<MonthlyTransaction>(
                    DapperConnection,
                    Utils.StoredProcs.GetReservationsAvgTkt,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        #endregion

        #region Private Methods
        private DynamicParameters GetInputParam(FilterParam inputParam)
        {
            //var facilities = inputParam.Facilities != null ? inputParam.Facilities.ToList() : new List<FacilityFilter>();
            //var parkingLevels = inputParam.ParkingLevels != null ? inputParam.ParkingLevels.ToList() : new List<LevelFilter>();
            //var products = inputParam.Products != null ? inputParam.Products.ToList() : new List<ProductFilter>();

            var productIds = inputParam.Products != null ?  string.Join(",", inputParam.Products.Select(x => x.Id)) : "";
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
        #endregion
    }
}
