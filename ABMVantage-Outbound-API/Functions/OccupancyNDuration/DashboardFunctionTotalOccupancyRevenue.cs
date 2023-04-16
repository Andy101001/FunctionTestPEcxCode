using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Service;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionTotalOccupancyRevenue
    {
        private readonly ILogger _logger;
        private readonly IOccupancyService _occupancyService;
        public DashboardFunctionTotalOccupancyRevenue(ILoggerFactory loggerFactory, IOccupancyService occupancyService)
        {
            ArgumentNullException.ThrowIfNull(occupancyService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionTotalOccupancyRevenue>();
            _occupancyService = occupancyService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionTotalOccupancyRevenue)}");
        }

        [Function("ABM Dashboard - Get Total Occupancy Revenue")]
        [OpenApiOperation(operationId: "GetTotalOccupancyRevenue", tags: new[] { "ABM Dashboard" }, Summary = "Get Total Occupancy Revenue", Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyAverageOccupancy), Summary = "Total Occupancy Revenue", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        //[OpenApiParameter(name: "calculationDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The date used for the average occupancy calculation.", Description = "The calculation date for the requested average daily occupancy. For example, if the calculation date is 2021-11-01 then the average occupancy is based on the total occupied minutes and total available minutes for all parking spots for that day 2021-11-01")]
        //[OpenApiParameter(name: "facilityId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional facilityId for filtering.", Description = "When a facilityId is provided, only that facility is included in the occupancy calculations. If the facilityId is \"all\" or \"ALL\", or empty, or null, then the average daily occupancy is not filtered by facility.")]
        //[OpenApiParameter(name: "levelId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional levelId for filtering", Description = "When a levelId is provided, then the average occupancy is filtered by level. If the levelId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by level.")]
        //[OpenApiParameter(name: "parkingProductId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional parkingProductId for filtering", Description = "When a parkingProductId is provided, then the average occupancy is calculated using only parking spots for the given product. If the parkingProductId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by parking product.")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "totaloccupancyrevenue")] HttpRequestData reg)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionTotalOccupancyRevenue)}");
            string userId = string.Empty;
            int customerId = 0;

            //if (!string.IsNullOrEmpty(HttpContext.Request.Headers["UserId"]))
            //    userId = HttpContext.Request.Headers["UserId"].ToString();

            //if (!string.IsNullOrEmpty(HttpContext.Request.Headers["CustomerId"]))
            //    customerId = int.Parse(HttpContext.Request.Headers["CustomerId"].ToString());
            //if (string.IsNullOrEmpty(parkingProductId))
            //{
            //    _logger.LogError($"{nameof(DashboardFunctionTotalOccupancyRevenue)} Query string  parametr customerId is EMPTY OR not supplied!");
            //    throw new ArgumentNullException("parkingProductId");
            //}

            var result = await _occupancyService.GetTotalOccRevenue("", 0);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionTotalOccupancyRevenue)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new{ totalOccupancyRevenue=result });
        }
    }
}
