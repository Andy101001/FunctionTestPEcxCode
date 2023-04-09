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
    public class DashboardFunctionDailyTotalRevenue
    {
        private readonly ILogger _logger;
        public DashboardFunctionDailyTotalRevenue(ILoggerFactory loggerFactory) 
        {
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyTotalRevenue>();
            _logger.LogInformation($"Constrinctung{nameof(DashboardFunctionDailyTotalRevenue)}");
        }


        [Function("ABM Dashboard - Get Daily Total Revenue")]
        [OpenApiOperation(operationId: "GetDailyTotalRevenue", tags: new[] { "ABM Dashboard" }, Summary = "Get Daily Total Revenue", Description = "Gets the total revenue for all transactions occuring on the given day, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyTotalRevenue), Summary = "Get Daily Total Revenue", Description = "Gets the total revenue for all transactions occuring on the calculation date filtered by facility, level, and parking product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        [OpenApiParameter (name: "calculationDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The date used for the daily revenue calculation.", Description = "The calculation date for the requested total revenue. For example, if the calculation date is 2021-11-01 then the total revenue is the total revenue for all transactions occurring on 2021-11-01")]
        [OpenApiParameter (name: "facilityId", In  = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional facilityId for filtering.", Description = "When a facilityId is provided, only revenue from transactions for that facility are included in the total revenye. If the facilityId is \"all\" or \"ALL\", or empty, or null, then the revenue is not filtered by facility.")]
        [OpenApiParameter (name: "LevelId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional levelId for filtering", Description = "When a levelId is provided, only revenue from transctions on the given level are included in the total revenue calculation. If the levelId is \"all\" or \"ALL\", or empty, or null, then the revenue is not filtered by level.")]
        [OpenApiParameter (name: "parkingProductId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional parkingProductId for filtering", Description = "When a parkingProductId is provided, only revenue from transactions for that parking product are included in the total revenue. If the parkingProductId is \"all\" or \"ALL\", or empty, or null, then the revenue is not filtered by parking product.")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "dailytotalrevenue")] HttpRequestData reg, [FromQuery] DateTime calculationDate, [FromQuery] string? facilityId, [FromQuery] string? LevelId, [FromQuery] string? parkingProductId)
        {
            throw new NotImplementedException();
        }
    }
}
