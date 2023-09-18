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

    public class DashboardFunctionFiltersData
    {
        private readonly ILogger _logger;
        private readonly IFilterDataService _filterDataService;
        private readonly IFilterDataService_New _filterDataServiceNew;

        public DashboardFunctionFiltersData(ILoggerFactory loggerFactory, IFilterDataService filterDataService, IFilterDataService_New filterDataServiceNew)
        {
            ArgumentNullException.ThrowIfNull(filterDataService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionFiltersData>();
            _filterDataService = filterDataService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionFiltersData)}");
            _filterDataServiceNew = filterDataServiceNew;
        }

        [Function("ABM Dashboard - Get Filters Data based on User Access")]
        [OpenApiOperation(operationId: "GetFiltersData", tags: new[] { "ABM Common" }, Summary = "Get Filters Data based on User Access", Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionFiltersData), Summary = "Get Filters Data based on User Access", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "filtersdata")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionFiltersData)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();

           // var content = "{\"userId\": \"Charan.Verma@abm.com\",\"customerId\": 1, \"BUs\": [{\"bu\": \"33852320\"},{\"bu\": \"32227642\"}]}";

            ServiceLocations inputFilter = JsonConvert.DeserializeObject<ServiceLocations>(content);

            //Get total occupancy revenue
            //var result = await _filterDataService.GetFiltersData(inputFilter);
            var result = await _filterDataServiceNew.GetFiltersData(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionFiltersData)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { response = result });
        }
    }
}