using ABMVantage_Outbound_API.DashboardFunctionModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public class DayReservationService : IDayReservationService
    {
        private readonly ILogger<DayReservationService> _logger;
        private readonly IDataAccessSqlService _dataAccessSqlService;
        private readonly IConfiguration _configuration;

        public DayReservationService(ILoggerFactory loggerFactory, IDataAccessSqlService dataAccessSqlService)
        {
            _logger = loggerFactory.CreateLogger<DayReservationService>();

            _dataAccessSqlService = dataAccessSqlService;

            _logger.LogInformation($"Constructing {nameof(DayReservationService)}");
        }
        public async Task<IList<DashboardFuctionDayReservation>> GetDaysResversation(DateTime? tranactionDate, string? facilityId, string? levelId, string? parkingProductId)
        {
            var reservations = await _dataAccessSqlService.GetDaysReservations(tranactionDate, facilityId, levelId, parkingProductId);

            return reservations;
        }
    }
}
