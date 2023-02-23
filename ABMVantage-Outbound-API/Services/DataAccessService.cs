﻿namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.DataAccess;
    using ABMVantage_Outbound_API.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    /// <summary>
    /// EF service for database reads and writes
    /// </summary>
    public class DataAccessService : IDataAccessService
    {
        /// <summary>
        /// Factory to generate <see cref="DocsContext"/> instances.
        /// </summary>
        private readonly IDbContextFactory<CosmosDataContext> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="factory">The factory instance.</param>
        public DataAccessService(IDbContextFactory<CosmosDataContext> factory) =>_factory = factory;

        /// <summary>
        /// Get active charging sessions
        /// </summary>
        /// <param name="id">optional id for a session</param>
        /// <returns>List<EvActiveSessions></returns>        
        public async Task<List<EvActiveSessions>>? GetActiveChargingSessionsAsync(string? id = null)
        {
            using var context = _factory.CreateDbContext();

            var evActiveSessions = await context.EvActiveSessions.ToListAsync();

            return evActiveSessions;
        }

        /// <summary>
        /// Get closed charging sessions
        /// </summary>
        /// <param name="id">Optional Id for Closed Sessions</param>
        /// <returns>List<EvClosedSessions></returns>        
        public async Task<List<EvClosedSessions>>? GetClosedChargingSessionsAsync(string? id = null)
        {
            using var context = _factory.CreateDbContext();

            var evClosedSessions = await context.EvClosedSessions.ToListAsync();

            return evClosedSessions; 
        }

        /// <summary>
        /// Returns a specific reservation for the dashboard
        /// </summary>
        /// <param name="id">reservation id to return</param>
        /// <returns>Reservation</returns>
        public async Task<Reservation> GetReservationsAsync(string id)
        {
            using var context = _factory.CreateDbContext();

            var reservation = await context.Reservations
                            .WithPartitionKey(id)
                            .SingleOrDefaultAsync(d => d.Id == id);

            return reservation;
        }
    }
}