using ABMVantage.Data.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace ABMVantage.Data.DataAccess
{
    using ABMVantage.Data.EntityModels;
    using ABMVantage.Data.Models;
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

        //Dashboard Containers
        private const string Dashboard_AverageDialyOccupany = nameof(Dashboard_AverageDialyOccupany);
        private const string Dashboard_TotalRevenue = nameof(Dashboard_TotalRevenue);
        private const string Dashboard_TotalTransactions = nameof(Dashboard_TotalTransactions);
        private const string Dashboard_HourlyReservation = nameof(Dashboard_HourlyReservation);
        private const string Dashboard_MonthlyRevenueAndBudget = nameof(Dashboard_MonthlyRevenueAndBudget);
        private const string Dashboard_MonthlyParkingOccupancy = nameof(Dashboard_MonthlyParkingOccupancy);
        private const string Dashboard_MonthlyTransaction = nameof(Dashboard_MonthlyTransaction);
        private const string Dashboard_AverageMonthlyTicketValue = nameof(Dashboard_AverageMonthlyTicketValue);

        //Occupany and Duration Containers
        private const string OD_TotalOccupancyRevenue = nameof(OD_TotalOccupancyRevenue);
        private const string OD_All = nameof(OD_All);

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

            //Dashboard Containers
            modelBuilder.Entity<Dashboard_AverageDailyOccupancy>()
               .HasNoDiscriminator()
               .ToContainer(Dashboard_AverageDialyOccupany)
               .HasPartitionKey(da => da.id);

            modelBuilder.Entity<Dashboard_TotalRevenue>()
                 .HasNoDiscriminator()
                 .ToContainer(Dashboard_TotalRevenue)
                 .HasPartitionKey(da => da.id);

            modelBuilder.Entity<Dashboard_TotalTransactions>()
                .HasNoDiscriminator()
                .ToContainer(Dashboard_TotalTransactions)
                .HasPartitionKey(da => da.id);

            modelBuilder.Entity<Dashboard_HourlyReservation>()
                .HasNoDiscriminator()
                .ToContainer(Dashboard_HourlyReservation)
                .HasPartitionKey(da => da.id);

            modelBuilder.Entity<Dashboard_MonthlyRevenueAndBudget>()
               .HasNoDiscriminator()
               .ToContainer(Dashboard_MonthlyRevenueAndBudget)
               .HasPartitionKey(da => da.id);

            modelBuilder.Entity<Dashboard_MonthlyParkingOccupancy>()
               .HasNoDiscriminator()
               .ToContainer(Dashboard_MonthlyParkingOccupancy)
               .HasPartitionKey(da => da.id);

            modelBuilder.Entity<Dashboard_MonthlyTransaction>()
               .HasNoDiscriminator()
               .ToContainer(Dashboard_MonthlyTransaction)
               .HasPartitionKey(da => da.id);

            modelBuilder.Entity<Dashboard_AverageMonthlyTicketValue>()
               .HasNoDiscriminator()
               .ToContainer(Dashboard_AverageMonthlyTicketValue)
               .HasPartitionKey(da => da.id);

            // Occupancy and Duration Containers
            modelBuilder.Entity<OD_TotalOccupancyRevenue> ()
               .HasNoDiscriminator()
               .ToContainer(OD_TotalOccupancyRevenue)
               .HasPartitionKey(da => da.id);

            modelBuilder.Entity<OD_All>()
               .HasNoDiscriminator()
               .ToContainer(OD_All)
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

        //Dashboard Container Data
        public DbSet<Dashboard_AverageDailyOccupancy> Dashboard_AverageDialyOccupanyData { get; set; }
        public DbSet<Dashboard_TotalRevenue> Dashboard_TotalRevenueData { get; set; }
        public DbSet<Dashboard_TotalTransactions> Dashboard_TotalTransactionsData { get; set; }
        public DbSet<Dashboard_HourlyReservation> Dashboard_HourlyReservationsData { get; set; }
        public DbSet<Dashboard_MonthlyRevenueAndBudget> Dashboard_MonthlyRevenueAndBudgetData { get; set; }
        public DbSet<Dashboard_MonthlyParkingOccupancy> Dashboard_MonthlyParkingOccupancyData { get; set; }
        public DbSet<Dashboard_MonthlyTransaction> Dashboard_MonthlyTransactionsData { get; set; }
        public DbSet<Dashboard_AverageMonthlyTicketValue> Dashboard_AverageMonthlyTicketValueData { get; set; }

        //Occupany and Duration Container Data
        public DbSet<OD_TotalOccupancyRevenue> OD_TotalOccupancyRevenueData { get; set; }
        public DbSet<OD_All> OD_AllData { get; set; }
    }
}
