

using ABMVantage.Data.Models;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionDailyTransactionCount
    {
        private readonly ILogger _logger;
        private readonly ITransactionService _dailyTransactionCountService;
        public DashboardFunctionDailyTransactionCount(ILoggerFactory loggerFactory, ITransactionService dailyTransactionCountService)
        {
            ArgumentNullException.ThrowIfNull(dailyTransactionCountService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyTotalRevenue>();
            _dailyTransactionCountService = dailyTransactionCountService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionDailyTransactionCount)}");
        }

        [Function("ABM Dashboard - Get Daily Transaction Count.")]
        [OpenApiOperation(operationId: "GetDailyTransactionCount", tags: new[] { "ABM Dashboard" }, Summary = "Get Daily Transaction Count", Description = "Get Daily Transaction Count.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyTransactionCount), Summary = "Daily Transaction Count", Description = "The number of transactions occuring on the given day, potentially filtered by facility, level, and parking product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyParkingOccupancy), Summary = "Get Monthly Parking Occupancy", Description = "Gets the average parking occupancy and previous year's occupancy, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "dailytransactioncount")] HttpRequestData req)
        {

            try
            {

                _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyTransactionCount)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
                var result = await _dailyTransactionCountService.GetDailyTransactiontCountAsync(filterParameters);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionDailyTransactionCount)}");
                return new OkObjectResult(new { totalTransactions = result });
            }
            catch (ArgumentException)
            {
                return new BadRequestResult();
            }




            


            
        }


    }
}
