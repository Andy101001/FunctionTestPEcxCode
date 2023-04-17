using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Repository
{
    public class OccupancyRepository<T> : GenericRepository<T> where T : class
    {
        #region Constructor
        public OccupancyRepository(IDapperConnection context) : base(context)
        {

        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(string userId, int customerId)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

            var totalOccRevenue = await SqlMapper.QueryAsync<OccRevenueByProduct>(
                    DapperConnection,
                    Utils.StoredProcs.GetTotalOccRevenue,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return totalOccRevenue;
        }

        public async Task<IEnumerable<OccWeeklyOccByDuration>> GetWeeklyOccByDuration(string userId, int customerId)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

            var result = await SqlMapper.QueryAsync<OccWeeklyOccByDuration>(
                    DapperConnection,
                    Utils.StoredProcs.GetWeeklyOccByDuration,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<OccCurrent>> GetOccCurrent(string userId, int customerId)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

            var result = await SqlMapper.QueryAsync<OccCurrent>(
                    DapperConnection,
                    Utils.StoredProcs.GetOccCurrent,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<AvgMonthlyOccVsDuration>> GetAvgMonthlyOccVsDuration(string userId, int customerId)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

            var result = await SqlMapper.QueryAsync<AvgMonthlyOccVsDuration>(
                    DapperConnection,
                    Utils.StoredProcs.GetAvgMonthlyOccVsDuration,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<YearlyOccupancy>> GetYearlyOccupancy(string userId, int customerId)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

            var result = await SqlMapper.QueryAsync<YearlyOccupancy>(
                    DapperConnection,
                    Utils.StoredProcs.GetYearlyOccupancy,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }
        #endregion

    }
}
