using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Repository
{
    public class EVChargerRepository<T> : GenericRepository<T>, IEVChargerLocationRepository where T : class
    {
        private readonly ILogger<EVChargerRepository<T>> _logger;

        #region Constructor

        public EVChargerRepository(ILoggerFactory loggerFactory, IDapperConnection context) : base(loggerFactory, context)
        {
            _logger = loggerFactory.CreateLogger<EVChargerRepository<T>>();
        }

        #endregion Constructor

        #region Public Methods

        public async Task<IEnumerable<MSPageLoadResponse>> GetChargerLocation(MSPageLoadRequest inputParam)
        {
            var dynamicParams = GetInputParam2(inputParam);
            IEnumerable<MSPageLoadResponse>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<MSPageLoadResponse>(
                        DapperConnection,
                        Utils.StoredProcs.GetEVMicroSitePageLoadValidation,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetChargerLocation)} has an error! : {ex.Message}");
            }
            return result;
        }

        public async Task<IEnumerable<MSCharagerInitiateResponse>> GetChargerInitiate(MSCharagerInitiateRequest inputParam)
        {
            var dynamicParams = GetInputParamCharagerInitiate(inputParam);
            IEnumerable<MSCharagerInitiateResponse>? result = null;
            try
            {
                result = await SqlMapper.QueryAsync<MSCharagerInitiateResponse>(
                        DapperConnection,
                        Utils.StoredProcs.GetEVMicroSiteFormSubmission,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetChargerInitiate)} has an error! : {ex.Message}");
            }
            return result;
        }



        #endregion Public Methods

        #region Private Methods


        private DynamicParameters GetInputParamCharagerInitiate(MSCharagerInitiateRequest inputParam)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@deviceId", inputParam.QrCode, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@ticketNumberOrReservationId", inputParam.TicketOrReservationId, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@lpn", inputParam.lpn, DbType.String, ParameterDirection.Input);

            return dynamicParams;
        }
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

        private DynamicParameters GetInputParam2(MSPageLoadRequest inputParam)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@deviceId", inputParam.QrCode, DbType.String, ParameterDirection.Input);
            dynamicParams.Add("@latitude", inputParam.Latitude, DbType.Decimal, ParameterDirection.Input);
            dynamicParams.Add("@longitude", inputParam.Longitude, DbType.Decimal, ParameterDirection.Input);

            return dynamicParams;
        }

       

        #endregion Private Methods
    }
}