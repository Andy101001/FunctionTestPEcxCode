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

    public class DashboardFunctionMonthlyAverageTicketValue
    {
        private readonly ILogger _logger;
        private readonly ITicketService _ticketService;
        private readonly IDashboardService _dashboardService;

        public DashboardFunctionMonthlyAverageTicketValue(ILoggerFactory loggerFactory, ITicketService ticketService, IDashboardService dashboardService)
        {
            ArgumentNullException.ThrowIfNull(ticketService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<DashboardFunctionMonthlyAverageTicketValue>();
            _ticketService = ticketService;
            _dashboardService = dashboardService;
        }

        [Function("ABM Dashboard - Get Monthly Average Ticket Value")]
        [OpenApiOperation(operationId: "GetMonthlyAverageTicketValue", tags: new[] { "ABM Dashboard" }, Summary = "Get Monthly Average Ticket Value", Description = "Gets the monthly average ticket value, potentially filtered by facility, level and product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyAverageTicketValue), Summary = "Get Monthly Average Ticket Value", Description = "Gets the monthly average ticket value, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "monthlyaverageticketvalue")] HttpRequestData req)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyReservationCountByHour)}");
            try
            {
                //var result = await _ticketService.AverageTicketValuePerYear(filterParameters);
                var result = await _dashboardService.AverageTicketValuePerYear(filterParameters);

                return new OkObjectResult(result);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"{nameof(DashboardFunctionMonthlyAverageTicketValue)} Missing query parameters {ae.Message}");

                return new BadRequestObjectResult("Missing or invalid query parameters.");
            }
        }
    }
}