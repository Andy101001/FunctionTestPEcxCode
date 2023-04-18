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

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionDailyAverageOccupancy
    {
        private readonly ILogger _logger;
        private readonly ITransactionService _dailyTransactionCountService;
        public DashboardFunctionDailyAverageOccupancy(ILoggerFactory loggerFactory, ITransactionService dailyTransactionCountService)
        {
            ArgumentNullException.ThrowIfNull(dailyTransactionCountService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyTotalRevenue>();
            _dailyTransactionCountService = dailyTransactionCountService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionDailyAverageOccupancy)}");
        }

        [Function("ABM Dashboard - Get Daily Average Occupancy")]
        [OpenApiOperation(operationId: "GetDailyAverageOccupancy", tags: new[] { "ABM Dashboard" }, Summary = "Get Average Daily Occupancy", Description = "Gets the average parking occupancy for the given day, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyAverageOccupancy), Summary = "Average Daily Occupancy", Description = "The average parking occupancy for the given day, potentially filtered by facility, level and product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyParkingOccupancy), Summary = "Get Monthly Parking Occupancy", Description = "Gets the average parking occupancy and previous year's occupancy, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "dailyaverageoccupancy")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyAverageOccupancy)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
                var result = await _dailyTransactionCountService.GetDailyAverageOccupancy(filterParameters);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionDailyAverageOccupancy)}");
                return new OkObjectResult(new { averageDailyOccupancyInteger = result.AverageDailyOccupancyInteger, averageDailyOccupancyPercentage = result.AverageDailyOccupancyPercentage });
            }

            catch (ArgumentException)
            {
                _logger.LogError($"{nameof(DashboardFunctionDailyAverageOccupancy)} Missing query parameters");
                return new BadRequestObjectResult("Missing or invalid query parameters.");
            }

        }
            
    }
}
