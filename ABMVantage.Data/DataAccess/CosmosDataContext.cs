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
        private const string RevenueByProductByDay = "Revenuebyday";
        private const string RevenueBudgetVsActualVariance = nameof(RevenueBudgetVsActualVariance);
        private const string RevenueStgByDay = "RevenueByDay";
        private const string RevenueByMonth = nameof(RevenueByMonth);

        //private const string RevenueTransaction = nameof(RevenueTransaction);

        

        //Dashboard Containers
        private const string InsightsAverageDialyOccupany = nameof(InsightsAverageDialyOccupany);
        private const string InsightsTotalRevenue = nameof(InsightsTotalRevenue);
        private const string RevenueTransaction = nameof(RevenueTransaction);
        private const string InsightsMonthlyRevenueAndBudget = nameof(InsightsMonthlyRevenueAndBudget);
        private const string InsightsMonthlyParkingOccupancy = nameof(InsightsMonthlyParkingOccupancy);
        private const string InsightsAverageMonthlyTicketValue = nameof(InsightsAverageMonthlyTicketValue);

        //Reservation Containers

        private const string ReservationsStgByHour = nameof(ReservationsStgByHour);
        private const string ReservationsStgByDay = nameof(ReservationsStgByDay);
        private const string ReservationsStgByMonth = nameof(ReservationsStgByMonth);
        private const string ReservationStgAvgTicketValue = nameof(ReservationStgAvgTicketValue);

        private const string Reservation = nameof(Reservation);
        private const string ReservationAvgTicket = nameof(ReservationAvgTicket);
        private const string RevenueRevenueVsBudget = nameof(RevenueRevenueVsBudget);
        





        //Occupany and Duration Containers
        private const string OccupancyRevenue = nameof(OccupancyRevenue);
        private const string OccupancyVsDuration = nameof(OccupancyVsDuration);

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

            modelBuilder.Entity<RevenueBudgetVsActualVariance>()
                   .HasNoDiscriminator()
                   .ToContainer(RevenueBudgetVsActualVariance)
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
            modelBuilder.Entity<InsightsAverageDailyOccupancy>()
               .HasNoDiscriminator()
               .ToContainer(InsightsAverageDialyOccupany)
               .HasPartitionKey(da => da.id);

            modelBuilder.Entity<InsightsTotalRevenue>()
                 .HasNoDiscriminator()
                 .ToContainer(InsightsTotalRevenue)
                 .HasPartitionKey(da => da.id);


            modelBuilder.Entity<InsightsMonthlyRevenueAndBudget>()
               .HasNoDiscriminator()
               .ToContainer(InsightsMonthlyRevenueAndBudget)
               .HasPartitionKey(da => da.id);

            modelBuilder.Entity<InsightsMonthlyParkingOccupancy>()
               .HasNoDiscriminator()
               .ToContainer(InsightsMonthlyParkingOccupancy)
               .HasPartitionKey(da => da.id);

            modelBuilder.Entity<InsightsAverageMonthlyTicketValue>()
               .HasNoDiscriminator()
               .ToContainer(InsightsAverageMonthlyTicketValue)
               .HasPartitionKey(da => da.id);

            #region Reservation

            modelBuilder.Entity<ReservationsStgByHour>()
              .HasNoDiscriminator()
              .ToContainer(ReservationsStgByHour)
              .HasPartitionKey(da => da.id);

            modelBuilder.Entity<ReservationsStgByDay>()
              .HasNoDiscriminator()
              .ToContainer(ReservationsStgByDay)
              .HasPartitionKey(da => da.id);

            modelBuilder.Entity<ReservationsStgByMonth>()
              .HasNoDiscriminator()
              .ToContainer(ReservationsStgByMonth)
              .HasPartitionKey(da => da.id);

            modelBuilder.Entity<ReservationAvgTicket>()
              .HasNoDiscriminator()
              .ToContainer(ReservationAvgTicket)
              .HasPartitionKey(da => da.id);


            modelBuilder.Entity<Reservation>()
             .HasNoDiscriminator()
             .ToContainer(Reservation)
             .HasPartitionKey(da => da.id);
            
            //Transaciton
            modelBuilder.Entity<RevenueTransaction>()
             .HasNoDiscriminator()
             .ToContainer(RevenueTransaction)
             .HasPartitionKey(da => da.FacilityId);

            modelBuilder.Entity<RevenueRevenueVsBudget>()
            .HasNoDiscriminator()
            .ToContainer(RevenueRevenueVsBudget)
            .HasPartitionKey(da => da.id);

            



            #endregion

            // Occupancy and Duration Containers
            modelBuilder.Entity<OD_TotalOccupancyRevenue> ()
               .HasNoDiscriminator()
               .ToContainer(OccupancyRevenue)
               .HasPartitionKey(da => da.id);

            modelBuilder.Entity<OD_All>()
               .HasNoDiscriminator()
               .ToContainer(OccupancyVsDuration)
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

        public DbSet<RevenueBudgetVsActualVariance> RevenueBudgetVsActualVariances { get; set; }

        public DbSet<RevenueStgByDay> RevenueStgByDays { get; set; }

        public DbSet<RevenueByMonth> RevenueByMonths { get; set; }

        public DbSet<RevenueTransaction> RevenueTransactions { get; set; }
        public DbSet<RevenueRevenueVsBudget> RevenueRevenueVsBudgets { get; set; }

        



        //Dashboard Container Data
        public DbSet<InsightsAverageDailyOccupancy> InsightsAverageDialyOccupanyData { get; set; }
        public DbSet<InsightsTotalRevenue> InsightsTotalRevenueData { get; set; }
        
        public DbSet<InsightsMonthlyRevenueAndBudget> InsightsMonthlyRevenueAndBudgetData { get; set; }
        public DbSet<InsightsMonthlyParkingOccupancy> InsightsMonthlyParkingOccupancyData { get; set; }
        public DbSet<InsightsMonthlyTransaction> InsightsMonthlyTransactionsData { get; set; }
        public DbSet<InsightsAverageMonthlyTicketValue> InsightsAverageMonthlyTicketValueData { get; set; }

        #region Reservation APIs

        public DbSet<ReservationsStgByHour> ReservationsStgByHours { get; set; }

        public DbSet<ReservationsStgByDay> ReservationsStgByDays { get; set; }
        public DbSet<ReservationsStgByMonth> ReservationsStgByMonths { get; set; }
        //public DbSet<ReservationAvgTicket> ReservationStgAvgTicketValues { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationAvgTicket> ReservationAvgTickets { get; set; }
        




        #endregion


        //Occupany and Duration Container Data
        public DbSet<OD_TotalOccupancyRevenue> OD_TotalOccupancyRevenueData { get; set; }
        public DbSet<OD_All> OD_AllData { get; set; }
    }
}
