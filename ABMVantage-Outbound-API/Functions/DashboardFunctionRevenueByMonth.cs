using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionRevenueByMonth
    {
        private readonly ILogger _logger;
        private readonly IRevenueService _revenueService;
        public DashboardFunctionRevenueByMonth(ILoggerFactory loggerFactory, IRevenueService revenueService)
        {
            ArgumentNullException.ThrowIfNull(revenueService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<RevenueService>();
            _revenueService = revenueService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionRevenueByMonth)}");
        }

        [Function("ABM Dashboard - GetRevenueByMonth")]
        [OpenApiOperation(operationId: "GetRevenueByMonth", tags: new[] { "ABM Dashboard" }, Summary = "Get Get Revenue By Month", Description = "Gets the Revenue By Month, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionRevenueByMonth), Summary = " Revenue By Month", Description = "Gets the Revenue By Month, potentially filtered by startDate, endDate, facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        [OpenApiParameter(name: "startDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The date used for the revenue calculation.", Description = "The start date for the requested revenue. For example, if the calculation date is 2021-11-01\"")]
        [OpenApiParameter(name: "endDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The date used for the revenue calculation.", Description = "The end date for the requested revenue. For example, if the calculation date is 2021-11-01")]
        [OpenApiParameter(name: "facilityId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional facilityId for filtering.", Description = "When a facilityId is provided, only that facility is included in the revenue calculations. If the facilityId is \"all\" or \"ALL\", ")]
        [OpenApiParameter(name: "levelId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional levelId for filtering", Description = "When a levelId is provided, then the average occupancy is filtered by level. If the levelId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by level.")]
        [OpenApiParameter(name: "parkingProductId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional parkingProductId for filtering", Description = "When a parkingProductId is provided, then the revenue is calculated using only parking spots for the given product. If the parkingProductId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by parking product.")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "getrevenuebymonth")] HttpRequestData reg, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string? facilityId, [FromQuery] string? levelId, [FromQuery] string? parkingProductId)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionRevenueByMonth)}");

            if (string.IsNullOrEmpty(parkingProductId))
            {
                _logger.LogError($"{nameof(DashboardFunctionRevenueByMonth)} Query string  parametr customerId is EMPTY OR not supplied!");
                throw new ArgumentNullException("parkingProductId");
            }

            var result = await _revenueService.GetRevnueByMonth(startDate,endDate, facilityId, levelId, parkingProductId);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionRevenueByMonth)}");

            //Just to make out json as required to UI
            return new OkObjectResult(result);
        }
    }
}
