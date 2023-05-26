namespace ABMVantage_Outbound_API.Functions.RevenueNTransaction
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using System.Net;

    public class DashboardFunctionRevenueVsBudget
    {
        private readonly ILogger _logger;
        private readonly ITransaction_NewService _transactionService;
        private readonly IRevenueAndTransactionService _revenueAndTransactionService;


        public DashboardFunctionRevenueVsBudget(ILoggerFactory loggerFactory, ITransaction_NewService transactionService, IRevenueAndTransactionService revenueAndTransactionService)
        {
            ArgumentNullException.ThrowIfNull(transactionService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionRevenueVsBudget>();
            _transactionService = transactionService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionRevenueVsBudget)}");
            _revenueAndTransactionService = revenueAndTransactionService;
        }

        [Function("ABM Dashboard - Get RevenueVsBudget")]
        [OpenApiOperation(operationId: "GetRevenueVsBudget", tags: new[] { "ABM Dashboard" }, Summary = "Get Revenue VS Budget", Description = "")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionRevenueVsBudget), Summary = "Revenue VS Budget", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "revenuevsbudget")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionRevenueVsBudget)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            //Get total occupancy revenue
            //var result = await _transactionService.GetRevenueVsBudget(inputFilter);
            var result = await _revenueAndTransactionService.GetRevenueVsBudget(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionRevenueVsBudget)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { yearlyOccupancy = result });
        }
    }
}