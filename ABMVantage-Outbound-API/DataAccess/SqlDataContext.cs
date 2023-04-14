using ABMVantage_Outbound_API.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.DataAccess
{
    public class SqlDataContext: DbContext
    {
        public SqlDataContext(DbContextOptions<SqlDataContext> options) : base(options)
        {
        }
        #region TODO
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Level>();
            //modelBuilder.Entity<Product>();
           
            modelBuilder.HasDefaultSchema("BASE");
            modelBuilder.Entity<DimCustomer>().HasKey(c => c.CustomerBuKey);
            modelBuilder.Entity<DimFacility>().HasKey(c=>c.FacilityId);
            modelBuilder.Entity<DimLocation>().HasKey(c => c.LocationId);
            modelBuilder.Entity<DimLevel>().HasKey(c => c.LevelId);
            modelBuilder.Entity<DimParkingSpace>().HasKey(c => c.ParkingSpaceId);
            modelBuilder.Entity<SpaceProduct>().HasKey(c=>c.ParkingProductId);
            modelBuilder.Entity<DimProduct>().HasKey(c => c.ProductId);
            modelBuilder.Entity<FactTicket>().HasKey(c => c.TicketId);
            modelBuilder.Entity<FactPaymentsTicketAndStaged>().HasKey(c => c.PaymentId);
            modelBuilder.Entity<TransactionsByMonthAndProduct>().HasNoKey();


            modelBuilder.Entity<FactOccupancyEvent>().HasKey(c => c.OccupancyId);
            modelBuilder.Entity<FactTicket>().HasKey(c => c.TicketId);
            modelBuilder.Entity<FactPaymentsTicketAndStaged>().HasKey(c => c.PaymentId);

        }
        #endregion




        /// <summary>
        ///  Get the Customers 
        /// </summary>
        public DbSet<DimCustomer> DimCustomers { get; set; }

        /// <summary>
        ///  Get the DimLocations 
        /// </summary>
        public DbSet<DimLocation> DimLocations { get; set; }

        /// <summary>
        ///  Get the DimLevels 
        /// </summary>
        public DbSet<DimLevel> DimLevels { get; set; }

        /// <summary>
        ///  Get the DimParkingSpaces 
        /// </summary>
        public DbSet<DimParkingSpace> DimParkingSpaces { get; set; }

        /// <summary>
        ///  Get the SpaceProducts 
        /// </summary>
        public DbSet<SpaceProduct> SpaceProducts { get; set; }

        /// <summary>
        ///  Get the SpaceProducts 
        /// </summary>
        public DbSet<DimProduct> Dimroducts { get; set; }

        /// <summary>
        ///  Get the DimFacilities 
        /// </summary>
        public DbSet<DimFacility> DimFacilities { get; set; }

        /// <summary>
        ///  Get the DimFacilities 
        /// </summary>
        public DbSet<DimProduct> DimProducts { get; set; }

        /// <summary>
        /// Gets the Transactions
        /// </summary>
        public DbSet<FactPaymentsTicketAndStaged> FactPaymentsTicketsAndStageds { get; set; }

        public DbSet<FactTicket> FactTickets { get; set; }

        /// <summary>
        /// Used by SQL stored procedure to get the Transactions by Month and Product
        /// </summary>
        public DbSet<TransactionsByMonthAndProduct> TransactionsByMonthAndProduct { get; set; }


        /// <summary>
        ///  Get the FactOccupancyEvents 
        /// </summary>
        public DbSet<FactOccupancyEvent> FactOccupancyEvents { get; set; }


    }
}
