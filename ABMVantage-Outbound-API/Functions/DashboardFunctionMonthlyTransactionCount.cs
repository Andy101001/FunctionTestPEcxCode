namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class DashboardFunctionMonthlyTransactionCount
    {
        private readonly ILogger _logger;
        private readonly ITransactionService _transactionService;
        private readonly IInsightsService _dashboardService;

        public DashboardFunctionMonthlyTransactionCount(ILoggerFactory loggerFactory, ITransactionService transactionService, IInsightsService dashboardService)
        {
            ArgumentNullException.ThrowIfNull(nameof(loggerFactory));
            ArgumentNullException.ThrowIfNull(nameof(transactionService));
            _logger = loggerFactory.CreateLogger<DashboardFunctionMonthlyTransactionCount>();
            _transactionService = transactionService;
            _dashboardService = dashboardService;
        }

        [Function("ABM Dashboard - Get Monthly Transaction Count")]
        [OpenApiOperation(operationId: "GetMonthlyTransactionCount", tags: new[] { "ABM Dashboard" }, Summary = "Get Transaction Count", Description = "Gets the total number of transactions for each month, potentially filtered by facility, level and product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyTransactionCount), Summary = "Get Monthly Parking Occupancy", Description = "Gets the average parking occupancy and previous year's occupancy, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "monthlytransactioncount")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation($"Executing function {nameof(DashboardFunctionMonthlyTransactionCount)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
                //var result = await _transactionService.GetMonthlyTransactionCountAsync(filterParameters);
                var result = await _dashboardService.GetMonthlyTransactionCountAsync(filterParameters);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionMonthlyTransactionCount)}");
                return new OkObjectResult(result);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"{nameof(DashboardFunctionMonthlyTransactionCount)} Missing query parameters {ae.Message}");

                return new BadRequestObjectResult("Missing or invalid query parameters.");
            }
        }
    }
}