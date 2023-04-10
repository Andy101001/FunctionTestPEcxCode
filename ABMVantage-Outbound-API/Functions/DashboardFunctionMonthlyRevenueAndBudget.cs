using ABMVantage_Outbound_API.DashboardFunctionModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionMonthlyRevenueAndBudget
    {
        private readonly ILogger _logger;
        public DashboardFunctionMonthlyRevenueAndBudget(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DashboardFunctionMonthlyRevenueAndBudget>();
        }

        [Function("ABM Dashboard - Get Monthly Revenue and Budget")]
        [OpenApiOperation(operationId: "GetMonthlyRevenueAndBudget", tags: new[] { "ABM Dashboard" }, Summary = "Get Monthly Revenue and Budgeted revenue", Description = "Gets the total revenue for each month and the budgeted for total revenue for each month, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyRevenueAndBudget), Summary = "Get Monthly Revenue and Budgeted revenue", Description = "Gets the total revenue for each month and the budgeted for total revenue for each month, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        [OpenApiParameter(name: "startDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The start date for which monthly revenue and budget totals are returned.", Description = "The start date for which monthly revenue and budget totals are returned.")]
        [OpenApiParameter(name: "endDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The end date for which monthly revenue and budget totals are returned.", Description = "The end date for which monthly revnue and budget totals are returned.")]
        [OpenApiParameter(name: "facilityId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional facilityId for filtering.", Description = "When a facilityId is provided, only revenue from that facility are included in the total. If the facilityId is \"all\" or \"ALL\", or empty, or null, then the revenue is not filtered by facility.")]
        [OpenApiParameter(name: "levelId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional levelId for filtering", Description = "When a levelId is provided, then the transactions used for calculating revenue are filtered by level. If the levelId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by level.")]
        [OpenApiParameter(name: "parkingProductId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "An optional parkingProductId for filtering", Description = "When a parkingProductId is provided, then the transactions used for calculating revenue are filtered by parking product. If the parkingProductId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by parking product.")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "monthlyrevenueandbudget")] HttpRequestData reg, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string? facilityId, [FromQuery] string? levelId, [FromQuery] string? parkingProductId)
        {
            throw new NotImplementedException();
        }
    }
}
