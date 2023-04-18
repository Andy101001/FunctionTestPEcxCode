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
    public class DashboardFunctionRevenueByDay
    {
        private readonly ILogger _logger;
        private readonly IRevenueService _revenueService;
        public DashboardFunctionRevenueByDay(ILoggerFactory loggerFactory, IRevenueService revenueService)
        {
            ArgumentNullException.ThrowIfNull(revenueService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<RevenueService>();
            _revenueService = revenueService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionRevenueByDay)}");
        }

        [Function("ABM Dashboard - GetRevenueByDay")]
        [OpenApiOperation(operationId: "GetRevenueByDay", tags: new[] { "ABM Dashboard" }, Summary = "Get Get Revenue By Day", Description = "Gets the Revenue By Day, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionRevenueByDay), Summary = "Average Daily Occupancy", Description = "Gets the Revenue By Day, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
       
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "revenuebyday")] HttpRequestData reg, [FromQuery] DateTime calculationDate, [FromQuery] string? facilityId, [FromQuery] string? levelId, [FromQuery] string? parkingProductId)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionRevenueByDay)}");

            if (string.IsNullOrEmpty(parkingProductId))
            {
                _logger.LogError($"{nameof(DashboardFunctionRevenueByDay)} Query string  parametr customerId is EMPTY OR not supplied!");
                throw new ArgumentNullException("parkingProductId");
            }

            var result = await _revenueService.GetRevenueByDay(calculationDate, facilityId, levelId, parkingProductId);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionRevenueByDay)}");

            //Just to make out json as required to UI
            return new OkObjectResult(result);
        }
    }
}
