namespace ABMVantage_Outbound_API.Functions.ReservationsNTickets
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

    public class DashboardFunctionDailyReservations
    {
        private readonly ILogger _logger;
        private readonly IReservationNTicketService _reservationService;

        public DashboardFunctionDailyReservations(ILoggerFactory loggerFactory, IReservationNTicketService reservationService)
        {
            ArgumentNullException.ThrowIfNull(reservationService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyReservations>();
            _reservationService = reservationService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionDailyReservations)}");
        }

        [Function("ABM Dashboard - Get Daily Reservations")]
        [OpenApiOperation(operationId: "GetDailyReservations", tags: new[] { "ABM Dashboard" }, Summary = "Get Daily Reservations", Description = "")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionDailyReservations), Summary = "Daily Reservations", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "dailyreservations")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyReservations)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            //Get total occupancy revenue
            var result = await _reservationService.GetDailyReservations(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionDailyReservations)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { response = result });
        }
    }
}