﻿namespace ABMVantage.Data.DataAccess
{
    using ABMVantage.Data.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class CosmosDataContext : DbContext
    {
        /// <summary>
        /// Name of the partition key shadow property.
        /// </summary>
        public const string Id = nameof(Id);

   
        private const string FactOccupancyDetail = nameof(FactOccupancyDetail);
        private const string StgRevenue = "Stg_revenue"; //nameof(StgRevenue);
        private const string DimParkingSpaceCount = nameof(DimParkingSpaceCount);
        private const string TransactionByHours = nameof(TransactionByHours);
        private const string TransactionByMonth = nameof(TransactionByMonth);
        private const string RevenueVsBudget = nameof(RevenueVsBudget);
        private const string RevenueByProductByDay = nameof(RevenueByProductByDay);
        private const string BudgetVsActualVariance = nameof(BudgetVsActualVariance);
        private const string RevenueStgByDay = "RevenueByDay";
        private const string RevenueByMonth = nameof(RevenueByMonth);
        

        /// <summary>
        /// Initializes a new instance of the <see cref="DocsContext"/> class.
        /// </summary>
        /// <param name="options">The configuration options.</param>
        public CosmosDataContext(DbContextOptions<CosmosDataContext> options)
            : base(options)
        {
            SavingChanges += DashboardContex_SavingChanges;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
     
            
            modelBuilder.Entity<FactOccupancyDetail>()
                    .HasNoDiscriminator()
                    .ToContainer(FactOccupancyDetail)
                    .HasPartitionKey(da => da.FactOccupancyDetailId);
            modelBuilder.Entity<DimParkingSpaceCount>()
                    .HasNoDiscriminator()
                    .ToContainer(DimParkingSpaceCount)
                    .HasPartitionKey(da => da.id);
            modelBuilder.Entity<StgRevenue>()
                   .HasNoDiscriminator()
                   .ToContainer(StgRevenue)
                   .HasPartitionKey(da => da.id);
            modelBuilder.Entity<DimParkingSpaceCount>()
                    .HasNoDiscriminator()
                    .ToContainer(DimParkingSpaceCount)
                    .HasPartitionKey(da => da.id);

            modelBuilder.Entity<TransactionByHours>()
                    .HasNoDiscriminator()
                    .ToContainer(TransactionByHours)
                    .HasPartitionKey(da => da.id);

            modelBuilder.Entity<TransactionByMonth>()
                    .HasNoDiscriminator()
                    .ToContainer(TransactionByMonth)
                    .HasPartitionKey(da => da.id);

            modelBuilder.Entity<RevenueVsBudget>()
                   .HasNoDiscriminator()
                   .ToContainer(RevenueVsBudget)
                   .HasPartitionKey(da => da.id);

            modelBuilder.Entity<RevenueByProductByDay>()
                   .HasNoDiscriminator()
                   .ToContainer(RevenueByProductByDay)
                   .HasPartitionKey(da => da.id);

            modelBuilder.Entity<BudgetVsActualVariance>()
                   .HasNoDiscriminator()
                   .ToContainer(BudgetVsActualVariance)
                   .HasPartitionKey(da => da.id);

            modelBuilder.Entity<RevenueStgByDay>()
                  .HasNoDiscriminator()
                  .ToContainer(RevenueStgByDay)
                  .HasPartitionKey(da => da.id);

            modelBuilder.Entity<RevenueByMonth>()
                 .HasNoDiscriminator()
                 .ToContainer(RevenueByMonth)
                 .HasPartitionKey(da => da.id);



        }

        /// <summary>
        /// Intercepts saving changes to store audits.
        /// </summary>
        /// <param name="sender">The sending context.</param>
        /// <param name="e">The change arguments.</param>
        private void DashboardContex_SavingChanges(
            object sender,
            SavingChangesEventArgs e)
        {
            //var entries = ChangeTracker.Entries<Reservation>()
            //    .Where(
            //        e => e.State == EntityState.Added ||
            //        e.State == EntityState.Modified)
            //    .Select(e => e.Entity)
            //    .ToList();

            //foreach (var docEntry in entries)
            //{
            //    Audits.Add(new DocumentAudit(docEntry));
            //}o
        }

        /// <summary>
        /// Unhook events on disposal.
        /// </summary>
        public override void Dispose()
        {
            SavingChanges -= DashboardContex_SavingChanges;
            base.Dispose();
        }

        /// <summary>
        /// Asynchronous disposal.
        /// </summary>
        /// <returns>The asynchronous task.</returns>
        public override ValueTask DisposeAsync()
        {
            SavingChanges -= DashboardContex_SavingChanges;
            return base.DisposeAsync();
        }

        public DbSet<FactOccupancyDetail> FactOccupancyDetails { get; set; }
        public DbSet<StgRevenue> StgRevenues { get; set; }

        public DbSet<DimParkingSpaceCount> DimParkingSpaceCounts { get; set; }

        public DbSet<TransactionByHours> TransactionByHourss { get; set; }
        public DbSet<TransactionByMonth> TransactionByMonths { get; set; }

        public DbSet<RevenueVsBudget> RevenueVsBudgets { get; set; }

        public DbSet<RevenueByProductByDay> RevenueByProductByDays { get; set; }

        public DbSet<BudgetVsActualVariance> BudgetVsActualVariances { get; set; }

        public DbSet<RevenueStgByDay> RevenueStgByDays { get; set; }

        public DbSet<RevenueByMonth> RevenueByMonths { get; set; }


    }
}