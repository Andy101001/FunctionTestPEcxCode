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
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
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
            /*using var context = _factory.CreateDbContext();
            from detail in context.FactOccupancyDetails
            group detail by new { detail.FacilityId, detail.LevelId, detail.ProductId, detail.ParkingSpaceCount, detail.BeginningOfHour.Year, detail.BeginningOfHour.Month, detail.BeginningOfHour.Day } into g
            where detail.BeginningOfHour >= parameters.FromDate && detail.BeginningOfHour <= parameters.ToDate*/
            


            throw new NotImplementedException();
            /*
            select	f.FACILITY_ID
		,f.LEVEL_ID
		,f.PARKING_PRODUCT_ID
		,f.PARKING_SPACE_COUNT
		, DATEFROMPARTS(YEAR(BEGINNING_OF_HOUR), MONTH(BEGINNING_OF_HOUR), DAY(BEGINNING_OF_HOUR)) [DAY]
		, SUM(OCCUPANCY_FOR_HOUR) TOTAL_OCCUPIED_PARKING_SPOT_HOURS_FOR_DAY
          INTO STG_OCCUPANCY_AVERAGE_FOR_DAY
        from STG_FACILITY_LEVEL_PRODUCT f
        inner join STG_OCCUPANCY_DETAIL o
        on f.FACILITY_ID = o.FACILITY_ID and (f.LEVEL_ID = o.LEVEL_ID or (f.LEVEL_ID is null and o.LEVEL_ID is null)) and f.PARKING_PRODUCT_ID = o.PRODUCT_ID
        WHERE BEGINNING_OF_HOUR between @FromDate and @ToDate
        GROUP BY f.FACILITY_ID,f.LEVEL_ID,f.PARKING_PRODUCT_ID, PARKING_SPACE_COUNT,YEAR(BEGINNING_OF_HOUR), MONTH(BEGINNING_OF_HOUR), DAY(BEGINNING_OF_HOUR)            


            */
        }

        private async Task<int> GetAvailableParkingSpaces(FilterParam parameters)
        {
            try
            {
                using var context = _factory.CreateDbContext();
                var container = context.Database.GetCosmosClient().GetContainer(context.Database.GetCosmosDatabaseId(), "DimFacility");
                var facilitiesResponse = await container.ReadItemAsync<dynamic>("DimFacility", new Microsoft.Azure.Cosmos.PartitionKey("FacilityId"));
                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}