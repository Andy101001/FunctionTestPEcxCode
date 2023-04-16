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
    public class ReservationsRepository<T> : GenericRepository<T> where T : class
    {
        #region Constructor
        public ReservationsRepository(IDapperConnection context) : base(context)
        {

        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(string userId, int customerId)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

            var result = await SqlMapper.QueryAsync<ReservationsByHour>(
                    DapperConnection,
                    Utils.StoredProcs.GetHourlyReservations,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<ReservationsByDay>> GetDailyReservations(string userId, int customerId)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

            var result = await SqlMapper.QueryAsync<ReservationsByDay>(
                    DapperConnection,
                    Utils.StoredProcs.GetDailyReservations,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(string userId, int customerId)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

            var result = await SqlMapper.QueryAsync<ReservationsByMonth>(
                    DapperConnection,
                    Utils.StoredProcs.GetMonthlyReservations,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(string userId, int customerId)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

            var result = await SqlMapper.QueryAsync<ResAvgTicketValue>(
                    DapperConnection,
                    Utils.StoredProcs.GetReservationsAvgTkt,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }
        #endregion
    }
}
