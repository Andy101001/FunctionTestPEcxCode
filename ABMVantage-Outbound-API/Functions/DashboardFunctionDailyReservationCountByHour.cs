using System.Net;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionDailyReservationCountByHour
    {
        private readonly ILogger _logger;
        private readonly IReservationService _reservationService;

        public DashboardFunctionDailyReservationCountByHour(ILoggerFactory loggerFactory, IReservationService reservationService)
        {
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyReservationCountByHour>();
            _reservationService = reservationService;
        }

        [Function("ABM Dashboard - Get Reservation count by hour")]
        [OpenApiOperation(operationId: "GetDailyReservationCountByHour", tags: new[] { "ABM Dashboard" }, Summary = "Get Reservation count by hour", Description = "Gets the number of reservations for each hour, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyReservationCountByHour), Summary = "Reservation count by hour", Description = "The number of reservations for each hour, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        [OpenApiParameter(name: "calculationDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The date for which hourly reservation counts are returned.", Description = "The calculation date for which hourly reservation counts will be returned. For example, if the calculation date is 2021-11-01 then the reservation counts for each hour of 2021-11-01 will be returned")]
        [OpenApiParameter(name: "facilityId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional facilityId for filtering.", Description = "When a facilityId is provided, only reservations from that facility are included in the count. If the facilityId is \"all\" or \"ALL\", or empty, or null, then the reservation counts are not filtered by facility.")]
        [OpenApiParameter(name: "levelId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional levelId for filtering", Description = "When a levelId is provided, then the reservations used for the counts are filtered by level. If the levelId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by level.")]
        [OpenApiParameter(name: "parkingProductId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional parkingProductId for filtering", Description = "When a parkingProductId is provided, then the reservations used for the count are filtered by parking product. If the parkingProductId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by parking product.")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "reservationscountbyhour")] HttpRequestData reg, [FromQuery] DateTime? calculationDate, [FromQuery] string? facilityId, [FromQuery] string? levelId, [FromQuery] string? parkingProductId)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyReservationCountByHour)}");

            HourlyReservationParameters hourlyReservationParameters = new HourlyReservationParameters();

            if (calculationDate.HasValue && !string.IsNullOrEmpty(facilityId) && !string.IsNullOrEmpty(levelId) && !string.IsNullOrEmpty(parkingProductId)) 
            {
                hourlyReservationParameters = new HourlyReservationParameters
                {
                    calculationDate = calculationDate,
                    facilityId = facilityId,
                    levelId = levelId,
                    parkingProductId = parkingProductId
                };

                var result = await _reservationService.ReservationPerHour(hourlyReservationParameters).ConfigureAwait(false);
                
                return new OkObjectResult(result);
            }
            else
            {
                _logger.LogError($"Executing function {nameof(DashboardFunctionDailyReservationCountByHour)} has invalid parameters {JsonConvert.SerializeObject(hourlyReservationParameters)}");
            }

            return new OkObjectResult(null);
        }
    }
}
