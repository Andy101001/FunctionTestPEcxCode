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

    public class ReservationsRepository<T> : GenericRepository<T>, IReservationsRepository where T : class
    {
        private readonly ILogger<ReservationsRepository<T>> _logger;

        #region Constructor

        public ReservationsRepository(ILoggerFactory loggerFactory, IDapperConnection context) : base(loggerFactory, context)
        {
            _logger = loggerFactory.CreateLogger<ReservationsRepository<T>>();
        }

        #endregion Constructor

        #region Public Methods

        public async Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<ReservationsByHour>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<ReservationsByHour>(
                       DapperConnection,
                       Utils.StoredProcs.GetHourlyReservations,
                       param: dynamicParams,
                       commandType: CommandType.StoredProcedure
                   );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetHourlyReservations)} has an error! : {ex.Message}");
            }

            return result;
        }

        public async Task<IEnumerable<ReservationsByDay>> GetDailyReservations(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<ReservationsByDay>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<ReservationsByDay>(
                        DapperConnection,
                        Utils.StoredProcs.GetDailyReservations,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetDailyReservations)} has an error! : {ex.Message}");
            }
            return result;
        }

        public async Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<ReservationsByMonth>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<ReservationsByMonth>(
                        DapperConnection,
                        Utils.StoredProcs.GetMonthlyReservations,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetMonthlyReservations)} has an error! : {ex.Message}");
            }
            return result;
        }

        public async Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);
            IEnumerable<ResAvgTicketValue>? result = null;

            try
            {
                result = await SqlMapper.QueryAsync<ResAvgTicketValue>(
                        DapperConnection,
                        Utils.StoredProcs.GetReservationsAvgTkt,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetReservationsAvgTkt)} has an error! : {ex.Message}");
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