using ABMVantage.Data.Models;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionDailyTotalRevenue
    {
        private readonly ILogger _logger;
        private readonly ITransactionService _transactionService;
        public DashboardFunctionDailyTotalRevenue(ILoggerFactory loggerFactory, ITransactionService transactionService) 
        {
            ArgumentNullException.ThrowIfNull(transactionService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyTotalRevenue>();
            _transactionService = transactionService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionDailyTotalRevenue)}");
        }


        [Function("ABM Dashboard - Get Daily Total Revenue")]
        [OpenApiOperation(operationId: "GetDailyTotalRevenue", tags: new[] { "ABM Dashboard" }, Summary = "Get Daily Total Revenue", Description = "Gets the total revenue for all transactions occuring on the given day, potentially filtered by facility, level and product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyTotalRevenue), Summary = "Daily Total Revenue", Description = "The total revenue for all transactions occuring on the calculation date filtered by facility, level, and parking product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "dailytotalrevenue")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyTotalRevenue)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
                var result = await _transactionService.GetDailyTotalRevenueAsync(filterParameters);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionDailyTotalRevenue)}");
                return new OkObjectResult(new { totalRevenue = result });
            }
            catch (ArgumentException)
            {
                return new BadRequestResult();
            }

            
        }
    }
}
