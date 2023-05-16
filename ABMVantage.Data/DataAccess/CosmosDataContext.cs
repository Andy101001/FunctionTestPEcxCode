namespace ABMVantage.Data.DataAccess
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
        private const string DimParkingSpaceCount = nameof(DimParkingSpaceCount);


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

        public DbSet<DimParkingSpaceCount> DimParkingSpaceCounts { get; set; }




    }
}