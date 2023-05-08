namespace ABMVantage_Outbound_API.Functions.RevenueNTransaction
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using System.Net;

    public class DashboardFunctionRevenueByMonth
    {
        private readonly ILogger _logger;
        private readonly ITransaction_NewService _transactionService;

        public DashboardFunctionRevenueByMonth(ILoggerFactory loggerFactory, ITransaction_NewService transactionService)
        {
            ArgumentNullException.ThrowIfNull(transactionService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<RevenueService>();
            _transactionService = transactionService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionRevenueByMonth)}");
        }

        [Function("ABM Dashboard - GetRevenueByMonth")]
        [OpenApiOperation(operationId: "GetRevenueByMonth", tags: new[] { "ABM Dashboard" }, Summary = "Get Get Revenue By Month", Description = "Gets the Revenue By Month, potentially filtered by facility, level and product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionRevenueByMonth), Summary = " Revenue By Month", Description = "Gets the Revenue By Month, potentially filtered by startDate, endDate, facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "revenuebymonth")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionRevenueByMonth)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            if (string.IsNullOrEmpty(content))
            {
                _logger.LogError($"{nameof(DashboardFunctionRevenueByMonth)} inputFilter are EMPTY OR not supplied!");
                throw new ArgumentNullException("parkingProductId");
            }

            var result = await _transactionService.GetRevenueByMonths(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionRevenueByMonth)}");

            //Just to make out json as required to UI
            return new OkObjectResult(result);
        }
    }
}