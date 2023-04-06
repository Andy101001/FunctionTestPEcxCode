using ABMVantage_Outbound_API.DataAccess;
using ABMVantage_Outbound_API.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public class DataAccessSqlService : IDataAccessSqlService
    {

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<DataAccessSqlService> _logger;

        /// <summary>
        /// Factory to generate <see cref="SqlDataContext"/> instances.
        /// </summary>
        private readonly IDbContextFactory<SqlDataContext> _dbSqlContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessSqlService"/> class.
        /// </summary>
        /// <param name="dbSqlContextFactory"></param>
        /// <param name="loggerFactory"></param>
        public DataAccessSqlService(IDbContextFactory<SqlDataContext> dbSqlContextFactory, ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);
            ArgumentNullException.ThrowIfNull(dbSqlContextFactory);

            _logger = loggerFactory.CreateLogger<DataAccessSqlService>();
            _dbSqlContextFactory = dbSqlContextFactory;

            _logger.LogInformation($"Constructing {nameof(DataAccessSqlService)}");

        }

        public async Task<List<Product>> GetProductAsync(string id)
        {
            _logger.LogInformation($"Getting product for Id:{id}");

            using var context = _dbSqlContextFactory.CreateDbContext();

            //TODO: make this async call and filler with Id
            var lstProduct = context.Products.ToList();


            _logger.LogInformation($"Finished Getting product for Id:{id}");

            return lstProduct;
        }

        public async Task<List<Level>> GetLevelAsync(string id)
        {
            _logger.LogInformation($"Getting product for Id:{id}");

            var lstLevel = new List<Level>();

            try
            {
                using var context = _dbSqlContextFactory.CreateDbContext();

                //TODO: make this async call and filler with Id
                lstLevel = context.Levels.ToList();

                _logger.LogInformation($"Finished Getting product for Id:{id}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error fetching data from Synapse database:{ex.Message}");
            }


            return lstLevel;
        }


        public async Task<List<Facility>> GetFacilityAsync(string id)
        {

            using var context = _dbSqlContextFactory.CreateDbContext();

            //TODO: make this async call and filler with Id
            var lstFacility = context.Facilities.ToList();

            _logger.LogInformation($"Finished Getting Facility for Id:{id}");

            return await Task.FromResult<List<Facility>>(lstFacility);
        }

    }
}
