using ABMVantage_Outbound_API.DashboardFunctionModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionDailyAverageOccupancy
    {
        private readonly ILogger _logger;
        public DashboardFunctionDailyAverageOccupancy(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyAverageOccupancy>();
            _logger.LogInformation($"Constructing{nameof(DashboardFunctionDailyAverageOccupancy)}");
        }

        [Function("ABM Dashboard - Get Daily Average Occupancy")]
        [OpenApiOperation(operationId: "GetDailyAverageOccupancy", tags: new[] { "ABM Dashboard" }, Summary = "Get Average Daily Occupancy", Description = "Gets the average parking facility occupancy for the given day, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyAverageOccupancy), Summary = "Average Daily Occupancy", Description = "The average parking occupancy for the given day, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        [OpenApiParameter(name: "calculationDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The date used for the average occupancy calculation.", Description = "The calculation date for the requested average daily occupancy. For example, if the calculation date is 2021-11-01 then the average occupancy is based on the total occupied minutes and total available minutes for all parking spots for that day 2021-11-01")]
        [OpenApiParameter(name: "facilityId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional facilityId for filtering.", Description = "When a facilityId is provided, only that facility is included in the occupancy calculations. If the facilityId is \"all\" or \"ALL\", or empty, or null, then the average daily occupancy is not filtered by facility.")]
        [OpenApiParameter(name: "levelId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional levelId for filtering", Description = "When a levelId is provided, then the average occupancy is filtered by level. If the levelId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by level.")]
        [OpenApiParameter(name: "parkingProductId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional parkingProductId for filtering", Description = "When a parkingProductId is provided, then the average occupancy is calculated using only parking spots for the given product. If the parkingProductId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by parking product.")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "dailyaverageoccupancy")] HttpRequestData reg, [FromQuery] DateTime calculationDate, [FromQuery] string? facilityId, [FromQuery] string? levelId, [FromQuery] string? parkingProductId)
        {
            throw new NotImplementedException();
        }
    }
}
