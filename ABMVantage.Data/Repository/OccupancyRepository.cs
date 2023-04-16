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
        #endregion

    }
}
