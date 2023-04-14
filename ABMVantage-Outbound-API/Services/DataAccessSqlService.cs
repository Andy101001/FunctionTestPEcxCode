using ABMVantage_Outbound_API.DataAccess;
using ABMVantage_Outbound_API.EntityModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public class DataAccessSqlService : IDataAccessSqlService
    {

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<DataAccessSqlService> _logger;

        /// <summary>
        /// Factory to generate <see cref="SqlDataContext"/> instances.
        /// </summary>
        private readonly IDbContextFactory<SqlDataContext> _dbSqlContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessSqlService"/> class.
        /// </summary>
        /// <param name="dbSqlContextFactory"></param>
        /// <param name="loggerFactory"></param>
        public DataAccessSqlService(IDbContextFactory<SqlDataContext> dbSqlContextFactory, ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);
            ArgumentNullException.ThrowIfNull(dbSqlContextFactory);

            _logger = loggerFactory.CreateLogger<DataAccessSqlService>();
            _dbSqlContextFactory = dbSqlContextFactory;

            _logger.LogInformation($"Constructing {nameof(DataAccessSqlService)}");

        }

        public async Task<IList<DimFacility>> GetFacilityAsync(string id)
        {
            var lstFacility=new List<DimFacility>();

            using(var db= _dbSqlContextFactory.CreateDbContext())
            {
                
                var dbSet = from c in db.DimCustomers
                                join l in db.DimLocations on c.BuCode equals l.BuCode
                                join f in db.DimFacilities on l.LocationId equals f.LocationId
                                where c.CustomerId == id
                                select new DimFacility { LocationId=l.LocationId, FacilityId = f.FacilityId,  FacilityName = f.FacilityName };

                lstFacility = await dbSet.ToListAsync();

            }



            return lstFacility;


        }

        public async Task<List<DimLevel>> GetLevelAsync(string id)
        {
            var lstLevel = new List<DimLevel>();

            using (var db = _dbSqlContextFactory.CreateDbContext())
            {

                lstLevel = await db.DimLevels.Where(d => d.FacilityId.Equals(id)).ToListAsync();
   
            }

            return lstLevel;
        }

        public async Task<List<DimProduct>> GetProductAsync(string id)
        {
            var lstLevel = new List<DimProduct>();

            using (var db = _dbSqlContextFactory.CreateDbContext())
            {

                var dbSet= from dsp in db.DimParkingSpaces
                           join sp in db.SpaceProducts on dsp.ParkingSpaceId equals sp.ParkingSpaceId
                           join p in db.DimProducts on sp.ParkingProductId equals p.ProductId
                           select new DimProduct { ProductId=p.ProductId, ProductName=p.ProductName };

                lstLevel= await dbSet.ToListAsync();
            }

            return lstLevel;
        }


        //public async Task<int> GetDailyTransactionCountAsync(DateTime calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        //{
        //    var lstLevel = new List<DimProduct>();
        //    int dailyCount = 0;

        //    using (var db = _dbSqlContextFactory.CreateDbContext())
        //    {

        //        //var dbSet = from pas in db.FactPaymentsTicketAndStageds
        //        //            join ft in db.FactTickets on pas.TicketId equals ft.TicketId
        //        //            join f in db.DimFacilities on ft.FacilityId equals f.FacilityId
        //        //            ;


                
        //    }

        //    return dailyCount;
        //}

        public async Task<decimal> GetDailyTotalRevenueAsync(DateTime calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            var lstLevel = new List<DimProduct>();
            decimal dailyCount = 0;

            using (var db = _dbSqlContextFactory.CreateDbContext())
            {
                /*
                SqlParameter param1 = new SqlParameter("@parkingProductId", parkingProductId);
                SqlParameter param2 = new SqlParameter("@facilityId", facilityId);
                SqlParameter param3 = new SqlParameter("@StartDate", calculationDate);
                SqlParameter param4 = new SqlParameter("@facilityId", facilityId);
                string endDate = "2022-12-09 23:59:59.000";
                string startDate = "2022-07-08 05:00:00.000";

                var conn = new SqlConnection(db.Database.GetConnectionString());
                conn.Open();

                string sql = $"EXEC BASE.DailyTotalRevenue '{parkingProductId}','{facilityId}','{startDate}','{endDate}','{levelId}'";

                sql = "EXEC BASE.DailyTotalRevenue '2545','LAX3576BLDG01','2022-07-08 05:00:00.000','2022-12-09 23:59:59.000','AGPK01_05'";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                var rdr = await cmd.ExecuteScalarAsync();
                //while(rdr.Read())
                //{
                    dailyCount= Convert.ToDecimal(rdr);
                //}


                //var result = db.Database.SqlQuery<decimal>($"{sql}").AsEnumerable();
                */

                ///TODO this has to do better way
                var result = db.Database.SqlQuery<decimal>($"EXEC [BASE].[DailyTotalRevenue] '2545', 'LAX3576BLDG01','2022-07-08 05:00:00.000','2022-12-09 23:59:59.000','AGPK01_05'").AsEnumerable();


                dailyCount = result.FirstOrDefault();

            }

            return dailyCount;
        }

    }
}
