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
    public class ReservationsRepository<T> : GenericRepository<T> where T : class
    {
        #region Constructor
        public ReservationsRepository(IDapperConnection context) : base(context)
        {

        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<ReservationsByHour>(
                    DapperConnection,
                    Utils.StoredProcs.GetHourlyReservations,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<ReservationsByDay>> GetDailyReservations(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<ReservationsByDay>(
                    DapperConnection,
                    Utils.StoredProcs.GetDailyReservations,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<ReservationsByMonth>(
                    DapperConnection,
                    Utils.StoredProcs.GetMonthlyReservations,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<ResAvgTicketValue>(
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
            var facilities = inputParam.Facilities != null ? inputParam.Facilities.ToList() : new List<FacilityFilter>();
            var parkingLevels = inputParam.ParkingLevels != null ? inputParam.ParkingLevels.ToList() : new List<LevelFilter>();
            var products = inputParam.Products != null ? inputParam.Products.ToList() : new List<ProductFilter>();

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@UserId", inputParam.UserId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@CustomerId", inputParam.CustomerId, DbType.Int32, ParameterDirection.Input);
            dynamicParams.Add("@FromDate", inputParam.FromDate, DbType.DateTime2, ParameterDirection.Input);
            dynamicParams.Add("@ToDate", inputParam.ToDate, DbType.DateTime2, ParameterDirection.Input);
            dynamicParams.Add("@Facilities", facilities.ToDataTable().AsTableValuedParameter("[BASE].[FACILITY_INPUT]"));
            dynamicParams.Add("@ParkingLevels", parkingLevels.ToDataTable().AsTableValuedParameter("[BASE].[PARKING_LEVEL_INPUT]"));
            dynamicParams.Add("@Products", products.ToDataTable().AsTableValuedParameter("[BASE].[PRODUCT_INPUT]"));

            return dynamicParams;
        }
        #endregion
    }
}
