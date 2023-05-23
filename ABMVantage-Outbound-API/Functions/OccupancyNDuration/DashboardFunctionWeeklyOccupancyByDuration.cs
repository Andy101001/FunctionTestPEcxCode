namespace ABMVantage_Outbound_API.Functions.OccupancyNDuration
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

    public class DashboardFunctionWeeklyOccupancyByDuration
    {
        private readonly ILogger _logger;
        private readonly IOccupancyService _occupancyService;
        private readonly IODService _odService;

        public DashboardFunctionWeeklyOccupancyByDuration(ILoggerFactory loggerFactory, IOccupancyService occupancyService, IODService odService)
        {
            ArgumentNullException.ThrowIfNull(occupancyService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionWeeklyOccupancyByDuration>();
            _occupancyService = occupancyService;
            _odService = odService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionWeeklyOccupancyByDuration)}");
        }

        [Function("ABM Dashboard - Get Weekly Occupancy By Duration")]
        [OpenApiOperation(operationId: "GetWeeklyOccupancyByDuration", tags: new[] { "ABM Dashboard" }, Summary = "Get Weekly Occupancy By Duration", Description = "")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionWeeklyOccupancyByDuration), Summary = "Weekly Occupancy By Duration", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "weeklyoccupancybyduration")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionWeeklyOccupancyByDuration)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            //Get total occupancy revenue
            //var result = await _occupancyService.GetWeeklyOccByDuration(inputFilter);
            var result = await _odService.GetWeeklyOccByDuration(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionWeeklyOccupancyByDuration)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { response = result });
        }
    }
}