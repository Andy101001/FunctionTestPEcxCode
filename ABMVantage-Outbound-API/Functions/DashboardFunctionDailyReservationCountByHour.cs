namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using ABMVantage.Data.Service;
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

    public class DashboardFunctionDailyReservationCountByHour
    {
        private readonly ILogger _logger;
        private readonly IReservationService _reservationService;
        private readonly IDashboardService _dashboardService;

        public DashboardFunctionDailyReservationCountByHour(ILoggerFactory loggerFactory, IReservationService reservationService, IDashboardService dashboardService)
        {
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyReservationCountByHour>();
            _reservationService = reservationService;
            _dashboardService = dashboardService;
        }

        [Function("ABM Dashboard - Get Reservation count by hour")]
        [OpenApiOperation(operationId: "GetDailyReservationCountByHour", tags: new[] { "ABM Dashboard" }, Summary = "Get Reservation count by hour", Description = "Gets the number of reservations for each hour, potentially filtered by facility, level and product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyReservationCountByHour), Summary = "Reservation count by hour", Description = "The number of reservations for each hour, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "reservationscountbyhour")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyReservationCountByHour)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
                //var result = await _reservationService.GetHourlyReservationsByProduct(filterParameters);
                var result = await _dashboardService.GetHourlyReservationsByProduct(filterParameters);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionDailyReservationCountByHour)}");
                return new OkObjectResult(result);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"{nameof(DashboardFunctionDailyReservationCountByHour)} Missing query parameters {ae.Message}");

                return new BadRequestObjectResult("Missing or invalid query parameters.");
            }
        }
    }
}