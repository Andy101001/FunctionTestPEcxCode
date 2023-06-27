﻿namespace ABMVantage_Outbound_API.Functions.ReservationsNTickets
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

    public class DashboardFunctionHourlyReservations
    {
        private readonly ILogger _logger;
        private readonly IReservationNTicketService _reservationService;
        private readonly IReservationAndTicketService _reservationAndTicketService;

        public DashboardFunctionHourlyReservations(ILoggerFactory loggerFactory, IReservationNTicketService reservationService, IReservationAndTicketService reservationAndTicketService)
        {
            ArgumentNullException.ThrowIfNull(reservationService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionHourlyReservations>();
            _reservationService = reservationService; 
            _reservationAndTicketService = reservationAndTicketService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionHourlyReservations)}");
        }

        [Function("ABM Dashboard - Get Hourly Reservations")]
        [OpenApiOperation(operationId: "GetHourlyReservations", tags: new[] { "ABM Dashboard" }, Summary = "Get Hourly Reservations", Description = "")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionHourlyReservations), Summary = "Hourly Reservations", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "hourlyreservations")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionHourlyReservations)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            //Get total occupancy revenue
            //var result = await _reservationService.GetHourlyReservations(inputFilter);
            var result = await _reservationAndTicketService.GetHourlyReservations(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionHourlyReservations)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { APIResponse = result });
        }
    }
}