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

namespace ABMVantage_Outbound_API.Functions.ReservationsNTickets
{
    public class DashboardFunctionMonthlyReservations
    {
        private readonly ILogger _logger;
        private readonly IReservationNTicketService _reservationService;

        public DashboardFunctionMonthlyReservations(ILoggerFactory loggerFactory, IReservationNTicketService reservationService)
        {
            ArgumentNullException.ThrowIfNull(reservationService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionMonthlyReservations>();
            _reservationService = reservationService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionMonthlyReservations)}");
        }

        [Function("ABM Dashboard - Get Monthly Reservations")]
        [OpenApiOperation(operationId: "GetMonthlyReservations", tags: new[] { "ABM Dashboard" }, Summary = "Get Monthly Reservations", Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionMonthlyReservations), Summary = "Monthly Reservations", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "monthlyreservations")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionMonthlyReservations)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            //Get total occupancy revenue
            var result = await _reservationService.GetMonthlyReservations(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionMonthlyReservations)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { response = result });
        }
    }
}
