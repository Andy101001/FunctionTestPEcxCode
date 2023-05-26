using ABMVantage.Data.EntityModels;
using ABMVantage.Data.EntityModels.SQL;
using Microsoft.EntityFrameworkCore;

namespace ABMVantage.Data.DataAccess
{
    public class SqlDataContextVTG: DbContext
    {
        public SqlDataContextVTG(DbContextOptions<SqlDataContextVTG> options) : base(options)
        {
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Filters Model
            modelBuilder.Entity<FilterDataSQL>().HasKey(c => c.Id);

            //Insights
            modelBuilder.Entity<InsightsAverageDialyOccupanySQL>().HasKey(c => c.Id);
            modelBuilder.Entity<InsightsTotalRevenueSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<InsightsMonthlyTransactionSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<InsightsMonthlyRevenueAndBudgetSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<InsightsMonthlyParkingOccupancySQL>().HasKey(c => c.Id);
            modelBuilder.Entity<InsightsAverageMonthlyTicketValueSQL>().HasKey(c => c.Id);

            //Reservations
            modelBuilder.Entity<ReservationSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<ReservationAvgTicketSQL>().HasKey(c => c.Id);
            
            //Occupancy and Duration
            modelBuilder.Entity<OccupancyVsDurationSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<OccupancyRevenueSQLData>().HasKey(c => c.Id);

            //Revenue and Transactions
            modelBuilder.Entity<RevenueTransactionSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<RevenueRevenueVsBudgetSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<RevenuebydaySQL>().HasKey(c => c.Id);
            modelBuilder.Entity<RevenueBudgetVsActualVarianceSQL>().HasKey(c => c.Id);
        }

        //Insights
        public DbSet<InsightsAverageDialyOccupanySQL> InsightsAverageDialyOccupanySQLData { get; set; }
        public DbSet<InsightsTotalRevenueSQL> InsightsTotalRevenueSQLData { get; set; }
        public DbSet<InsightsMonthlyTransactionSQL> InsightsMonthlyTransactionsSQLData { get; set; }
        public DbSet<InsightsMonthlyRevenueAndBudgetSQL> InsightsMonthlyRevenueAndBudgetSQLData { get; set; }
        public DbSet<InsightsMonthlyParkingOccupancySQL> InsightsMonthlyParkingOccupancySQLData { get; set; }
        public DbSet<InsightsAverageMonthlyTicketValueSQL> InsightsAverageMonthlyTicketValueSQLData { get; set; }

        //Filter Data
        public DbSet<FilterDataSQL> filterDataSQLData { get; set; }

        //Reservation and Tickets
        public DbSet<ReservationSQL> ReservationsSQLData { get; set; }
        public DbSet<ReservationAvgTicketSQL> ReservationAvgTicketSQLData { get; set; }

        //Occupancy
        public DbSet<OccupancyRevenueSQLData> OccupancyRevenueSQLData { get; set; }
        public DbSet<OccupancyVsDurationSQL> OccupancyVsDurationSQLData { get; set; }
        
        //Revenue and Transactions
        public DbSet<RevenueTransactionSQL> RevenueTransactionSQLData { get; set; }
        public DbSet<RevenueRevenueVsBudgetSQL> RevenueRevenueVsBudgetSQLData { get; set; }
        public DbSet<RevenuebydaySQL> RevenuebydaySQLData { get; set; }
        public DbSet<RevenueBudgetVsActualVarianceSQL> RevenueBudgetVsActualVarianceSQLData { get; set; }


    }
}
