namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using ABMVantage_Outbound_API.DataAccess;
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Models;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Data;
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

        public async Task<IEnumerable<ReservationByHour>> GetReservationByHourCountsAsync(HourlyReservationParameters hourlyReservationParameters)
        {
            _logger.LogInformation($"Getting Dashboard Hourly Reservation Count {nameof(GetReservationByHourCountsAsync)}");
            var results = new List<ReservationByHour>();
            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {
                    var conn = db.Database.GetDbConnection();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = $"BASE.ReservationCountByHour '{hourlyReservationParameters.facilityId}', '{hourlyReservationParameters.levelId}', '{hourlyReservationParameters.parkingProductId}', '{hourlyReservationParameters.calculationDate}'";
                    cmd.CommandType = CommandType.Text;
                    db.Database.OpenConnection();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reservationByHour = new ReservationByHour();
                            string time = reader["HourName"].ToString() ?? "00:00";
                            reservationByHour.ReservationTime = $"{time}:00";
                            reservationByHour.Data = new List<ReservationByHourData> 
                            { 
                                new ReservationByHourData
                                {
                                    NoOfReservations = int.Parse(reader["ReservationCount"].ToString() ?? "000" ),
                                    Product = reader["PRODUCT"].ToString() ?? "N/A"
                                }
                            };

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
        public async Task<IEnumerable<DashboardMonthlyAverageTicketValue>> GetAverageTicketValuePerYearAsync(TicketPerYearParameters ticketValuesPerYear)
        {
            _logger.LogInformation($"Getting average ticket values per year {nameof(GetAverageTicketValuePerYearAsync)}");
            //var results = new List<DashboardMonthlyAverageTicketValue>();
            try
            {
                //********************************* TODO: Need to create this stored proc *****************************************
                //using (var db = _dbSqlContextFactory.CreateDbContext())
                //{
                //    var conn = db.Database.GetDbConnection();
                //    var cmd = conn.CreateCommand();
                //    cmd.CommandText = $"BASE.AverageTicketValuePerYear '{ticketValuesPerYear.facilityId}', '{ticketValuesPerYear.levelId}', '{ticketValuesPerYear.parkingProductId}', '{ticketValuesPerYear.calculationDate}'";
                //    cmd.CommandType = CommandType.Text;
                //    db.Database.OpenConnection();

                //    using (var reader = cmd.ExecuteReader())
                //    {
                //        while (reader.Read())
                //        {
                //            var transaction = new DashboardMonthlyAverageTicketValue();
                //            //transaction.ReservationTime = int.Parse(reader["YEAR"].ToString() ?? "0000");
                //            //transaction.Month = int.Parse(reader["MONTH"].ToString() ?? "0000");
                //            //transaction.TransactionCount = Convert.ToInt32(reader["TRANSACTION_COUNT"]);
                //            //transaction.ParkingProduct = reader["PRODUCT_NAME"].ToString();
                //            results.Add(transaction);
                //        }
                //    }
                //    return results;
                //}
                //********************************* TODO: Need to create this stored proc *****************************************

                List<DashboardMonthlyAverageTicketValue> ticket = new List<DashboardMonthlyAverageTicketValue>
                {

                    new DashboardMonthlyAverageTicketValue
                    {
                        Month = "May",
                        MonthlyAverageTicketValue = new List<AverageTicketValueForMonth>
                            {
                                new AverageTicketValueForMonth {Product = "EV",
                                    AverageTicketValue = 65 },
                                new AverageTicketValueForMonth {Product = "Valet",
                                    AverageTicketValue = 11 },
                                new AverageTicketValueForMonth {Product = "Premium",
                                    AverageTicketValue = 22 },
                                new AverageTicketValueForMonth {Product = "General",
                                    AverageTicketValue = 33 },
                                new AverageTicketValueForMonth {Product = "Monthly",
                                    AverageTicketValue = 44 }
                            }
                        },
                    new DashboardMonthlyAverageTicketValue
                    {
                        Month = "June",
                        MonthlyAverageTicketValue = new List<AverageTicketValueForMonth>
                            {
                                new AverageTicketValueForMonth {Product = "EV",
                                    AverageTicketValue = 1 },
                                new AverageTicketValueForMonth {Product = "Valet",
                                    AverageTicketValue = 3 },
                                new AverageTicketValueForMonth {Product = "Premium",
                                    AverageTicketValue = 6 },
                                new AverageTicketValueForMonth {Product = "General",
                                    AverageTicketValue = 7 },
                                new AverageTicketValueForMonth {Product = "Monthly",
                                    AverageTicketValue = 8 }
                            }
                    }
                };

                return ticket;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Getting average ticket values per year  {nameof(GetReservationByHourCountsAsync)} failed: {ex.Message}");
                throw ex;
            }
        }


        public async Task<IEnumerable<TransactionsByMonthAndProduct>> GetMonthlyTransactionCountsAsync(DateTime startDate, DateTime endDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            var results = new List<TransactionsByMonthAndProduct>();
            using (var db = _dbSqlContextFactory.CreateDbContext())
            {
                var conn = db.Database.GetDbConnection();
                var cmd = conn.CreateCommand();
                cmd.CommandText = $"BASE.TransactionsByMonthAndProduct '{facilityId}', '{levelId}', '{parkingProductId}', '{startDate}', '{endDate}'";
                cmd.CommandType = CommandType.Text;
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


        public async Task<DashboardDailyAverageOccupancy> GetDailyAverageOccupancy(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {

            int dailyCount = 0;
            var occupancy = new DashboardDailyAverageOccupancy();
            try
            {
                using (var db = _dbSqlContextFactory.CreateDbContext())
                {
                    string endDate = "2022-12-09 23:59:59.000";
                    string startDate = "2022-07-08 05:00:00.000";

                    var conn = new SqlConnection(db.Database.GetConnectionString());
                    conn.Open();

                    //string sql = $"EXEC DailyTotalRevenue '{parkingProductId}','{facilityId}','{startDate}','{endDate}','{levelId}'";

                    string sql = "EXEC BASE.DailyAverageOccupancy '2545','LAX3576BLDG01','2022-07-08 05:00:00.000','2022-12-09 23:59:59.000','AGPK01_05'";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    //var rdr = await cmd.ExecuteScalarAsync();
                    var rdr = await cmd.ExecuteReaderAsync();
                    while (rdr.Read())
                    {
                        occupancy.AverageDailyOccupancyInteger = Convert.ToInt32(rdr["averageOccupancy"]);
                        occupancy.AverageDailyOccupancyPercentage = Convert.ToInt32(rdr["averageOccupancyPercentage"]);
                    }
        //    using (var db = _dbSqlContextFactory.CreateDbContext())
        //    {
        //        //var dbSet = from pas in db.FactPaymentsTicketAndStageds
        //        //            join ft in db.FactTickets on pas.TicketId equals ft.TicketId
        //        //            join f in db.DimFacilities on ft.FacilityId equals f.FacilityId
        //        //            ;

                    //dailyCount = Convert.ToInt32(rdr);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }


            return occupancy;
        }
        //    }


        public async Task<int> GetDailyTransactionCountAsync(DateTime? transactionDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            int dailyCount = 0;
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

                    //string sql = $"EXEC DailyTotalRevenue '{parkingProductId}','{facilityId}','{startDate}','{endDate}','{levelId}'";
                    string sql = $"EXEC BASE.DailyAverageOccupancy '{parkingProductId}','{facilityId}','{startDate}','{endDate}','{levelId}'";

                    //string sql = "EXEC BASE.DailyTransaction '2545','LAX3576BLDG01','2022-07-08 05:00:00.000','2022-12-09 23:59:59.000','AGPK01_05'";

                    SqlCommand cmd = new SqlCommand(sql, conn);

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

        public async Task<IList<DashboardFuctionDayRevenue>> GetRevnueByDay(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {

            var lstRevnue = new List<DashboardFuctionDayRevenue>();

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
                        var revenue = new DashboardFuctionDayRevenue
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



        public async Task<IList<DashboardFuctionDayReservation>> GetDaysReservations(DateTime? calculationDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            int dailyCount = 0;

            var lstDaysRervation = new List<DashboardFuctionDayReservation>();

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

                lstDaysRervation.Add(new DashboardFuctionDayReservation { NoOfReservations = 100, WeekDay = "Mon" });
                lstDaysRervation.Add(new DashboardFuctionDayReservation { NoOfReservations = 100, WeekDay = "Tue" });
                lstDaysRervation.Add(new DashboardFuctionDayReservation { NoOfReservations = 100, WeekDay = "Wed" });
                lstDaysRervation.Add(new DashboardFuctionDayReservation { NoOfReservations = 100, WeekDay = "Thu" });
                lstDaysRervation.Add(new DashboardFuctionDayReservation { NoOfReservations = 100, WeekDay = "Fri" });
                lstDaysRervation.Add(new DashboardFuctionDayReservation { NoOfReservations = 100, WeekDay = "Sat" });
                lstDaysRervation.Add(new DashboardFuctionDayReservation { NoOfReservations = 100, WeekDay = "Sun" });

                return lstDaysRervation;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public Task<IEnumerable<OccupancyByMonth>> GetMonthlyParkingOccupanciesAsync(DateTime startDate, DateTime endDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            throw new NotImplementedException();
        }
    }
}