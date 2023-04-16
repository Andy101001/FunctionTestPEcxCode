using ABMVantage_Outbound_API.DashboardFunctionModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ABMVantage_Outbound_API.Services;
using System.ComponentModel.DataAnnotations;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionMonthlyTransactionCount
    {
        private readonly ILogger _logger;
        private readonly ITransactionService _transactionService;


        public DashboardFunctionMonthlyTransactionCount(ILoggerFactory loggerFactory, ITransactionService transactionService)
        {
            ArgumentNullException.ThrowIfNull(nameof(loggerFactory));
            ArgumentNullException.ThrowIfNull(nameof(transactionService));
            _logger = loggerFactory.CreateLogger<DashboardFunctionMonthlyTransactionCount>();
            _transactionService = transactionService;
        }

        [Function("ABM Dashboard - Get Monthly Transaction Count")]
        [OpenApiOperation(operationId: "GetMonthlyTransactionCount", tags: new[] { "ABM Dashboard" }, Summary = "Get Transaction Count", Description = "Gets the total number of transactions for each month, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyTransactionCount), Summary = "Get Monthly Transaction Count", Description = "Gets the total number of transactions for each month, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        [OpenApiParameter(name: "calculationDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The start date from which monthly transaction counts are returned.", Description = "The start date for which monthly transaction counts are returned.")]
        [OpenApiParameter(name: "facilityId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "An optional facilityId for filtering.", Description = "When a facilityId is provided, only transactions from that facility are included in the total. If the facilityId is \"all\" or \"ALL\", or empty, or null, then the revenue is not filtered by facility.")]
        [OpenApiParameter(name: "levelId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "An optional levelId for filtering", Description = "When a levelId is provided, then the transactions are filtered by level. If the levelId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by level.")]
        [OpenApiParameter(name: "parkingProductId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "An optional parkingProductId for filtering", Description = "When a parkingProductId is provided, then the transactions are filtered by parking product. If the parkingProductId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by parking product.")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "monthlytransactioncount")] HttpRequestData reg, [FromQuery] DateTime calculationDate, string? facilityId, [FromQuery] string? levelId, [FromQuery] string? parkingProductId)
        {
            try
            {
                _logger.LogInformation($"Executing function {nameof(DashboardFunctionMonthlyTransactionCount)}");
                var result = await _transactionService.GetMonthlyTransactionCountAsync(calculationDate, facilityId, levelId, parkingProductId);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionMonthlyTransactionCount)}");
                return new OkObjectResult(result);
            }
            catch (ArgumentException)
            {
                return new BadRequestResult();
            }
        }
    }
}
