namespace ABMVantage_Outbound_API.Functions.OccupancyNDuration
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using ABMVantage.Data.Service;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using System.Net;

    public class DashboardFunctionTotalOccupancyRevenue
    {
        private readonly ILogger _logger;
        private readonly IOccupancyService _occupancyService;
        private readonly IODService _odService;

        public DashboardFunctionTotalOccupancyRevenue(ILoggerFactory loggerFactory, IOccupancyService occupancyService, IODService odService)
        {
            ArgumentNullException.ThrowIfNull(occupancyService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionTotalOccupancyRevenue>();
            _occupancyService = occupancyService;
            _odService = odService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionTotalOccupancyRevenue)}");
        }

        [Function("ABM Dashboard - Get Total Occupancy Revenue")]
        [OpenApiOperation(operationId: "GetTotalOccupancyRevenue", tags: new[] { "ABM Dashboard" }, Summary = "Get Total Occupancy Revenue", Description = "")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionTotalOccupancyRevenue), Summary = "Total Occupancy Revenue", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "totaloccupancyrevenue")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionTotalOccupancyRevenue)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            //Get total occupancy revenue
            //var result = await _occupancyService.GetTotalOccRevenue(inputFilter);
              var result = await _odService.GetTotalOccRevenue(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionTotalOccupancyRevenue)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { response = result });
        }
    }
}