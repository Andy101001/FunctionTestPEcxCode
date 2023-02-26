namespace ABMVantage_Outbound_API.DataAccess
{
    using EntityModels;
    using Microsoft.Azure.Cosmos;
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
        /// Name of the container for metadata.
        /// </summary>
        private const string ElectricVehicleActiveSessions = nameof(ElectricVehicleActiveSessions);

        /// <summary>
        /// Name of the container for metadata.
        /// </summary>
        private const string ElectricVehicleClosedSessions = nameof(ElectricVehicleClosedSessions);

        /// <summary>
        /// Name of the container for metadata.
        /// </summary>
        private const string Tickets = nameof(Tickets);

        /// <summary>
        /// Name of the container for metadata.
        /// </summary>
        private const string ObsReservationTransactions = nameof(ObsReservationTransactions);

        /// <summary>
        /// Name of the container for metadata.
        /// </summary>
        private const string ParcsTicketOccupancy = nameof(ParcsTicketOccupancy);
        /// <summary>
        /// Name of the container for metadata.
        /// </summary>
        private const string PgsTicketOccupancy = nameof(PgsTicketOccupancy);




        /// <summary>
        /// Initializes a new instance of the <see cref="DocsContext"/> class.
        /// </summary>
        /// <param name="options">The configuration options.</param>
        public CosmosDataContext(DbContextOptions<CosmosDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasNoDiscriminator()
                .ToContainer(Dashboard)
                .HasPartitionKey(da => da.Id);

            modelBuilder.Entity<ObsReservationTransactions>()
                .HasNoDiscriminator()
                .ToContainer(ObsReservationTransactions)
                .HasNoKey();

            modelBuilder.Entity<Booking>()
                    .HasNoDiscriminator()
                    .ToContainer(ObsReservations)
                    .HasPartitionKey(da => da.Id);
                        
            modelBuilder.Entity<EvActiveSessions>()
                    .HasNoDiscriminator()
                    .ToContainer(ElectricVehicleActiveSessions)
                    .HasPartitionKey(da => da.Id);

            modelBuilder.Entity<EvClosedSessions>()
                    .HasNoDiscriminator()
                    .ToContainer(ElectricVehicleClosedSessions)
                    .HasPartitionKey(da => da.Id);

            modelBuilder.Entity<Occupancy>()
                    .HasNoDiscriminator()
                    .ToContainer(ParcsTicketOccupancy)
                    .HasPartitionKey(da => da.Id);

            modelBuilder.Entity<PgsOccupancy>()
                    .HasNoDiscriminator()
                    .ToContainer(PgsTicketOccupancy)
                    .HasPartitionKey(da => da.Id);


            modelBuilder.Entity<Booking>().OwnsMany(b => b.BookingReservations);
            modelBuilder.Entity<EvActiveSessions>().OwnsMany(e => e.ResponseActiveChargeSession);

        }

        /// <summary>
        /// Gets or sets the Reservations collection.
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; }
        /// <summary>
        /// Gets or sets the EvActiveSessions collection.
        /// </summary>
        public DbSet<EvActiveSessions> EvActiveSessions { get; set; }
        /// <summary>
        /// Gets or sets the EvClosedSessions collection.
        /// </summary>
        public DbSet<EvClosedSessions> EvClosedSessions { get; set; }        
        /// <summary>
        /// OBS Reservations collection
        /// </summary>
        public DbSet<Booking> Booking { get; set; }
        /// <summary>
        /// OBS Reservations Transactions collection
        /// </summary>
        public DbSet<ObsReservationTransactions> ReservationTransactions { get; set; }
        /// <summary>
        /// OBS Reservations Transactions collection
        /// </summary>
        public DbSet<Occupancy> ParcsTickOccupanies { get; set; }
        /// <summary>
        /// OBS Reservations Transactions collection
        /// </summary>
        public DbSet<PgsOccupancy> PgsTickOccupanies { get; set; }


    }
}