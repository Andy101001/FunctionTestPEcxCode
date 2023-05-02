﻿using ABMVantage.Data.Interfaces;
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
    public class OccupancyRepository<T> : GenericRepository<T>, IOccupancyRepository where T : class
    {
        #region Constructor
        public OccupancyRepository(IDapperConnection context) : base(context)
        {

        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(FilterParam inputFilter)
        {
            try
            {
                var dynamicParams = GetInputParam(inputFilter);
                //dynamicParams.Add("@UserId", userId, DbType.String, ParameterDirection.Input);
                //dynamicParams.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);

                var totalOccRevenue = await SqlMapper.QueryAsync<OccRevenueByProduct>(
                        DapperConnection,
                        Utils.StoredProcs.GetTotalOccRevenue,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                return totalOccRevenue;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<IEnumerable<OccWeeklyOccByDuration>> GetWeeklyOccByDuration(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<OccWeeklyOccByDuration>(
                    DapperConnection,
                    Utils.StoredProcs.GetWeeklyOccByDuration,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<OccCurrent>> GetOccCurrent(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<OccCurrent>(
                    DapperConnection,
                    Utils.StoredProcs.GetOccCurrent,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<AvgMonthlyOccVsDuration>> GetAvgMonthlyOccVsDuration(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<AvgMonthlyOccVsDuration>(
                    DapperConnection,
                    Utils.StoredProcs.GetAvgMonthlyOccVsDuration,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<YearlyOccupancy>> GetYearlyOccupancy(FilterParam inputFilter)
        {
            var dynamicParams = GetInputParam(inputFilter);

            var result = await SqlMapper.QueryAsync<YearlyOccupancy>(
                    DapperConnection,
                    Utils.StoredProcs.GetYearlyOccupancy,
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
        #endregion

    }
}
