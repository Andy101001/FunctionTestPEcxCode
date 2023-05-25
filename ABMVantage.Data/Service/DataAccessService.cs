namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.DataAccess;
    using ABMVantage.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
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
        /// Initializes a new instance of the <see cref="InsightsService"/> class.
        /// </summary>
        /// <param name="factory">The factory instance.</param>
        public DataAccessService(IDbContextFactory<CosmosDataContext> factory) => _factory = factory;

        /// <summary>
        /// Returns a specific reservation for the dashboard
        /// </summary>
        /// <param name="id">reservation id to return</param>
        /// <returns>Reservation</returns>
        public async Task<DailyAverageOccupancy>? GetDailyAverageOccupancy(FilterParam parameters)
        {

            int availableParkingSpaces = await GetAvailableParkingSpaces(parameters);


            using var context = _factory.CreateDbContext();



 
            int totalOccupiedParkingSpotHours = (from detail in context.FactOccupancyDetails
            where (parameters.Facilities.Select(x => x.Id).Contains(detail.FacilityId) || parameters.Facilities.Count() == 0)
            && (parameters.ParkingLevels.Select(x => x.Id).Contains(detail.LevelId) || parameters.ParkingLevels.Count() == 0)
            && (parameters.Products.Select(x => x.Id.ToString()).Contains(detail.ProductId) || parameters.Products.Count() == 0)
            select detail.OccupancyForHour).ToList().Sum();


            var averageOccupancyInteger = (float) totalOccupiedParkingSpotHours / ((float) availableParkingSpaces * 24) * (float) availableParkingSpaces;
            var averageOccupancycPercentage = (float) totalOccupiedParkingSpotHours / ((float) availableParkingSpaces * 24) * 100;

            return new DailyAverageOccupancy
            {
                AverageDailyOccupancyInteger = Convert.ToInt32(averageOccupancyInteger),
            AverageDailyOccupancyPercentage = Convert.ToInt32(averageOccupancycPercentage)
            };

        }

        private async Task<int> GetAvailableParkingSpaces(FilterParam parameters)
        {
            using var context = _factory.CreateDbContext();

            var totalSpaces = (from item in context.DimParkingSpaceCounts
            where (parameters.Facilities == null || parameters.Facilities.Select(x => x.Id).Contains(item.FacilityId) || parameters.Facilities.Count() == 0)
            && (parameters.ParkingLevels  == null || parameters.ParkingLevels.Select(x => x.Id).Contains(item.LevelId) || parameters.ParkingLevels.Count() == 0)
            && (parameters.Products == null || parameters.Products.Select(x => x.Id).Contains(item.ParkingProductId) || parameters.Products.Count() == 0)
            select item.ParkingSpaceCount).Sum();
            return totalSpaces;
        }
    }
}