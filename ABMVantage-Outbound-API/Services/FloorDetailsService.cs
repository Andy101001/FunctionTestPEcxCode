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
        private readonly IDataAccessService _dataAccessService;
        private readonly IConfiguration _configuration;
        private readonly bool IsSqlDbConnectionOn;
        public FloorDetailsService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService, IDataAccessService dataAccessService, IConfiguration configuration) {

            _logger = loggerFactory.CreateLogger<FloorDetailsService>();
            _dataAccessService= dataAccessService;
            _configuration = configuration;
            IsSqlDbConnectionOn = Convert.ToBoolean(_configuration.GetSection("SqlSettings")["IsSqlDbConnectionOn"]);

            _dataAccessSqlService = dataAccessSqlService;

            _logger.LogInformation($"Constructing {nameof(FloorDetailsService)}");
        }
        
        public async Task<FloorDetails> GetFloorDetails(string id)
        {
            FloorDetails floor =new FloorDetails();

            if(IsSqlDbConnectionOn)
            {
                try
                {
                    floor.Facilities = await _dataAccessSqlService.GetFacilityAsync(id);
                    string facilityId = floor.Facilities.FirstOrDefault().FacilityId;
                    floor.Levels = await _dataAccessSqlService.GetLevelAsync(facilityId);
                    floor.Products = await _dataAccessSqlService.GetProductAsync(facilityId);

                }
                catch(Exception ex)
                {
                    string msg = ex.Message;
                }
                
            }
            else
            {
                //TODO: Cosmos db implementatin go here

              // var data = _dataAccessService.GetLocaitonAsync(id);


               
            }
          

            return floor;
        }

        
    }
}
