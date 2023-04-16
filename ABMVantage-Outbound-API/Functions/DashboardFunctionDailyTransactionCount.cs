

using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionDailyTransactionCount
    {
        private readonly ILogger _logger;
        private readonly ITransactionService _dailyTransactionCountService;
        public DashboardFunctionDailyTransactionCount(ILoggerFactory loggerFactory, ITransactionService dailyTransactionCountService)
        {
            ArgumentNullException.ThrowIfNull(dailyTransactionCountService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyTotalRevenue>();
            _dailyTransactionCountService = dailyTransactionCountService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionDailyTransactionCount)}");
        }

        [Function("ABM Dashboard - Get Daily Transaction Count.")]
        [OpenApiOperation(operationId: "GetDailyTransactionCount", tags: new[] { "ABM Dashboard" }, Summary = "Get Daily Transaction Count", Description = "Get Daily Transaction Count.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyTransactionCount), Summary = "Daily Transaction Count", Description = "The number of transactions occuring on the given day, potentially filtered by facility, level, and parking product.")]
        [OpenApiParameter(name: "calculationDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The date used for the daily revenue calculation.", Description = "The calculation date for the requested transaction count. For example, if the calculation date is 2021-11-01 then the transaction count is the number of transactions occuring on 2021-11-01")]
        [OpenApiParameter(name: "facilityId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "An optional facilityId for filtering.", Description = "When a facilityId is provided, only transactions for that facility are included in the count. If the facilityId is 'all', or 'ALL' or empty or null, then the transactions are not filtered by facility.")]
        [OpenApiParameter(name: "levelId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "An optional levelId for filtering.", Description = "When a levelId is provided, only transactions on the given level are included in the count. If the levelId is 'all' or 'ALL' or empty or null, then the transactions are not filtered by level.")]
        [OpenApiParameter(name: "parkingProductId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "An optional parkingProductId for filtering.", Description = "When a parkingProductId is provided, only transactions for that parking product are included in the count. If the parkingProductId is 'all' or 'ALL' or empty or null, then the transactions are not filtered by parking product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "dailytransactioncount")] HttpRequestData req, [FromQuery] DateTime calculationDate, [FromQuery] string? facilityId, [FromQuery] string? levelId, [FromQuery] string? parkingProductId)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyTransactionCount)}");

            if (string.IsNullOrEmpty(parkingProductId))
            { 
                _logger.LogError($"{nameof(PushVantageAzureFunctionFloorDetails)} Query string  parametr customerId is EMPTY OR not supplied!");
                throw new ArgumentNullException("parkingProductId");
            }

            var result = await _dailyTransactionCountService.GetDailyTransactiontCountAsync(calculationDate,facilityId, levelId, parkingProductId);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionDailyTransactionCount)}");

            return new OkObjectResult(new { totalTransactions = result });
        }


    }
}
