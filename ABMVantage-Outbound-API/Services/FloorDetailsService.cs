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
        private readonly IDataAccessService _dataAccessService;
        public FloorDetailsService(ILoggerFactory loggerFactory, IDataAccessService dataAccessService) {

            _logger = loggerFactory.CreateLogger<FloorDetailsService>();
            _dataAccessService = dataAccessService;

            _logger.LogInformation($"Constructing {nameof(FloorDetailsService)}");
        }
        
        public async Task<List<Facility>> GetAllFacilityAsync(string id)

        {
            throw new NotImplementedException();
        }

        public async Task<List<Level>> GetAllLevelAsync(string id)
        {
            var lstlevels = await _dataAccessService.GetLevelAsync(id);
            return lstlevels;
        }

        public async Task<List<Product>> GetAllProductAsync(string id)
        {
            var products= await _dataAccessService.GetProductAsync(id);

            return products;
        }

        public async Task<FloorDetails> GetFloorDetails(string id)
        {
            var products = await _dataAccessService.GetProductAsync(id);
            var lstlevels = await _dataAccessService.GetLevelAsync(id);
            var lstFacility = await _dataAccessService.GetFacilityAsync(id);
            var floor=new FloorDetails { Facilities = lstFacility, Levels= lstlevels, Products= products };

            return floor;
        }

        
    }
}
