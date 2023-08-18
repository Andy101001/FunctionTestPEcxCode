namespace ABMVantage_Outbound_API.Functions.FilterData
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

    public class DashboardFunctionDataReferesh
    {
        private readonly ILogger _logger;
        private readonly IFilterDataService _filterDataService;
        private readonly IDataRefereshService _dataRefereshService;

        public DashboardFunctionDataReferesh(ILoggerFactory loggerFactory, IFilterDataService filterDataService, IDataRefereshService dataRefereshService)
        {
            ArgumentNullException.ThrowIfNull(filterDataService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionDataReferesh>();
            _filterDataService = filterDataService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionDataReferesh)}");
            _dataRefereshService = dataRefereshService;
        }

        [Function("ABM Dashboard - Get Referesh Data Date")]
        [OpenApiOperation(operationId: "GetRefereshData", tags: new[] { "ABM Common" }, Summary = "Get Referesh Data Date", Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionDataReferesh), Summary = "Get Referesh Data Date", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "datareferesh")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionDataReferesh)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            ServiceLocations inputFilter = JsonConvert.DeserializeObject<ServiceLocations>(content);

            //Get total occupancy revenue
            //var result = await _filterDataService.GetFiltersData(inputFilter);
            var result = await _dataRefereshService.GetDataRefereshDetails();
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionDataReferesh)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { response = result });
        }
    }
}