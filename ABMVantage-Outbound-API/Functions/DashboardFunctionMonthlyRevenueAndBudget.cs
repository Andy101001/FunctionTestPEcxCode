namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage.Data.Models;
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using ABMVantage_Outbound_API.Functions.RevenueNTransaction;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using System.Net;

    public class DashboardFunctionMonthlyRevenueAndBudget
    {
        private readonly ILogger _logger;
        private readonly ITransactionService _transactionService;

        public DashboardFunctionMonthlyRevenueAndBudget(ILoggerFactory loggerFactory, ITransactionService transactionService)
        {
            ArgumentNullException.ThrowIfNull(nameof(loggerFactory));
            ArgumentNullException.ThrowIfNull(nameof(transactionService));
            _logger = loggerFactory.CreateLogger<DashboardFunctionMonthlyRevenueAndBudget>();
            _transactionService = transactionService;
        }

        [Function("ABM Dashboard - Get Monthly Revenue and Budget")]
        [OpenApiOperation(operationId: "GetMonthlyRevenueAndBudget", tags: new[] { "ABM Dashboard" }, Summary = "Get Monthly Revenue and Budgeted revenue", Description = "Gets the total revenue for each month and the budgeted for total revenue for each month, potentially filtered by facility, level and product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyRevenueAndBudget), Summary = "Get Monthly Revenue and Budgeted revenue", Description = "Gets the total revenue for each month and the budgeted for total revenue for each month, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "monthlyrevenueandbudget")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation($"Executing function {nameof(DashboardFunctionRevenueByDay)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
                var result = await _transactionService.GetMonthlyRevenueAndBudget(filterParameters);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionRevenueByDay)}");

                //Just to make out json as required to UI
                return new OkObjectResult(result);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"{nameof(DashboardFunctionMonthlyRevenueAndBudget)} Missing query parameters {ae.Message}");

                return new BadRequestObjectResult("Missing or invalid query parameters.");
            }
        }
    }
}