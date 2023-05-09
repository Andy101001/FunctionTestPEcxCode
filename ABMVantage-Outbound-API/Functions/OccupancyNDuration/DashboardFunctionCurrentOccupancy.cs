namespace ABMVantage_Outbound_API.Functions.OccupancyNDuration
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using ABMVantage.Data.Service;
    using ABMVantage_Outbound_API.DashboardFunctionModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using System.Net;

    public class DashboardFunctionCurrentOccupancy
    {
        private readonly ILogger _logger;
        private readonly IOccupancyService _occupancyService;

        public DashboardFunctionCurrentOccupancy(ILoggerFactory loggerFactory, IOccupancyService occupancyService)
        {
            ArgumentNullException.ThrowIfNull(occupancyService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionCurrentOccupancy>();
            _occupancyService = occupancyService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionCurrentOccupancy)}");
        }

        [Function("ABM Dashboard - Get Current Occupancy")]
        [OpenApiOperation(operationId: "GetCurrentOccupancy", tags: new[] { "ABM Dashboard" }, Summary = "Get Current Occupancy", Description = "")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionCurrentOccupancy), Summary = "Current Occupancy", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "currentoccupancy")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionCurrentOccupancy)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            //Get total occupancy revenue
            var result = await _occupancyService.GetOccCurrent(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionCurrentOccupancy)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { response = result });
        }
    }
}
