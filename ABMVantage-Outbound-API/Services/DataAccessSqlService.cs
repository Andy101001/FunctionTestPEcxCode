using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.DataAccess;
using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Functions;
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

        
        public async Task<DashboardDailyAverageOccupancy> GetDailyAverageOccupancy(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {
        
            int dailyCount = 0;
            var occupancy =new DashboardDailyAverageOccupancy();
            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {
                    ///TOO: Synapse DB does not have properdata so hardcoding date parameters
                    ///This is to change with calculate date
                    string endDate = "2022-12-09 23:59:59.000";
                    string startDate = "2022-07-08 05:00:00.000";

                    var conn = new SqlConnection(db.Database.GetConnectionString());
                    conn.Open();

                    string sql = $"EXEC BASE.DailyAverageOccupancy '{parkingProductId}','{facilityId}','{startDate}','{endDate}','{levelId}'";

                    //string sql = "EXEC BASE.DailyAverageOccupancy '2545','LAX3576BLDG01','2022-07-08 05:00:00.000','2022-12-09 23:59:59.000','AGPK01_05'";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    //var rdr = await cmd.ExecuteScalarAsync();
                    var rdr = await cmd.ExecuteReaderAsync();
                    while (rdr.Read())
                    {
                        occupancy.AverageDailyOccupancyInteger = Convert.ToInt32(rdr["averageOccupancy"]);
                        occupancy.AverageDailyOccupancyPercentage= Convert.ToInt32(rdr["averageOccupancyPercentage"]);
                    }

                    //dailyCount = Convert.ToInt32(rdr);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"{nameof(DataAccessSqlService)} {ex.Message}");
                throw;
            }
            

            return occupancy;
        }

        public async Task<decimal> GetDailyTotalRevenueAsync(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            decimal dailyCount = 0;

            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {
                    ///TOO: Synapse DB does not have properdata so hardcoding date parameters
                    ///This is to change with calculate date
                    ///
                    string endDate = "2022-12-09 23:59:59.000";
                    string startDate = "2022-07-08 05:00:00.000";

                    var conn = new SqlConnection(db.Database.GetConnectionString());
                    conn.Open();

                    string sql = $"EXEC BASE.DailyTotalRevenue '{parkingProductId}','{facilityId}','{startDate}','{endDate}','{levelId}'";

                    //string sql = "EXEC BASE.DailyTransaction '2545','LAX3576BLDG01','2022-07-08 05:00:00.000','2022-12-09 23:59:59.000','AGPK01_05'";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    var rdr = await cmd.ExecuteScalarAsync();

                    dailyCount = Convert.ToDecimal(rdr);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DataAccessSqlService)} {ex.Message}");
                throw;
            }


            return dailyCount;
        }

        public async Task<int> GetDailyTransactionCountAsync(DateTime? transactionDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            int dailyCount = 0;
            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {
                    ///TOO: Synapse DB does not have properdata so hardcoding date parameters
                    ///This is to change with calculate date
                    ///
                    string endDate = "2022-12-09 23:59:59.000";
                    string startDate = "2022-07-08 05:00:00.000";

                    var conn = new SqlConnection(db.Database.GetConnectionString());
                    conn.Open();

                    string sql = $"EXEC DailyTransaction '{parkingProductId}','{facilityId}','{startDate}','{endDate}','{levelId}'";

                    //string sql = "EXEC BASE.DailyTransaction '2545','LAX3576BLDG01','2022-07-08 05:00:00.000','2022-12-09 23:59:59.000','AGPK01_05'";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    var rdr = await cmd.ExecuteScalarAsync();

                    dailyCount = Convert.ToInt32(rdr);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DataAccessSqlService)} {ex.Message}");
                throw;
            }
            
            return dailyCount;
        }

        public async Task<IList<DashboardFuctionDayRevenue>> GetRevnueByDay(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {

            var lstRevnue=new List<DashboardFuctionDayRevenue>();
             
            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {
                    ///TOO: Synapse DB does not have properdata so hardcoding date parameters
                    ///This is to change with calculate date
                    ///
                    string endDate = "2023-04-20 00:00:00.000";
                    string startDate = "2023-04-10 00:00:00.000";

                    var conn = new SqlConnection(db.Database.GetConnectionString());
                    conn.Open();

                    string sql = $"EXEC BASE.RevenueByDay '{parkingProductId}','{facilityId}','{levelId}','{startDate}','{endDate}'";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    var rdr = await cmd.ExecuteReaderAsync();

                    while(rdr.Read())
                    {
                        var revenue= new DashboardFuctionDayRevenue
                        {
                            WeekDay = Convert.ToString(rdr["DayName"]),
                            Amount = Convert.ToDecimal(rdr["Revenue"])
                        };

                        lstRevnue.Add(revenue);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DataAccessSqlService)} {ex.Message}");
                throw;
            }

            return lstRevnue;
        }

        public Task<IEnumerable<TransactionsByMonthAndProduct>> GetMonthlyTransactionCountsAsync(DateTime startDate, DateTime endDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<DashboardFuctionDayReservation>> GetDaysReservations(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            throw new NotImplementedException();
        }
    }
}
