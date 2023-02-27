namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;

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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/vantagePortal/obs/reservations")] HttpRequestData req)
        {
            _logger.LogInformation("Executing function Get Reservatons");
            List<Booking> reservations = await _obsReservationService.GetAllReservationsAsync();
            _logger.LogInformation("Executed Function Get Reservatons");

            return new OkObjectResult(reservations);
        }
    }
}