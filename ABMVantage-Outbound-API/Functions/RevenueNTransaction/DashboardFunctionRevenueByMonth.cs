using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ABMVantage_Outbound_API.Functions.RevenueNTransaction
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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "revenuebymonth")] HttpRequestData reg, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string? facilityId, [FromQuery] string? levelId, [FromQuery] string? parkingProductId)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionRevenueByMonth)}");

            if (string.IsNullOrEmpty(parkingProductId))
            {
                _logger.LogError($"{nameof(DashboardFunctionRevenueByMonth)} Query string  parametr customerId is EMPTY OR not supplied!");
                throw new ArgumentNullException("parkingProductId");
            }

            var result = await _revenueService.GetRevnueByMonth(startDate, endDate, facilityId, levelId, parkingProductId);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionRevenueByMonth)}");

            //Just to make out json as required to UI
            return new OkObjectResult(result);
        }
    }
}
