using ABMVantage_Outbound_API.Configuration;
using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
   
    public class FloorDetailsService : IFloorDetailsService
    {
        private readonly ILogger<FloorDetailsService> _logger;
        private readonly IDataAccessSqlService _dataAccessSqlService;
        private readonly IConfiguration _configuration;
        private readonly bool IsSqlDbConnectionOn;
        public FloorDetailsService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService, IConfiguration configuration) {

            _logger = loggerFactory.CreateLogger<FloorDetailsService>();

            _configuration = configuration;
            IsSqlDbConnectionOn = Convert.ToBoolean(_configuration.GetSection("SqlSettings")["IsSqlDbConnectionOn"]);

            _dataAccessSqlService = dataAccessSqlService;

          


            _logger.LogInformation($"Constructing {nameof(FloorDetailsService)}");
        }
        
        public async Task<List<Facility>> GetAllFacilityAsync(string id)

        {
            throw new NotImplementedException();
        }

        public async Task<List<Level>> GetAllLevelAsync(string id)
        {
            var lstlevels = await _dataAccessSqlService.GetLevelAsync(id);
            return lstlevels;
        }

        public async Task<List<Product>> GetAllProductAsync(string id)
        {
            var products= await _dataAccessSqlService.GetProductAsync(id);

            return products;
        }

        public async Task<FloorDetails> GetFloorDetails(string id)
        {
            FloorDetails floor =new FloorDetails();

            if(IsSqlDbConnectionOn)
            {
                var products = await _dataAccessSqlService.GetProductAsync(id);
                var lstlevels = await _dataAccessSqlService.GetLevelAsync(id);
                var lstFacility = await _dataAccessSqlService.GetFacilityAsync(id);
                floor = new FloorDetails { Facilities = lstFacility, Levels = lstlevels, Products = products };
            }
            else
            {
                //TODO: Cosmos db implementatin go here
                throw new Exception($"ERROR: IsSqlDbConnectionOn is {IsSqlDbConnectionOn}");
            }
          

            return floor;
        }

        
    }
}
