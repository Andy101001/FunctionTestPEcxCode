namespace ABMVantage_Outbound_API.DataAccess
{
    using EntityModels;
    using Microsoft.EntityFrameworkCore;

    public class CosmosDataContext : DbContext
    {
        /// <summary>
        /// Name of the partition key shadow property.
        /// </summary>
        public const string Id = nameof(Id);

        /// <summary>
        /// Name of the container for metadata.
        /// </summary>
        private const string Dashboard = nameof(Dashboard);

        /// <summary>
        /// Name of the container for metadata.
        /// </summary>
        private const string ObsReservations = nameof(ObsReservations);

        /// <summary>
        /// Initializes a new instance of the <see cref="DocsContext"/> class.
        /// </summary>
        /// <param name="options">The configuration options.</param>
        public CosmosDataContext(DbContextOptions<CosmosDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                    .HasNoDiscriminator()
                    .ToContainer(ObsReservations)
                    .HasPartitionKey(da => da.Id);
        }

        public DbSet<Booking> Booking { get; set; }
    }
}