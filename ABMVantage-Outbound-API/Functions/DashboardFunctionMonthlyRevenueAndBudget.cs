using ABMVantage_Outbound_API.DashboardFunctionModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using ABMVantage_Outbound_API.Services;
using ABMVantage.Data.Models;
using Newtonsoft.Json;

namespace ABMVantage_Outbound_API.Functions
{
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
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyAverageTicketValue), Summary = "Get Monthly Average Ticket Value", Description = "Gets the monthly average ticket value, potentially filtered by facility, level and product.")]
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
            catch (ArgumentException)
            {
                return new BadRequestResult();
            }
        }
    }
}
