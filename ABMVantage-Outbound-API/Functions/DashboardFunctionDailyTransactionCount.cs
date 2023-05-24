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
    using System.Net;

    public class DashboardFunctionDailyTransactionCount
    {
        private readonly ILogger _logger;
        private readonly ITransactionService _dailyTransactionCountService;
        private readonly IInsightsService _dashboardService;

        public DashboardFunctionDailyTransactionCount(ILoggerFactory loggerFactory, ITransactionService dailyTransactionCountService, IInsightsService dashboardService)
        {
            ArgumentNullException.ThrowIfNull(dailyTransactionCountService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyTotalRevenue>();
            _dailyTransactionCountService = dailyTransactionCountService;
            _dashboardService = dashboardService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionDailyTransactionCount)}");
        }

        [Function("ABM Dashboard - Get Daily Transaction Count.")]
        [OpenApiOperation(operationId: "GetDailyTransactionCount", tags: new[] { "ABM Dashboard" }, Summary = "Get Daily Transaction Count", Description = "Get Daily Transaction Count.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyTransactionCount), Summary = "Daily Transaction Count", Description = "The number of transactions occuring on the given day, potentially filtered by facility, level, and parking product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "dailytransactioncount")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyTransactionCount)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
                //var result = await _dailyTransactionCountService.GetDailyTransactiontCountAsync(filterParameters);
                var result = await _dashboardService.GetDailyTransactiontCountAsync(filterParameters);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionDailyTransactionCount)}");
                return new OkObjectResult(new { totalTransactions = result });
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"{nameof(DashboardFunctionDailyTransactionCount)} Missing query parameters {ae.Message}");

                return new BadRequestObjectResult("Missing or invalid query parameters.");
            }
        }
    }
}