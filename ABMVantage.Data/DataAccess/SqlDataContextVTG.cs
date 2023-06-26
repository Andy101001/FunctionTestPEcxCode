﻿using ABMVantage.Data.EntityModels;
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
            modelBuilder.Entity<ReservationSpanningHourSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<ReservationSpanningMonthSQL>().HasKey(c => c.Id);
            
            //Occupancy and Duration
            modelBuilder.Entity<OccupancyVsDurationSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<OccupancyRevenueSQLData>().HasKey(c => c.Id);

            //Revenue and Transactions
            modelBuilder.Entity<RevenueTransactionSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<RevenueSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<RevenueRevenueVsBudgetSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<RevenuebydaySQL>().HasKey(c => c.Id);
            modelBuilder.Entity<RevenueBudgetVsActualVarianceSQL>().HasKey(c => c.Id);
            modelBuilder.Entity<RevenueAndBudgetSQL>().HasKey(c => c.Id);

            //Other
            modelBuilder.Entity<FacilityLevelProductSQL>().HasKey(c => c.Id);
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
        public DbSet<ReservationSpanningHourSQL> ReserationsSpanningHourSQLData { get; set; }
        
        public DbSet<ReservationSpanningMonthSQL> ReservationsSpanningMonthSQLData { get; set; }

        //Occupancy
        public DbSet<OccupancyRevenueSQLData> OccupancyRevenueSQLData { get; set; }
        public DbSet<OccupancyVsDurationSQL> OccupancyVsDurationSQLData { get; set; }
        
        //Revenue and Transactions
        public DbSet<RevenueTransactionSQL> RevenueTransactionSQLData { get; set; }
        //Revenue
        public DbSet<RevenueSQL> RevenueSQLData { get; set; }
        public DbSet<RevenueRevenueVsBudgetSQL> RevenueRevenueVsBudgetSQLData { get; set; }
        
        //Removing this sql table because it has bad data. Replacing this table with RevenueTransactionSQL which has the same data without a bad join.
        //public DbSet<RevenuebydaySQL> RevenuebydaySQLData { get; set; }
        public DbSet<RevenueBudgetVsActualVarianceSQL> RevenueBudgetVsActualVarianceSQLData { get; set; }

        public DbSet<RevenueAndBudgetSQL> RevenueAndBudgetSQLData { get; set; }

        public DbSet<FacilityLevelProductSQL> FacilityLevelProductSQLData { get; set; }




    }
}
