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

    public class DashboardFunctionDailyAverageOccupancy
    {
        private readonly ILogger _logger;
        private readonly IOccupancyService _occupancyService;
        private readonly IInsightsService _dashboardService;

        public DashboardFunctionDailyAverageOccupancy(ILoggerFactory loggerFactory, IOccupancyService occupancyService , IInsightsService dashboardService)
        {
            ArgumentNullException.ThrowIfNull(occupancyService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyTotalRevenue>();
            _occupancyService = occupancyService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionDailyAverageOccupancy)}");
            _dashboardService = dashboardService;
        }

        [Function("ABM Dashboard - Get Daily Average Occupancy")]
        [OpenApiOperation(operationId: "GetDailyAverageOccupancy", tags: new[] { "ABM Dashboard" }, Summary = "Get Average Daily Occupancy", Description = "Gets the average parking occupancy for the given day, potentially filtered by facility, level and product.")]
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
                //var result = await _occupancyService.GetDailyAverageOccupancy(filterParameters);
                var result = await _dashboardService.GetDailyAverageOccupancy(filterParameters);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionDailyAverageOccupancy)}");
                return new OkObjectResult(new { averageDailyOccupancyInteger = result.AverageDailyOccupancyInteger, averageDailyOccupancyPercentage = result.AverageDailyOccupancyPercentage });
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"{nameof(DashboardFunctionDailyAverageOccupancy)} Missing query parameters {ae.Message}");
                return new BadRequestObjectResult("Missing or invalid query parameters.");
            }
        }
    }
}