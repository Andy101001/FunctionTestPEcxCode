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

    public class DashboardFunctionMonthlyParkingOccupancy
    {
        private readonly ILogger _logger;
        private readonly IParkingOccupancyService _parkingOccupancyService;
        private readonly IDashboardService _dashboardService;

        public DashboardFunctionMonthlyParkingOccupancy(ILoggerFactory loggerFactory, IParkingOccupancyService parkingOccupancyService, IDashboardService dashboardService)
        {
            ArgumentNullException.ThrowIfNull(parkingOccupancyService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<DashboardFunctionMonthlyParkingOccupancy>();
            _parkingOccupancyService = parkingOccupancyService;
            _dashboardService = dashboardService;
        }

        [Function("ABM Dashboard - Get Monthly Parking Occupancy")]
        [OpenApiOperation(operationId: "GetMonthlyParkingOccupancy", tags: new[] { "ABM Dashboard" }, Summary = "Get Monthly Parking Occupancy", Description = "Gets the average parking occupancy and previous year's occupancy, potentially filtered by facility, level and product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyParkingOccupancy), Summary = "Get Monthly Parking Occupancy", Description = "Gets the average parking occupancy and previous year's occupancy, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "monthlyoccupancy")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation($"Executing function {nameof(DashboardFunctionMonthlyParkingOccupancy)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
                //var result = await _parkingOccupancyService.GetMonthlyParkingOccupancyAsync(filterParameters);
                var result = await _dashboardService.GetMonthlyParkingOccupancyAsync(filterParameters);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionMonthlyParkingOccupancy)}");
                return new OkObjectResult(result);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"{nameof(DashboardFunctionMonthlyParkingOccupancy)} Missing query parameters {ae.Message}");

                return new BadRequestObjectResult("Missing or invalid query parameters.");
            }
        }
    }
}