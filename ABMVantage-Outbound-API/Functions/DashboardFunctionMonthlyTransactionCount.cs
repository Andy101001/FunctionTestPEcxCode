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
using ABMVantage.Data.Models;

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
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyParkingOccupancy), Summary = "Get Monthly Parking Occupancy", Description = "Gets the average parking occupancy and previous year's occupancy, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "monthlytransactioncount")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation($"Executing function {nameof(DashboardFunctionMonthlyTransactionCount)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
                var result = await _transactionService.GetMonthlyTransactionCountAsync(filterParameters);
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
