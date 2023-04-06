using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Models;
using Microsoft.Extensions.Logging;
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
        public FloorDetailsService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService) {

            _logger = loggerFactory.CreateLogger<FloorDetailsService>();
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
            
            var products = await _dataAccessSqlService.GetProductAsync(id);
            var lstlevels = await _dataAccessSqlService.GetLevelAsync(id);
            var lstFacility = await _dataAccessSqlService.GetFacilityAsync(id);
            var floor=new FloorDetails { Facilities = lstFacility, Levels= lstlevels, Products= products };

            return floor;
        }

        
    }
}
