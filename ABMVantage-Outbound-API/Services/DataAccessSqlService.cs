namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage.Data.Models;
    using ABMVantage.Data.Utils;
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using ABMVantage_Outbound_API.DataAccess;
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Models;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Data;
    using System.Data.Common;

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
            var lstFacility = new List<DimFacility>();

            using (var db = _dbSqlContextFactory.CreateDbContext())
            {
                var dbSet = from c in db.DimCustomers
                            join l in db.DimLocations on c.BuCode equals l.BuCode
                            join f in db.DimFacilities on l.LocationId equals f.LocationId
                            where c.CustomerId == id
                            select new DimFacility { LocationId = l.LocationId, FacilityId = f.FacilityId, FacilityName = f.FacilityName };

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
                var dbSet = from dsp in db.DimParkingSpaces
                            join sp in db.SpaceProducts on dsp.ParkingSpaceId equals sp.ParkingSpaceId
                            join p in db.DimProducts on sp.ParkingProductId equals p.ProductId
                            select new DimProduct { ProductId = p.ProductId, ProductName = p.ProductName };

                lstLevel = await dbSet.ToListAsync();
            }

            return lstLevel;
        }

        public async Task<IEnumerable<ReservationsForProductAndHour>> GetReservationByHourCountsAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters)
        {
            _logger.LogInformation($"Getting Dashboard Hourly Reservation Count {nameof(GetReservationByHourCountsAsync)}");
            var results = new List<ReservationsForProductAndHour>();
            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {
                    var conn = db.Database.GetDbConnection();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = StoredProcs.GetReservationsByHour;
                    cmd.CommandType = CommandType.StoredProcedure;
                    AddDefaultQUeryParametersToCommand(queryParameters, cmd);
                    db.Database.OpenConnection();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reservationByHour = new ReservationsForProductAndHour();
                            //reservationByHour.Year = Convert.ToInt32(reader["YEAR"]);
                            //reservationByHour.Month = Convert.ToInt32(reader["MONTH"]);
                            //reservationByHour.Day = Convert.ToInt32(reader["DAY"]);
                            // reservationByHour.Hour = Convert.ToInt32(reader["HOUR"]);
                            reservationByHour.Hour = reader["TIME"]?.ToString() ?? string.Empty;
                            reservationByHour.Product = reader["PRODUCT_NAME"].ToString();
                            reservationByHour.ReservationCount = Convert.ToInt32(reader["RESERVATION_COUNT"]);  
                            

                            results.Add(reservationByHour);
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Getting Dashboard Hourly Reservation Count {nameof(GetReservationByHourCountsAsync)} failed: {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<MonthlyAverageTicketValue>> GetAverageTicketValuePerYearAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters)
        {
            _logger.LogInformation($"Getting average ticket values per year {nameof(GetAverageTicketValuePerYearAsync)}");
            var results = new List<MonthlyAverageTicketValue>();
            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {
                    var conn = db.Database.GetDbConnection();
                    var cmd = conn.CreateCommand();
                    AddDefaultQUeryParametersToCommand(queryParameters, cmd);
                    cmd.CommandText = StoredProcs.GetMonthlyAverageTicketValue;


                    //cmd.CommandText = $"BASE.AverageTicketValueByMonthAndProduct '{queryParameters.FromDate}', '{queryParameters.ToDate}','{queryParameters.FacilityId}', '{queryParameters.LevelId}', '{queryParameters.ParkingProductId}'";
                    cmd.CommandType = CommandType.StoredProcedure;
                    db.Database.OpenConnection();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var averageTicketValue = new MonthlyAverageTicketValue();
                            averageTicketValue.Year = int.Parse(reader["YEAR"].ToString() ?? "0000");
                            averageTicketValue.Month = int.Parse(reader["MONTH"].ToString() ?? "0000");
                            averageTicketValue.AverageTicketValue = Convert.ToDecimal(reader["AVERAGE_TICKET_VALUE"]);
                            averageTicketValue.ParkingProduct = reader["PRODUCT_NAME"].ToString();
                            results.Add(averageTicketValue);
                        }
                    }
                    return results;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Getting average ticket values per year  {nameof(GetReservationByHourCountsAsync)} failed: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<TransactionsByMonthAndProduct>> GetMonthlyTransactionCountsAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters)
        {
            var results = new List<TransactionsByMonthAndProduct>();
            using (var db = _dbSqlContextFactory.CreateDbContext())
            {
                var conn = db.Database.GetDbConnection();
                var cmd = conn.CreateCommand();
                AddDefaultQUeryParametersToCommand(queryParameters, cmd);
                cmd.CommandText = StoredProcs.GetMonthlyTransactions;
                cmd.CommandType = CommandType.StoredProcedure;
                db.Database.OpenConnection();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var transaction = new TransactionsByMonthAndProduct();
                        transaction.Year = int.Parse(reader["YEAR"].ToString() ?? "0000");
                        transaction.Month = int.Parse(reader["MONTH"].ToString() ?? "0000");
                        transaction.TransactionCount = Convert.ToInt32(reader["TRANSACTION_COUNT"]);
                        transaction.ParkingProduct = reader["PRODUCT_NAME"].ToString();
                        results.Add(transaction);
                    }
                }
                return results;
            }
        }

        public async Task<IEnumerable<OccupancyByMonth>> GetMonthlyParkingOccupanciesAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters)
        {
            using (var db = _dbSqlContextFactory.CreateDbContext())
            {
                var conn = db.Database.GetDbConnection();
                var cmd = conn.CreateCommand();
                AddDefaultQUeryParametersToCommand(queryParameters, cmd);
                cmd.CommandText = StoredProcs.GetMonthlyOccupancy;
                cmd.CommandType = CommandType.StoredProcedure;
                db.Database.OpenConnection();
                using (var reader = cmd.ExecuteReader())
                {
                    var results = new List<OccupancyByMonth>();
                    while (reader.Read())
                    {
                        var occupancy = new OccupancyByMonth();
                        occupancy.Year = int.Parse(reader["YEAR"].ToString() ?? "0");
                        occupancy.Month = int.Parse(reader["MONTH"].ToString() ?? "0");
                        occupancy.OccupancyInteger = Convert.ToInt32(reader["OCCUPANCY"]);
                        occupancy.OccupancyPercentage = Convert.ToDecimal(reader["OCCUPANCY_PERCENTAGE"]);
                        results.Add(occupancy);
                    }
                    return results;
                }
            }
        }


        public async Task<DashboardDailyAverageOccupancy> GetDailyAverageOccupancy(DashboardFunctionDefaultDataAccessQueryParameters queryParameters)
        {


            var occupancy = new DashboardDailyAverageOccupancy();
            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {

                    var conn = new SqlConnection(db.Database.GetConnectionString());
                    conn.Open();

                    

                    SqlCommand cmd = new SqlCommand(StoredProcs.GetDailyAverageOccupancy, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    AddDefaultQUeryParametersToCommand(queryParameters, cmd);
                    var rdr = await cmd.ExecuteReaderAsync();

                    while (rdr.Read())
                    {
                        occupancy.AverageDailyOccupancyInteger = Convert.ToInt32(rdr["Occupancy"]);
                        occupancy.AverageDailyOccupancyPercentage = Convert.ToInt32(rdr["Occupancy_Percentage"]);
                    }

                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }


            return occupancy;
        }


        public async Task<int> GetDailyTransactionCountAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters)
        {
            int dailyCount = 0;
            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {


                    var conn = new SqlConnection(db.Database.GetConnectionString());
                    conn.Open();

                    //string sql = $"EXEC DailyTotalRevenue '{parkingProductId}','{facilityId}','{startDate}','{endDate}','{levelId}'";
                    string storedProcName= StoredProcs.GetDailyTransactionCount;

                    SqlCommand cmd = new SqlCommand(storedProcName, conn);
                    AddDefaultQUeryParametersToCommand(queryParameters, cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var rdr = await cmd.ExecuteScalarAsync();
                    dailyCount = Convert.ToInt32(rdr);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }


            return dailyCount;
        }

        public async Task<decimal> GetDailyTotalRevenueAsync(DashboardFunctionDefaultDataAccessQueryParameters queryParameters)
        {
            decimal dailyCount = 0;

            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {

                    var conn = new SqlConnection(db.Database.GetConnectionString());
                    conn.Open();
                    string storedProcName = StoredProcs.GetDailyTotalRevenue;
                    //string sql = "EXEC BASE.DailyTransaction '2545','LAX3576BLDG01','2022-07-08 05:00:00.000','2022-12-09 23:59:59.000','AGPK01_05'";
                    SqlCommand cmd = new SqlCommand(storedProcName, conn);
                    AddDefaultQUeryParametersToCommand(queryParameters, cmd);
                    cmd.CommandType = CommandType.StoredProcedure;

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

        public async Task<IList<DashboardFunctionDayRevenue>> GetRevnueByDay(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {

            var lstRevnue = new List<DashboardFunctionDayRevenue>();

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

                    while (rdr.Read())
                    {
                        var revenue = new DashboardFunctionDayRevenue
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

        public async Task<IList<DashboardFunctionMonthRevenue>> GetRevnueByMonth(DateTime? startDate, DateTime? endDate, string? facilityId, string? levelId, string? parkingProductId)
        {

            var lstRevnue = new List<DashboardFunctionMonthRevenue>();

            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {
                    ///TOO: Synapse DB does not have properdata so hardcoding date parameters
                    ///This is to change with calculate date
                    ///
                    //string endDate = "2023-04-20 00:00:00.000";
                    //string startDate = "2023-04-10 00:00:00.000";

                    var conn = new SqlConnection(db.Database.GetConnectionString());
                    conn.Open();

                    //EXEC BASE.RevenueByMonth '2545','LAX3576BLDG01', 'AGPK01_05', '2023-04-13 00:00:00.000', '2023-04-20 00:00:00'
                    string sql = $"EXEC BASE.RevenueByMonth '{parkingProductId}','{facilityId}','{levelId}','{startDate}','{endDate}'";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    var rdr = await cmd.ExecuteReaderAsync();

                    while (rdr.Read())
                    {
                        var revenue = new DashboardFunctionMonthRevenue
                        {
                            Month = Convert.ToString(rdr["MonthName"]),
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

        public async Task<IList<DashboardFunctionDayReservation>> GetDaysReservations(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            int dailyCount = 0;

            var lstDaysRervation = new List<DashboardFunctionDayReservation>();

            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {
                    ///TOO: Synapse DB does not have properdata so hardcoding date parameters
                    ///This is to change with calculate date
                    ///
                    string endDate = "2022-12-09 23:59:59.000";
                    string startDate = "2022-07-08 05:00:00.000";

                    //var conn = new SqlConnection(db.Database.GetConnectionString());
                    //conn.Open();

                    //string sql = $"EXEC DailyTransaction '{parkingProductId}','{facilityId}','{startDate}','{endDate}','{levelId}'";

                    ////string sql = "EXEC BASE.DailyTransaction '2545','LAX3576BLDG01','2022-07-08 05:00:00.000','2022-12-09 23:59:59.000','AGPK01_05'";

                    //dailyCount = result.FirstOrDefault();
                }


                ///TODO: this will change when SP is ready.

                lstDaysRervation.Add(new DashboardFunctionDayReservation { NoOfReservations = 100, WeekDay = "Mon" });
                lstDaysRervation.Add(new DashboardFunctionDayReservation { NoOfReservations = 100, WeekDay = "Tue" });
                lstDaysRervation.Add(new DashboardFunctionDayReservation { NoOfReservations = 100, WeekDay = "Wed" });
                lstDaysRervation.Add(new DashboardFunctionDayReservation { NoOfReservations = 100, WeekDay = "Thu" });
                lstDaysRervation.Add(new DashboardFunctionDayReservation { NoOfReservations = 100, WeekDay = "Fri" });
                lstDaysRervation.Add(new DashboardFunctionDayReservation { NoOfReservations = 100, WeekDay = "Sat" });
                lstDaysRervation.Add(new DashboardFunctionDayReservation { NoOfReservations = 100, WeekDay = "Sun" });

                return lstDaysRervation;
            }
            catch(Exception ex)
            {
                throw;
            }
        }



        public async Task<IEnumerable<RevenueAndBudgetForMonth>> GetMonthlyRevenueAndBudget(DashboardFunctionDefaultDataAccessQueryParameters queryParameters)
        {
            var result = new List<RevenueAndBudgetForMonth>();
            using (var db = _dbSqlContextFactory.CreateDbContext())
            {
                var conn = db.Database.GetDbConnection();
                var cmd = conn.CreateCommand();
                AddDefaultQUeryParametersToCommand(queryParameters, cmd);
                cmd.CommandText = StoredProcs.GetMonthlyRevenueAndBudget;
                cmd.CommandType = CommandType.StoredProcedure;
                db.Database.OpenConnection();
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var revenueAndBudget = new RevenueAndBudgetForMonth();
                        revenueAndBudget.Year = int.Parse(reader["YEAR"].ToString() ?? "0");
                        revenueAndBudget.Month = int.Parse(reader["MONTH"].ToString() ?? "0");
                        revenueAndBudget.Revenue = Convert.ToInt32(reader["REVENUE"]);
                        revenueAndBudget.BudgetedRevenue = Convert.ToInt32(reader["BUDGETED_REVENUE"]);
                        result.Add(revenueAndBudget);

                    }

                }
            }

            return result;
        }

        private static void AddDefaultQUeryParametersToCommand(DashboardFunctionDefaultDataAccessQueryParameters queryParameters, DbCommand cmd)
        {
            SqlParameter[] parameters = new SqlParameter[]
             {
                        new SqlParameter("@StartDate", queryParameters.FromDate),
                        new SqlParameter("@EndDate", queryParameters.ToDate),
                        new SqlParameter("@FacilityIds", queryParameters.FacilityIdsAsCommaDelimitedString),
                        new SqlParameter("@LevelIds", queryParameters.LevelsIdsAsCommaDelimitedString),
                        new SqlParameter("@ParkingProductIds", queryParameters.ParkingProductIdsAsCommaDelimitedString)
             };
            cmd.Parameters.AddRange(parameters);
        }
    }
}