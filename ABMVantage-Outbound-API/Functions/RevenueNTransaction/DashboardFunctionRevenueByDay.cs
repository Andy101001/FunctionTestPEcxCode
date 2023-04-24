using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using ABMVantage.Data.Service;
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

namespace ABMVantage_Outbound_API.Functions.RevenueNTransaction
{
    public class DashboardFunctionRevenueByDay
    {
        private readonly ILogger _logger;
        private readonly ITransaction_NewService _transactionService;
        public DashboardFunctionRevenueByDay(ILoggerFactory loggerFactory, ITransaction_NewService transactionService)
        {
            ArgumentNullException.ThrowIfNull(transactionService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<RevenueService>();
            _transactionService = transactionService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionRevenueByDay)}");
        }

        [Function("ABM Dashboard - GetRevenueByDay")]
        [OpenApiOperation(operationId: "GetRevenueByDay", tags: new[] { "ABM Dashboard" }, Summary = "Get Get Revenue By Day", Description = "Gets the Revenue By Day, potentially filtered by facility, level and product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionRevenueByDay), Summary = "Average Daily Occupancy", Description = "Gets the Revenue By Day, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
       
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "revenuebyday")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionRevenueByDay)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            if (string.IsNullOrEmpty(content))
            {
                _logger.LogError($"{nameof(DashboardFunctionRevenueByDay)}  parametrs  are EMPTY OR not supplied!");
                throw new ArgumentNullException("inputFilter");
            }

            var result = await _transactionService.GetRevenueByDays(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionRevenueByDay)}");

            //Just to make out json as required to UI
            return new OkObjectResult(result);
        }
    }
}
