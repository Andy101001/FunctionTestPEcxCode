namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
    using Microsoft.Extensions.Logging;
    using System.Net;

    /// <summary>
    /// OBS Reservations
    /// </summary>
    public class PushVantageAzureFunctionReservations
    {
        private readonly ILogger _logger;
        private readonly IObsReservationService _obsReservationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushVantageAzureFunctionReservations"/> class.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="obsReservationService"></param>
        public PushVantageAzureFunctionReservations(ILoggerFactory loggerFactory, IObsReservationService obsReservationService)
        {
            ArgumentNullException.ThrowIfNull(obsReservationService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionReservations>();
            _obsReservationService = obsReservationService;
            _logger.LogInformation($"Constructing {nameof(PushVantageAzureFunctionReservations)}");
        }

        /// <summary>
        /// Function for OBS Reservations
        /// </summary>
        /// <param name="req">req data</param>
        /// <returns>IActionResult</returns>
        [Function("PushVantageAzureFunctionOBSReservations")]
        [OpenApiOperation(operationId: "PushVantageAzureFunctionOBSReservations", tags: new[] { "PushVantageAzureFunctionOBSReservations" }, Summary = "Get OBS Reservations", Description = "This gets an existing OBS Reservation.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Booking), Summary = "Get OBS Reservations", Description = "Get OBS Reservations")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "OBS Reservations not found", Description = "OBS Reservations not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/vantagePortal/obs/reservations")] HttpRequestData req)
        {
            _logger.LogInformation("Executing function Get Reservatons");
            List<Booking> reservations = await _obsReservationService.GetAllReservationsAsync();
            _logger.LogInformation("Executed Function Get Reservatons");

            return new OkObjectResult(reservations);
        }
    }
}